using System;
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

                    if (token == "null" || (token != "null" && AuthTokenCrud.CheckToken(token, session)))
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
                    var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                    if (token == "null" || (token != "null" && AuthTokenCrud.CheckToken(token, session)))
                    {
                        var id = (int)parameters.Id;

                        var member = MembersCrud.Get(id, session);

                        return JsonConvert.SerializeObject(member, Formatting.Indented, new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    }
                    return HttpStatusCode.Unauthorized;
                };

            Post["/new"] = parameters =>
                {
                    var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                    if (token == "null" || (token != "null" && AuthTokenCrud.CheckToken(token, session)))
                    {
                        string content = Request.Body.ReadAsString();

                        var member = JsonConvert.DeserializeObject<Member>(content);
                        MembersCrud.Save(member, session);

                        var user = CreateUser(member);
                        UserCrud.Save(user, session);

                        SendNotificationEmail(user);

                        return HttpStatusCode.Accepted;
                    }
                    return HttpStatusCode.Unauthorized;
                };

            Post["/update"] = parameters =>
            {
                var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                if (token == "null" || (token != "null" && AuthTokenCrud.CheckToken(token, session)))
                {
                    string content = Request.Body.ReadAsString();

                    var member = JsonConvert.DeserializeObject<Member>(content);

                    MembersCrud.Save(member, session);

                    return HttpStatusCode.Accepted;
                }
                return HttpStatusCode.Unauthorized;
            };

            Post["/delete"] = parameters =>
                {
                    var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                    if (token == "null" || (token != "null" && AuthTokenCrud.CheckToken(token, session)))
                    {

                        string content = Request.Body.ReadAsString();

                        var member = JsonConvert.DeserializeObject<Member>(content);

                        MembersCrud.Delete(member, session);

                        return HttpStatusCode.Accepted;
                    }
                    return HttpStatusCode.Unauthorized;
                };
        }

        private static User CreateUser(Member member)
        {
            var user = new User();

            user.Email = member.Email;
            user.Password = CreatePassword();
            user.Name = member.FullName;
            user.AuthRole = "Member";
            user.AuthToken = string.Empty;

            return user;
        }

        private static string CreatePassword()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        private static void SendNotificationEmail(User user)
        {
            var toAddress = user.Email;
            var mailSubject = "User password for localhost site";
            var mailBody = "Your account information is: \n Email: "
                + user.Email + " \nPassword: "
                + user.Password
                + " \n Please log in and change your password!";

            SmtpService.SendEmail(toAddress, mailSubject, mailBody);
        }
    }
}
