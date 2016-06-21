using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.IO;
using System.Windows;
using License.Crud;
using License.Mapping;
using License.Model;
using Nancy;
using Newtonsoft.Json;
using NHibernate;
using System.Reflection;
using System.Configuration;

namespace License.Endpoint
{
    public class MemberModule : NancyModule
    {
        private ISession Session;
        private string StoragePath = ConfigurationManager.AppSettings["FilePath"].ToString();

        public MemberModule()
            : base("/member")
        {
            this.Session = NHibernateHelper.OpenSession();

            After.AddItemToEndOfPipeline((ctx) => ctx.Response
               .WithHeader("Access-Control-Allow-Methods", "POST,GET")
               .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type,X-Requested-With,X-User-Token"));

            Get["/ping"] = parameters => Response.AsText("pong");

            Get["/all"] = parameters =>
                {
                    var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                    if (token == "null" || (token != "null" && AuthToken.CheckToken(token, Session)))
                    {
                        var members = MembersCrud.ListAll(Session).ToImmutableArray();

                        return members.Length == 0 ? null : JsonConvert.SerializeObject(members, Formatting.Indented, new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    }

                    return HttpStatusCode.Unauthorized;
                };

            Get["/"] = parameters =>
                {
                    var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                    if (token == "null" || (token != "null" && AuthToken.CheckToken(token, Session)))
                    {
                        var email = (string)this.Request.Query.Email;

                        var member = MembersCrud.Get(email, Session);

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

                    if (token == "null" || (token != "null" && AuthToken.CheckToken(token, Session)))
                    {
                        string content = Request.Body.ReadAsString();

                        var member = JsonConvert.DeserializeObject<Member>(content);
                        SaveOrUpdateMember(member, Session);

                        var user = AuthentificationLogic.CreateUser(member);
                        UserCrud.Save(user, Session);

                        AuthentificationLogic.SendNotificationEmail(user);

                        return HttpStatusCode.Accepted;
                    }

                    return HttpStatusCode.Unauthorized;
                };

            Post["/update"] = parameters =>
            {
                var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                if (token == "null" || (token != "null" && AuthToken.CheckToken(token, Session)))
                {
                    string content = Request.Body.ReadAsString();

                    var member = JsonConvert.DeserializeObject<Member>(content);

                    SaveOrUpdateMember(member, Session);

                    return HttpStatusCode.Accepted;
                }

                return HttpStatusCode.Unauthorized;
            };

            Post["/delete"] = parameters =>
                {
                    var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                    if (token == "null" || (token != "null" && AuthToken.CheckToken(token, Session)))
                    {

                        string content = Request.Body.ReadAsString();

                        var member = JsonConvert.DeserializeObject<Member>(content);

                        MembersCrud.Delete(member, Session);

                        return HttpStatusCode.Accepted;
                    }

                    return HttpStatusCode.Unauthorized;
                };

            Post["/photo"] = parameters =>
            {
                var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                if (token == "null" || (token != "null" && AuthToken.CheckToken(token, Session)))
                {                
                    if (!Directory.Exists(StoragePath))
                    {
                        Directory.CreateDirectory(StoragePath);
                    }

                    var fileName = Request.Files.First().Name;
                    var filePath = Path.Combine(StoragePath, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        Request.Files.First().Value.CopyTo(fileStream);
                    }

                    var user = AuthToken.FindToken(token, Session);
                    var member = MembersCrud.Get(user.Email, Session);

                    if (!string.IsNullOrEmpty(member.MemberPhoto))
                    {
                        File.Delete(Path.Combine(StoragePath, member.MemberPhoto));
                    }

                    member.MemberPhoto = fileName;
                    MembersCrud.Save(member, Session);
                    
                    return HttpStatusCode.Accepted;
                }

                return HttpStatusCode.Unauthorized;
            };
        }

        private static void SaveOrUpdateMember(Member member, ISession session)
        {
            var lectures = member.Lectures;
            member.Lectures = null;

            MembersCrud.Save(member, session);

            if (lectures.Count > 0)
            {
                SaveLectures(lectures, member, session);
            }
        }

        private static void SaveLectures(IList<Lecture> lectures, Member member, ISession session)
        {
            var dbLectures = LectureCrud.GetAll(session).ToImmutableArray();

            foreach (var lecture in dbLectures)
            {
                if (lecture.Teacher.Id == member.Id)
                {
                    LectureCrud.Delete(lecture, session);
                }
            }

            foreach (var lecture in lectures)
            {
                lecture.Teacher = member;
                lecture.Id = 0;
                LectureCrud.Save(lecture, session);
            }
        }
    }

    public class PhotoModel
    {
        public Member Member;
        public HttpFile File;
    }
}