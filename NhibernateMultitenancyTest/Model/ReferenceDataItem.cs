using System;
using System.Collections.Generic;
using System.Text;

namespace NhibernateMultitenancyTest.Model
{
    public class ReferenceDataItem : Entity
    {
        public virtual string Value { get; set; }
    }
}
