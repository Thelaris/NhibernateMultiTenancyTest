using System;
using System.Collections.Generic;
using System.Text;
using NhibernateMultitenancyTest.Config;
using NhibernateMultitenancyTest.Model;

namespace NhibernateMultitenancyTest.Mappings
{
    public class TenancyEntityMap<T> : EntityMap<T> where T : TenancyEntity
    {
        public TenancyEntityMap()
        {
            Map(m => m.TenantId);

            ApplyFilter<MultiTenantFilter>("TenantId IN (:TenantIds)");
        }
    }
}
