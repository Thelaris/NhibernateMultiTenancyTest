//using System;
//using System.IO;

//namespace NhibernateMultitenancyTest.Data.Config
//{
//    public class NhibernateConfig
//    {
//        private bool _deleteExistingDb;
//        private bool _configureMultiTenancy;
//        private Guid _tenantId;

//        public ISessionFactory CreateNewSessionFactory(bool deleteExistingDb = false)
//        {
//            //_tenantId = tenantId;
//            //_configureMultiTenancy = configureMultiTenancy;
//            _deleteExistingDb = deleteExistingDb;

//            return Fluently.Configure()
//                .Database(SQLiteConfiguration.Standard.UsingFile("test.db"))
//                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
//                .ExposeConfiguration(BuildSchema)
//                //.ExposeConfiguration(ConfigureMultiTenancy)
//                .BuildSessionFactory();
//        }

//        private void BuildSchema(Configuration config)
//        {
//            if (_deleteExistingDb)
//            {
//                if (File.Exists("test.db"))
//                {
//                    File.Delete("test.db");
//                }
//            }

//        }

//        //private void ConfigureMultiTenancy(Configuration config)
//        //{
//        //    if (_configureMultiTenancy)
//        //    {
//        //        config.SetInterceptor();
//        //    }
//        //}
//    }
//}
