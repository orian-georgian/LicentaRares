using System.Linq;
using License.Crud;
using License;
using License.Mapping;
using Nancy;
using Newtonsoft.Json;
using NHibernate;

namespace License.Endpoint
{
    public class LoginModule : NancyModule
    {
        private ISession session;
        public LoginModule()
            : base("/")
        {
            session = NHibernateHelper.OpenSession();
            After.AddItemToEndOfPipeline((ctx) => ctx.Response
               .WithHeader("Access-Control-Allow-Methods", "POST,GET")
               .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type,X-Requested-With, X-User-Token"));

            Get["/login"] = p =>
            {
                var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                if (token != "null" && AuthToken.CheckToken(token, session))
                {
                    var user = AuthToken.FindToken(token, session);

                    return JsonConvert.SerializeObject(user, Formatting.Indented);
                }

                return HttpStatusCode.Unauthorized;
            };

            Post["/login"] = p =>
                {
                    var content = Request.Body.ReadAsString();

                    var model = JsonConvert.DeserializeObject<LoginModel>(content);

                    var user = UserCrud.Get(model.Email, session);

                    if (user == null || user.Password != model.Password)
                    {
                        return Response.AsText("Username sau parola incorecte!").WithStatusCode(HttpStatusCode.NotFound);
                    }

                    var token = AuthToken.SetToken();

                    AuthToken.InsertToken(user, token, session);

                    return JsonConvert.SerializeObject(user, Formatting.Indented);
                };

            Post["/logout/"] = p =>
            {
                var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                if (token != "null" && AuthToken.CheckToken(token, session))
                {
                    var user = AuthToken.FindToken(token, session);
                    AuthToken.InsertToken(user, string.Empty, session);

                    return HttpStatusCode.Accepted;
                }

                return HttpStatusCode.Unauthorized;
            };
        }

        public class LoginModel
        {
            public string Email;
            public string Password;
        }
    }

}
