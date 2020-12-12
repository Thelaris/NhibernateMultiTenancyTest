using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace NhibernateMultitenancyTest.Config
{
    public static class NhibernateConfig
    {
        public static ISessionFactory SessionFactory { get; } = CreateNewSessionFactory(false);

        private static bool _deleteExistingDb;

        //private static bool _configureMultiTenancy;
        //private static Guid _tenantId;

        public static ISessionFactory CreateNewSessionFactory(bool deleteExistingDb = false)
        {
            if (SessionFactory == null)
            {
                //_tenantId = tenantId;
                //_configureMultiTenancy = configureMultiTenancy;
                _deleteExistingDb = deleteExistingDb;

                return Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.UsingFile("test.db")
                        //.ShowSql()
                        //.FormatSql()
                    )
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
                    .ExposeConfiguration(BuildSchema)
                    //.ExposeConfiguration(ConfigureMultiTenancy)
                    .BuildSessionFactory();
            }

            return SessionFactory;
        }

        private static void BuildSchema(Configuration config)
        {
            if (_deleteExistingDb)
            {
                if (File.Exists("test.db"))
                {
                    File.Delete("test.db");
                }
            }

            new SchemaExport(config).Create(false, true);
        }

        //private void ConfigureMultiTenancy(Configuration config)
        //{
        //    if (_configureMultiTenancy)
        //    {
        //        config.SetInterceptor();
        //    }
        //}
    }
}
