﻿using System;
using System.Linq;
using License.Crud;
using License.Model;
using NHibernate;
using NHibernate.Transform;

namespace License
{
    public class AuthToken
    {
        public static string SetToken()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 30)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public static void InsertToken(User user, string token, ISession session)
        {
            user.AuthToken = token;

            UserCrud.Save(user, session);
        }

        public static bool CheckToken(string token, ISession session)
        {
            if (FindToken(token, session) != null)
            {
                return true;
            }
            return false;
        }

        public static User FindToken(string token, ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var authentifiedUsers = session.CreateSQLQuery("SELECT * FROM [University].[dbo].[Users] Where AuthToken = :token")
                                    .SetParameter("token", token)
                                    .SetResultTransformer(Transformers.AliasToBean(typeof(User)))
                                    .List<User>();

                transaction.Commit();
                return authentifiedUsers.Count == 0 ? null : authentifiedUsers[0];
            }
        }
    }
}
