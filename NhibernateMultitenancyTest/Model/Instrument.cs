using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace NhibernateMultitenancyTest.Model
{
    public class Instrument : TenancyEntity
    {
        public virtual int Number { get; set; }
        public virtual IEnumerable<Account> Accounts { get { return _accounts.AsEnumerable(); } }

        private readonly IList<Account> _accounts = new List<Account>();

        public virtual void AddAccount(Account account)
        {
            if (TenantId != Program._currentWriteTenantId)
            {
                throw new InvalidOperationException($"Cannot add account to Instrument#: {Number}. Current write Tenant ID ({Program._currentWriteTenantId}) does not match Instrument's Tenant ID ({TenantId}).");
            }

            _accounts.Add(account);
        }
    }
}
