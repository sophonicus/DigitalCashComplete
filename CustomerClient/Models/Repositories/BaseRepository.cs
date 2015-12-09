using CustomerClient.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerClient.Models.Repositories
{
    public class BaseRepository : IDisposable, IBaseRepository
    {

        protected const int SITE_ID = 16;

        private bool _UsingTransaction;

        public bool UsingTransaction
        {
            get { return this._UsingTransaction; }
            set { this._UsingTransaction = value; }
        }


        public BankDatabaseEntities db { get; set; }

        public BaseRepository()
        {
            if (this.db == null)
            {
                this.db = new BankDatabaseEntities();
            }

            this._UsingTransaction = false;
        }

        public int SaveChanges()
        {
            if (!this._UsingTransaction)
            {
                return this.db.SaveChanges();
            }
            return 0;
        }



        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public interface IBaseRepository
    {
        BankDatabaseEntities db { get; set; }
    }
}