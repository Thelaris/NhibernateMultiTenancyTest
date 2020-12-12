using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using NHibernate;

namespace NhibernateMultitenancyTest.Config
{
    public class NHUnitOfWork : IUnitOfWork
    {
        public ISession Session => _session;

        private ISession _session;
        private ITransaction _transaction;

        public NHUnitOfWork()
        {

        }

        public void OpenSession(Guid[] readTenantIds)
        {
            if (_session == null || !_session.IsConnected)
            {
                if (_session != null)
                {
                    _session.Dispose();
                }

                var interceptor = new NHibernateMultiTenancyInterceptor(Program._currentWriteTenantId);
                _session = NhibernateConfig.SessionFactory.WithOptions().Interceptor(interceptor).OpenSession();
                _session.EnableFilter("MultiTenantFilter").SetParameterList("TenantIds", readTenantIds);
            }
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (_transaction == null || _transaction.IsActive)
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                }

                _transaction = _session.BeginTransaction(isolationLevel);
            }
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_session != null)
            {
                _session.Dispose();
                _session = null;
            }
        }
    }
}
