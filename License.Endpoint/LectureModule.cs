using System.Linq;
using License.Crud;
using License.Mapping;
using License.Model;
using Nancy;
using Newtonsoft.Json;
using NHibernate;

namespace License.Endpoint
{
    public class LectureModule : NancyModule
    {
        private ISession session;

        public LectureModule()
            : base("/lecture")
        {
            session = NHibernateHelper.OpenSession();
            After.AddItemToEndOfPipeline((ctx) => ctx.Response
               .WithHeader("Access-Control-Allow-Methods", "POST,GET")
               .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type,X-Requested-With, X-User-Token"));

            Post["/new"] = p =>
            {
                var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                if (token != "null" && AuthToken.CheckToken(token, session))
                {
                    string content = Request.Body.ReadAsString();

                    var lecture = JsonConvert.DeserializeObject<Lecture>(content);
                    LectureCrud.Save(lecture, session);

                    return HttpStatusCode.OK;
                }

                return HttpStatusCode.Unauthorized;
            };

            Post["/delete"] = p =>
            {
                var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                if (token != "null" && AuthToken.CheckToken(token, session))
                {
                    string content = Request.Body.ReadAsString();

                    var lecture = JsonConvert.DeserializeObject<Lecture>(content);
                    LectureCrud.Delete(lecture, session);

                    return HttpStatusCode.OK;
                }

                return HttpStatusCode.Unauthorized;
            };
        }
    }
}
