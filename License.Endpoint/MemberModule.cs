using System.Collections.Immutable;
using System.Linq;
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
               .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type,X-Requested-With,X-User-Token"));

            Get["/ping"] = parameters => Response.AsText("pong");

            Get["/"] = parameters =>
                {
                    var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                    if (AuthTokenCrud.CheckToken(token, session))
                    {
                        var members = MembersCrud.ListAll(session).ToImmutableArray();

                        return members.Length == 0 ? null : JsonConvert.SerializeObject(members, Formatting.Indented, new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    }
                    return HttpStatusCode.Unauthorized;


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

                    var member = JsonConvert.DeserializeObject<Member>(content);

                    MembersCrud.Save(member, session);

                    return HttpStatusCode.Accepted;
                };

            Post["/delete"] = parameters =>
                {
                    string content = Request.Body.ReadAsString();

                    var member = JsonConvert.DeserializeObject<Member>(content);

                    MembersCrud.Delete(member, session);

                    return HttpStatusCode.Accepted;
                };
        }
    }
}
