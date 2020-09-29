using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.Domain.Entities.RoomAggregate;

namespace WhoIsFaster.Domain.Interfaces.Repositories {
    public interface IRoomRepository {
        Task AddRoomAsync (Room room);
        Task DeleteAsync (int id);
        Task HardDeleteAsync(int id);
        Task<Room> GetByIdAsync (int id);
        Task<Room> SecureGetByIdAsync(int id);
        Task<List<Room>> GetAllNotStartedPublicRooms();
        Task<Room> GetRandomNotStartedJoinablePublicRoom();
        Task<Room> GetJoinedRoomForUserName(string userName);

    }
}