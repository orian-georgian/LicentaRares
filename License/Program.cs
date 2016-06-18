using System;
using System.Collections.Generic;
using License.Mapping;
using License.Model;
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

                    //foreach (var m in members)
                    //{
                    //    Console.WriteLine(m.FullName);
                    //   // Console.WriteLine(m.User.UserName);
                    //}

                    Console.ReadKey();

                    tx.Commit();

                }
            }

        }
    }
}
