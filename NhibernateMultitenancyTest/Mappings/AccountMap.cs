using System;
using System.Collections.Generic;
using System.Text;
using NhibernateMultitenancyTest.Model;

namespace NhibernateMultitenancyTest.Mappings
{
    public class AccountMap : TenancyEntityMap<Account>
    {
        public AccountMap()
        {
            Map(m => m.AccountNumber);
            References(m => m.Instrument)
                .Column("InstrumentId");
        }
    }
}
