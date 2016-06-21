using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using License.Model;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace License.Mapping
{
    public class NHibernateHelper
    {
        private static ISessionFactory sessionFactory;

        public static ISession OpenSession()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ShowSql()
                  .ConnectionString(@"Data Source=ANDREEA-PC;Initial Catalog=University;Integrated Security=True")
                              .ShowSql()
                )
               .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssemblyOf<Member>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg)
                                                .Execute(false, true))
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}
