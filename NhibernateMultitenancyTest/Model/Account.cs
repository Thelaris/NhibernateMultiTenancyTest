using System;
using System.Collections.Generic;
using System.Text;

namespace NhibernateMultitenancyTest.Model
{
    public class Account : TenancyEntity
    {
        public virtual int AccountNumber { get; set; }
        public virtual Instrument Instrument { get; set; }
    }
}
