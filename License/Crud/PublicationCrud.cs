using System.Collections.Generic;
using License.Model;
using NHibernate;

namespace License.Crud
{
    public class PublicationCrud
    {
        public static IEnumerable<Member> GetAll(int year, ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                IQueryOver<License.Model.Member, Publication> query =
                                session.QueryOver<License.Model.Member>()
                                    .JoinQueryOver<Publication>(c => c.Publications)
                                        .Where(k => k.PublicationDate.Year == year);

                var result = query.List();

                transaction.Commit();
                return result;
            }
        }
    }
}
