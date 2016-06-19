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
        public static Member Get(int id, ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var member = session.Get<Member>(id);

                transaction.Commit();
                return member;
            }
        }

        public static Member Get(string email, ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var members = session.CreateSQLQuery("SELECT * FROM [University].[dbo].[Members] Where Email = :email")
                                    .SetParameter("email", email)
                                    .SetResultTransformer(Transformers.AliasToBean(typeof(Member)))
                                    .List<Member>();

                transaction.Commit();
                return members.Count == 0 ? null : members[0];
            }
        }

        public static void Save(Member member, ISession session)
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

        public static void Delete(Member member, ISession session)
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

        public static IEnumerable<Member> ListAll(ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                IList<Member> members = session
                        .CreateCriteria(typeof(Member))
                        .List<Member>();

                return members;
            }
        }
    }
}
