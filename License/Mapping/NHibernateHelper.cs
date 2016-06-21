using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using License.Model;
using NHibernate;
using NHibernate.Cfg;
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
                  .ConnectionString(@"Data Source=Gicutzu;Initial Catalog=University;Integrated Security=True")
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

        //public static ISessionFactory BuildSessionFactory()
        //{
        //    AutoPersistenceModel model = CreateMappings();

        //    return Fluently.Configure()
        //        .Database(MsSqlConfiguration.MsSql2012
        //        .ConnectionString(c => c
        //            .Server("ANDREEA-PC")
        //            .Database("University")))
        //                            .Mappings(m => m
        //            .AutoMappings.Add(model))
        //        .ExposeConfiguration(BuildSchema)
        //                          .BuildSessionFactory();
        //}

        //private static AutoPersistenceModel CreateMappings()
        //{
        //    return AutoMap
        //        .Assembly(System.Reflection.Assembly.GetCallingAssembly())
        //        .Where(t => t.Namespace == "License.Model");
        //}

        private static void BuildSchema(Configuration config)
        {
            new SchemaExport(config).Create(false, true);
        }
    }
}
