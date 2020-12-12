using System;
using System.Collections.Generic;
using System.Text;

namespace NhibernateMultitenancyTest.Config
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
