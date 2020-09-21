using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices.DTOs;


namespace WhoIsFaster.ApplicationServices.Interfaces
{
    public interface IRoomService
    {
        Task<int> CreateAndJoinPracticeRoomAsync(string userName);
        Task<int> JoinOrCreateRoomAsync(string userName);
        Task<int> CreateAndJoinPartyRoomAsync(string userName);
        Task<int> JoinPartyRoomAsync(string userName, int roomId);
        Task<RoomDTO> GetRoomByIdAsync(int id);
        Task StartRoom(int id);
        Task<RoomDTO> GetRoomByUserNameAsync(string userName);
    }
}
 