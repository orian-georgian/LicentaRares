using System;
using System.Collections.Generic;
using License.Model;
using NHibernate;
namespace License.Crud
{
    public class LectureCrud
    {
        public static void Save(Lecture lecture, ISession session)
        {
            if (lecture == null)
            {
                throw new ArgumentNullException("lecture must not be null!");
            }
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(lecture);

                transaction.Commit();
            }
        }

        public static void Delete(Lecture lecture, ISession session)
        {
            if (lecture == null)
            {
                throw new ArgumentNullException("lecture must not be null!");
            }

            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(lecture);

                transaction.Commit();
            }

        }

        public static IList<Lecture> GetAll(ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                IList<Lecture> lectures = session
                        .CreateCriteria(typeof(Lecture))
                        .List<Lecture>();

                transaction.Commit();

                return lectures;
            }
        }
    }
}
