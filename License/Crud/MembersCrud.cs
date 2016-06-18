using System;
using System.Collections;
using System.Collections.Generic;
using License.Mapping;
using License.Model;
using NHibernate;

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
