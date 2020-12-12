using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using NhibernateMultitenancyTest.Model;

namespace NhibernateMultitenancyTest.Mappings
{
    public class EntityMap<T> : ClassMap<T> where T : Entity
    {
        public EntityMap()
        {
            Id(m => m.Id)
                .GeneratedBy.GuidComb();
        }
    }
}
