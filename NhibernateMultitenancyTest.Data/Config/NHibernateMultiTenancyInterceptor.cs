//using System;
//using System.Reflection;

//namespace NhibernateMultitenancyTest.Data.Config
//{
//    public class NHibernateMultiTenancyInterceptor : EmptyInterceptor
//    {
//        private readonly Guid _tenantId;

//        public NHibernateMultiTenancyInterceptor(Guid tenantId)
//        {
//            _tenantId = tenantId;
//        }

//        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
//        {
//            var index = Array.IndexOf(propertyNames, "TenantId");

//            if (index == -1)
//            {
//                return false; // Saves without TenantId on the Entity
//            }

//            state[index] = _tenantId;

//            entity.GetType()
//                .GetField("TenantId", BindingFlags.Instance | BindingFlags.Public)
//                .SetValue(entity, _tenantId);

//            return base.OnSave(entity, id, state, propertyNames, types);
//        }
//    }
//}
