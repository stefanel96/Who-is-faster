using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.Domain.Interfaces.Repositories;

namespace WhoIsFaster.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRegularUserRepository RegularUserRepository { get; }
        IRoomRepository RoomRepository { get; }
        ITextRepository TextRepository { get; }

        Task SaveChangesAsync();
    }
}
