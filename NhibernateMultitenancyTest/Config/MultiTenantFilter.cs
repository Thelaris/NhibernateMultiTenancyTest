using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using NHibernate;

namespace NhibernateMultitenancyTest.Config
{
    public class MultiTenantFilter : FilterDefinition
    {
        public MultiTenantFilter()
        {
            //WithName("MultiTenantFilter").AddParameter("TenantId", NHibernateUtil.Guid);
            WithName("MultiTenantFilter");
            WithCondition("TenantId IN (:TenantIds)");
            AddParameter("TenantIds", NHibernateUtil.Guid);
        }
    }
}
