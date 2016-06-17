using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using License.Mapping;
using License.Mapping;
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
            Get["/ping"] = parameters => Response.AsText("pong");

            Get["/"] = parameters =>
            {
                var members = GetMembers().ToImmutableArray();

                return members.Length == 0 ? null : JsonConvert.SerializeObject(members, Formatting.Indented);
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
    }
}
