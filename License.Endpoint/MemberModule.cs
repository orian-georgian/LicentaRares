﻿using System.Collections.Generic;
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

            Get["/all"] = parameters =>
                {
                    var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                    if (token == "null" || (token != "null" && AuthToken.CheckToken(token, session)))
                    {
                        var members = MembersCrud.ListAll(session).ToImmutableArray();

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

                    if (token == "null" || (token != "null" && AuthToken.CheckToken(token, session)))
                    {
                        var email = (string)this.Request.Query.Email;

                        var member = MembersCrud.Get(email, session);

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

                    if (token == "null" || (token != "null" && AuthToken.CheckToken(token, session)))
                    {
                        string content = Request.Body.ReadAsString();

                        var member = JsonConvert.DeserializeObject<Member>(content);
                        SaveOrUpdateMember(member, session);

                        var user = AuthentificationLogic.CreateUser(member);
                        UserCrud.Save(user, session);

                        AuthentificationLogic.SendNotificationEmail(user);

                        return HttpStatusCode.Accepted;
                    }

                    return HttpStatusCode.Unauthorized;
                };

            Post["/update"] = parameters =>
            {
                var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                if (token == "null" || (token != "null" && AuthToken.CheckToken(token, session)))
                {
                    string content = Request.Body.ReadAsString();

                    var member = JsonConvert.DeserializeObject<Member>(content);

                    SaveOrUpdateMember(member, session);

                    return HttpStatusCode.Accepted;
                }

                return HttpStatusCode.Unauthorized;
            };

            Post["/delete"] = parameters =>
                {
                    var token = this.Request.Headers["X-User-Token"].FirstOrDefault().ToString();

                    if (token == "null" || (token != "null" && AuthToken.CheckToken(token, session)))
                    {

                        string content = Request.Body.ReadAsString();

                        var member = JsonConvert.DeserializeObject<Member>(content);

                        MembersCrud.Delete(member, session);

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

            SaveLectures(lectures, member, session);
        }

        private static void SaveLectures(IList<Lecture> lectures, Member member, ISession session)
        {
            var dbLectures = LectureCrud.GetAllFor(session).ToImmutableArray();

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
                LectureCrud.Save(lecture, session);
            }
        }
    }
}