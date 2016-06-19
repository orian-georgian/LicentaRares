using System;
using System.Collections;
using System.Collections.Generic;
using License.Model;
using NHibernate;
using NHibernate.Transform;

namespace License.Crud
{
    public class UserCrud
    {
        public static User Get(int id, ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var user = session.Get<User>(id);

                transaction.Commit();
                return user;
            }
        }

        public static User Get(string email, ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var User = session.CreateSQLQuery("SELECT * FROM [University].[dbo].[Users] Where Email = :email")
                                    .SetParameter("email", email)
                                    .SetResultTransformer(Transformers.AliasToBean(typeof(User)))
                                    .List<User>();

                transaction.Commit();
                return User.Count == 0 ? null : User[0];
            }
        }

        public static void Save(User user, ISession session)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user must not be null!");
            }
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(user);

                transaction.Commit();
            }

        }

        public static void Delete(User user, ISession session)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user must not be null!");
            }

            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(user);

                transaction.Commit();
            }

        }

        public static IEnumerable<User> ListAll(ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                IList<User> User = session
                        .CreateCriteria(typeof(User))
                        .List<User>();

                return User;
            }
        }
    }
}
