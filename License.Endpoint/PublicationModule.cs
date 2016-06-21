using System.Linq;
using License.Crud;
using License.Mapping;
using Nancy;
using Newtonsoft.Json;
using NHibernate;

namespace License.Endpoint
{
    public class PublicationModule : NancyModule
    {
        ISession session;

        public PublicationModule()
            : base("/publication")
        {
            this.session = NHibernateHelper.OpenSession();

            After.AddItemToEndOfPipeline((ctx) => ctx.Response
               .WithHeader("Access-Control-Allow-Methods", "POST,GET")
               .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type,X-Requested-With,X-User-Token"));

            Get["/{year}"] = parameters =>
            {
                var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                if (token == "null" || (token != "null" && AuthToken.CheckToken(token, session)))
                {
                    var year = (int)parameters.year;
                    var members = PublicationCrud.GetAll(year, session);

                    return members.Count() == 0 ? null : JsonConvert.SerializeObject(members, Formatting.Indented, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                }

                return HttpStatusCode.Unauthorized;
            };
        }

    }
}
