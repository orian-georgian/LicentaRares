using License.Crud;
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

            Post["/login"] = p =>
                {
                    var content = Request.Body.ReadAsString();

                    var model = JsonConvert.DeserializeObject<LoginModel>(content);

                    var user = UserCrud.Get(model.Email, session);

                    if (user == null || user.Password != model.Password)
                    {
                        return Response.AsText("Username sau parola incorecte!").WithStatusCode(HttpStatusCode.NotFound);
                    }

                    var token = AuthTokenCrud.SetToken();

                    AuthTokenCrud.InsertToken(user, token, session);

                    return JsonConvert.SerializeObject(user, Formatting.Indented);
                };

            Post["/logout/"] = p =>
            {
                var email = Request.Body.ReadAsString().Trim();

                var user = UserCrud.Get(email, session);

                AuthTokenCrud.InsertToken(user, string.Empty, session);

                return HttpStatusCode.Accepted;
            };
        }

        public class LoginModel
        {
            public string Email;
            public string Password;
        }
    }

}
