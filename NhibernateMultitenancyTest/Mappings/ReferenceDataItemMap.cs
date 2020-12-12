using System;
using System.Collections.Generic;
using System.Text;
using NhibernateMultitenancyTest.Model;

namespace NhibernateMultitenancyTest.Mappings
{
    public class ReferenceDataItemMap : EntityMap<ReferenceDataItem>
    {
        public ReferenceDataItemMap()
        {
            Map(m => m.Value);
        }
    }
}
