using System.Collections.Immutable;
using License.Crud;
using License.Model;
using Nancy;
using Newtonsoft.Json;

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
                    var members = MembersCrud.ListAll().ToImmutableArray();

                    return members.Length == 0 ? null : JsonConvert.SerializeObject(members, Formatting.Indented);
                };
            Get["/{Id}"] = parameters =>
                {
                    var id = (int)parameters.Id;

                    var member = MembersCrud.Get(id);

                    return JsonConvert.SerializeObject(member, Formatting.Indented);
                };

            Post["/new"] = parameters =>
                {
                    string content = Request.Body.ReadAsString();

                    var member = JsonConvert.DeserializeObject<Members>(content);

                    MembersCrud.Save(member);

                    return HttpStatusCode.Accepted;
                };

            Post["/delete"] = parameters =>
                {
                    string content = Request.Body.ReadAsString();

                    var member = JsonConvert.DeserializeObject<Members>(content);

                    MembersCrud.Delete(member);

                    return HttpStatusCode.Accepted;
                };
        }
    }
}
