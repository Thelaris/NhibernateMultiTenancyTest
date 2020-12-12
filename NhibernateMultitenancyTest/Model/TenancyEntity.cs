using System;
using System.Collections.Generic;
using System.Text;

namespace NhibernateMultitenancyTest.Model
{
    public class TenancyEntity : Entity
    {
        public virtual Guid TenantId { get; set; }
    }
}
