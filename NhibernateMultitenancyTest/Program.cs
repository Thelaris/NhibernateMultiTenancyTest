using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Context;
using NHibernate.Mapping;
using NhibernateMultitenancyTest.Config;
using NhibernateMultitenancyTest.Model;

namespace NhibernateMultitenancyTest
{
    class Program
    {
        private static readonly Guid TenantOne = Guid.NewGuid();
        private static readonly Guid TenantTwo = Guid.NewGuid();
        private static Guid[] _currentReadTenantIds;
        public static Guid _currentWriteTenantId;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine($"Tenant One ID: {TenantOne}");
            Console.WriteLine($"Tenant Two ID: {TenantTwo}");

            Console.WriteLine(Environment.NewLine);

            _currentReadTenantIds = new Guid[] { TenantOne, TenantTwo };

            CreateTestData();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);

            _currentReadTenantIds = new Guid[] { TenantOne, TenantTwo };
            using (var uow = new NHUnitOfWork())
            {
                uow.OpenSession(_currentReadTenantIds);
                uow.BeginTransaction();

                var allInstruments = GetInstruments(uow);
                var allAccounts = GetAccounts(uow);
                var allReferenceDataItems = GetReferenceDataItems(uow);

                Console.WriteLine("All Instruments:");
                foreach (var instrument in allInstruments)
                {
                    Console.WriteLine($"{instrument.TenantId} - Instrument#: {instrument.Number}");
                }

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine("All Accounts:");
                foreach (var account in allAccounts)
                {
                    Console.WriteLine(
                        $"{account.TenantId} - Instrument#: {account.Instrument.Number} Account#: {account.AccountNumber}");
                }

                Console.WriteLine(Environment.NewLine);


                Console.WriteLine("All Reference Data Items:");
                foreach (var refDataItem in allReferenceDataItems)
                {
                    Console.WriteLine($"{refDataItem.Value}");
                }

                Console.WriteLine(Environment.NewLine);
            }


            _currentReadTenantIds = new Guid[] { TenantOne };
            using (var uow = new NHUnitOfWork())
            {
                uow.OpenSession(_currentReadTenantIds);
                uow.BeginTransaction();

                var tenantOneInstruments = GetInstruments(uow);
                var tenantOneAccountsFromInstruments = tenantOneInstruments.SelectMany(i => i.Accounts);
                var tenantOneAccounts = GetAccounts(uow);

                Console.WriteLine("Tenant One Instruments:");
                foreach (var instrument in tenantOneInstruments)
                {
                    Console.WriteLine($"{instrument.TenantId} - Instrument#: {instrument.Number}");
                }

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine("Tenant One Accounts:");
                foreach (var account in tenantOneAccounts)
                {
                    Console.WriteLine(
                        $"{account.TenantId} - Instrument#: {account.Instrument.Number} Account#: {account.AccountNumber}");
                }

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine("Tenant One Accounts From Instruments:");
                foreach (var account in tenantOneAccountsFromInstruments)
                {
                    Console.WriteLine(
                        $"{account.TenantId} - Instrument#: {account.Instrument.Number} Account#: {account.AccountNumber}");
                }

                Console.WriteLine(Environment.NewLine);
            }


            _currentReadTenantIds = new Guid[] { TenantTwo };
            using (var uow = new NHUnitOfWork())
            {
                uow.OpenSession(_currentReadTenantIds);
                uow.BeginTransaction();

                var tenantTwoInstruments = GetInstruments(uow);
                var tenantTwoAccountsFromInstruments = tenantTwoInstruments.SelectMany(i => i.Accounts);
                var tenantTwoAccounts = GetAccounts(uow);

                Console.WriteLine("Tenant Two Instruments:");
                foreach (var instrument in tenantTwoInstruments)
                {
                    Console.WriteLine($"{instrument.TenantId} - Instrument#: {instrument.Number}");
                }

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine("Tenant Two Accounts:");
                foreach (var account in tenantTwoAccounts)
                {
                    Console.WriteLine(
                        $"{account.TenantId} - Instrument#: {account.Instrument.Number} Account#: {account.AccountNumber}");
                }

                Console.WriteLine(Environment.NewLine);

                Console.WriteLine("Tenant Two Accounts From Instruments:");
                foreach (var account in tenantTwoAccountsFromInstruments)
                {
                    Console.WriteLine(
                        $"{account.TenantId} - Instrument#: {account.Instrument.Number} Account#: {account.AccountNumber}");
                }

                Console.WriteLine(Environment.NewLine);
            }


            _currentReadTenantIds = new Guid[] { TenantOne };
            using (var uow = new NHUnitOfWork())
            {
                uow.OpenSession(_currentReadTenantIds);
                uow.BeginTransaction();

                var tenantOneRefDataItems = GetReferenceDataItems(uow);

                Console.WriteLine("Tenant One Reference Data Items:");
                foreach (var refDataItem in tenantOneRefDataItems)
                {
                    Console.WriteLine($"{refDataItem.Value}");
                }

                Console.WriteLine(Environment.NewLine);
            }


            _currentReadTenantIds = new Guid[] { TenantTwo };
            using (var uow = new NHUnitOfWork())
            {
                uow.OpenSession(_currentReadTenantIds);
                uow.BeginTransaction();

                var tenantTwoRefDataItems = GetReferenceDataItems(uow);

                Console.WriteLine("Tenant Two Reference Data Items:");
                foreach (var refDataItem in tenantTwoRefDataItems)
                {
                    Console.WriteLine($"{refDataItem.Value}");
                }

                Console.WriteLine(Environment.NewLine);
            }
        }

        private static void CreateTestData()
        {
            _currentWriteTenantId = TenantOne;
            var tenantOneInstrument = CreateInstrument(1);

            _currentWriteTenantId = TenantTwo;
            var tenantTwoInstrument = CreateInstrument(2);

            Console.WriteLine(Environment.NewLine);

            for (var i = 0; i < 10; i++)
            {
                var num = i + 1;
                if (num % 2 == 0)
                {
                    _currentWriteTenantId = TenantTwo;
                    if (num % 4 == 0)
                    {
                        CreateAccount(num, tenantOneInstrument);
                    }
                    else
                    {
                        CreateAccount(num, tenantTwoInstrument);
                    }
                }
                else
                {
                    _currentWriteTenantId = TenantOne;
                    if (num % 3 == 0)
                    {
                        CreateAccount(num, tenantTwoInstrument);
                    }
                    else
                    {
                        CreateAccount(num, tenantOneInstrument);
                    }
                }
                
                Console.WriteLine(Environment.NewLine);

                _currentWriteTenantId = TenantOne;
                CreateReferenceDataItem("RefItem_" + num);

                Console.WriteLine(Environment.NewLine);
            }
        }

        private static Instrument CreateInstrument(int number)
        {
            using var uow = new NHUnitOfWork();
            uow.OpenSession(_currentReadTenantIds);
            uow.BeginTransaction();

            Console.WriteLine($"Creating Instrument#: {number} for Tenant ID: {_currentWriteTenantId}");

            var instrument = new Instrument
            {
                Number = number
            };

            uow.Session.SaveOrUpdate(instrument);
            uow.Commit();

            return instrument;
        }

        private static void CreateAccount(int number, Instrument instrument)
        {
            using var uow = new NHUnitOfWork();
            uow.OpenSession(_currentReadTenantIds);
            uow.BeginTransaction();

            Console.WriteLine(
                $"Creating Account#: {number} under Instrument#: {instrument.Number} for Tenant ID: {_currentWriteTenantId}");

            var account = new Account
            {
                AccountNumber = number,
                Instrument = instrument
            };

            try
            {
                instrument.AddAccount(account);
                uow.Session.SaveOrUpdate(account);

                uow.Commit();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Unable to create Account: {ex.Message}");
                uow.Rollback();
            }
        }

        private static void CreateReferenceDataItem(string value)
        {
            using var uow = new NHUnitOfWork();
            uow.OpenSession(_currentReadTenantIds);
            uow.BeginTransaction();

            Console.WriteLine($"Creating ReferenceDataItem with Value: {value} using Tenant ID: {_currentWriteTenantId}");

            var referenceDataItem = new ReferenceDataItem
            {
                Value = value
            };

            uow.Session.SaveOrUpdate(referenceDataItem);
            uow.Commit();
        }

        private static List<Instrument> GetInstruments(NHUnitOfWork uow)
        {
            var instruments = uow.Session.Query<Instrument>().ToList();

            return instruments;
        }

        private static List<Account> GetAccounts(NHUnitOfWork uow)
        {
            var accounts = uow.Session.Query<Account>().ToList();

            return accounts;
        }

        private static List<ReferenceDataItem> GetReferenceDataItems(NHUnitOfWork uow)
        {
            var referenceDataItems = uow.Session.Query<ReferenceDataItem>().ToList();

            return referenceDataItems;
        }
    }
}
