using System;
using System.Collections.Generic;
using License.Mapping;
using NHibernate;

namespace License
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    IList<Members> members = session
                             .CreateCriteria(typeof(Members))
                             .List<Members>();

                    Console.Write(members);
                    Console.ReadKey();

                    tx.Commit();

                }
            }

        }
    }
}
