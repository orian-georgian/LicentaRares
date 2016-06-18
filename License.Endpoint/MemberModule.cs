using System.Collections.Immutable;
using License.Crud;
using License.Mapping;
using License.Model;
using Nancy;
using Newtonsoft.Json;
using NHibernate;

namespace License.Endpoint
{
    public class MemberModule : NancyModule
    {
        private ISession session;

        public MemberModule()
            : base("/member")
        {
            this.session = NHibernateHelper.OpenSession();

            After.AddItemToEndOfPipeline((ctx) => ctx.Response
               .WithHeader("Access-Control-Allow-Methods", "POST,GET")
               .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type,X-Requested-With"));

            Get["/ping"] = parameters => Response.AsText("pong");

            Get["/"] = parameters =>
                {
                    var members = MembersCrud.ListAll(session).ToImmutableArray();

                    return members.Length == 0 ? null : JsonConvert.SerializeObject(members, Formatting.Indented, new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                };
            Get["/{Id}"] = parameters =>
                {
                    var id = (int)parameters.Id;

                    var member = MembersCrud.Get(id, session);

                    return JsonConvert.SerializeObject(member, Formatting.Indented, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                };

            Post["/new"] = parameters =>
                {
                    string content = Request.Body.ReadAsString();

                    var member = JsonConvert.DeserializeObject<Members>(content);

                    MembersCrud.Save(member, session);

                    return HttpStatusCode.Accepted;
                };

            Post["/delete"] = parameters =>
                {
                    string content = Request.Body.ReadAsString();

                    var member = JsonConvert.DeserializeObject<Members>(content);

                    MembersCrud.Delete(member, session);

                    return HttpStatusCode.Accepted;
                };
        }
    }
}
