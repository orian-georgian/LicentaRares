using System.Linq;
using License.Mapping;
using Nancy;
using Newtonsoft.Json;
using NHibernate;
namespace License.Endpoint
{
    public class ContactModule : NancyModule
    {
        private ISession session;

        public ContactModule()
            : base("/")
        {
            this.session = NHibernateHelper.OpenSession();

            After.AddItemToEndOfPipeline((ctx) => ctx.Response
               .WithHeader("Access-Control-Allow-Methods", "POST,GET")
               .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type,X-Requested-With,X-User-Token"));

            Post["/contact"] = parameters =>
            {
                var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                if (token == "null" || (token != "null" && AuthToken.CheckToken(token, session)))
                {
                    var content = Request.Body.ReadAsString();

                    var model = JsonConvert.DeserializeObject<ContactModel>(content);

                    var mailSubject = "Mail from " + model.Name + " with email address " + model.Email;
                    var mailMessage = "Name :" + model.Name + "\n Email: " + model.Email + "\n Message: \n" + model.Message;
                    SmtpService.SendEmail(string.Empty, mailSubject, mailMessage);

                    return HttpStatusCode.Accepted;
                }

                return HttpStatusCode.Unauthorized;
            };

        }

        public class ContactModel
        {
            public string Name;
            public string Email;
            public string Message;
        }
    }
}
