using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using License.Mapping;
using License.Model;
using Nancy;
using Newtonsoft.Json;
using NHibernate;

namespace License.Endpoint
{
    public class MemberModule : NancyModule
    {
        public MemberModule()
            : base("/member")
        {
            After.AddItemToEndOfPipeline((ctx) => ctx.Response
               .WithHeader("Access-Control-Allow-Methods", "POST,GET")
               .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type,X-Requested-With"));

            Get["/ping"] = parameters => Response.AsText("pong");

            Get["/"] = parameters =>
            {
                var members = GetMembers().ToImmutableArray();

                return members.Length == 0 ? null : JsonConvert.SerializeObject(members, Formatting.Indented);
            };

            Post["/new"] = parameters =>
                {
                    string content = Request.Body.ReadAsString();

                    var member = JsonConvert.DeserializeObject<Members>(content);

                    AddMember(member);
                    return HttpStatusCode.Accepted;
                };
        }

        private IEnumerable<Members> GetMembers()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    IList<Members> members = session
                            .CreateCriteria(typeof(Members))
                            .List<Members>();

                    return members;
                }
            }
        }

        private void AddMember(Members member)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(member);

                    transaction.Commit();
                }
            }
        }
    }
}
