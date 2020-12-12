using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using NhibernateMultitenancyTest.Model;

namespace NhibernateMultitenancyTest.Mappings
{
    public class InstrumentMap : TenancyEntityMap<Instrument>
    {
        public InstrumentMap()
        {
            Map(m => m.Number);
            HasMany(m => m.Accounts)
                .Access.ReadOnlyPropertyThroughCamelCaseField(Prefix.Underscore)
                .KeyColumn("InstrumentId")
                .Cascade.AllDeleteOrphan()
                .Inverse();
        }
    }
}
