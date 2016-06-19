using System;
using System.Collections;
using System.Collections.Generic;
using License.Model;
using NHibernate;
using NHibernate.Transform;

namespace License.Crud
{
    public class MembersCrud
    {
        public static Members Get(int id, ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var member = session.Get<Members>(id);

                transaction.Commit();
                return member;
            }
        }

        public static Members Get(string email, ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var members = session.CreateSQLQuery("SELECT * FROM [University].[dbo].[Members] Where Email = :email")
                                    .SetParameter("email", email)
                                    .SetResultTransformer(Transformers.AliasToBean(typeof(Members)))
                                    .List<Members>();

                transaction.Commit();
                return members.Count == 0 ? null : members[0];
            }
        }

        public static void Save(Members member, ISession session)
        {
            if (member == null)
            {
                throw new ArgumentNullException("member must not be null!");
            }
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(member);

                transaction.Commit();
            }

        }

        public static void Delete(Members member, ISession session)
        {
            if (member == null)
            {
                throw new ArgumentNullException("member must not be null!");
            }

            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(member);

                transaction.Commit();
            }

        }

        public static IEnumerable<Members> ListAll(ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                IList<Members> members = session
                        .CreateCriteria(typeof(Members))
                        .List<Members>();

                return members;
            }
        }
    }
}
