using WhoIsFaster.Infrastructure.Data.Persistance.Respositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.Domain.Interfaces;
using WhoIsFaster.Domain.Interfaces.Repositories;

namespace WhoIsFaster.Infrastructure.Data.Persistance.Common
{
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        private readonly WhoIsFasterDbContext Context;

        public IRegularUserRepository RegularUserRepository { get; }
        public IRoomRepository RoomRepository { get; }
        public ITextRepository TextRepository { get; }

        public EfCoreUnitOfWork(WhoIsFasterDbContext context)
        {
            Context = context;
            RegularUserRepository = new RegularUserRepository(context);
            RoomRepository = new RoomRepository(context);
            TextRepository = new TextRepository(context);

        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException duce)
            {
                throw duce;
            }
            catch (DbUpdateException due)
            {
                throw due;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region IDisposable implementation

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }
            if (disposing)
            {
                Context.Dispose();
            }
            disposedValue = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion IDisposable implementation
    }
}
