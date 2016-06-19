﻿using License.Crud;
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
               .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type,X-Requested-With, X-Member-Token"));

            Post["/login"] = p =>
                {
                    var content = Request.Body.ReadAsString();

                    var model = JsonConvert.DeserializeObject<LoginModel>(content);

                    var member = MembersCrud.Get(model.Email, session);

                    if (member == null || member.Password != model.Password)
                    {
                        return Response.AsText("Username sau parola incorecte!").WithStatusCode(HttpStatusCode.NotFound);
                    }

                    var token = AuthTokenCrud.SetToken();

                    AuthTokenCrud.InsertToken(member, token, session);

                    return Response.AsText(token);
                };

            Post["/logout/"] = p =>
            {
                var email = Request.Body.ReadAsString().Trim();

                var member = MembersCrud.Get(email, session);

                AuthTokenCrud.InsertToken(member, string.Empty, session);

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
