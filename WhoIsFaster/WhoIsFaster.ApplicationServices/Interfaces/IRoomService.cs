using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices.DTOs;


namespace WhoIsFaster.ApplicationServices.Interfaces
{
    public interface IRoomService
    {
        Task<RoomResponseDTO> CreateAndJoinPracticeRoomAsync(string userName);
        Task<RoomResponseDTO> JoinOrCreateRoomAsync(string userName);
        Task<RoomResponseDTO> CreateAndJoinPartyRoomAsync(string userName);
        Task<RoomResponseDTO> JoinPartyRoomAsync(string userName, int roomId);
        Task<RoomDTO> GetRoomByIdAsync(int id);
        Task StartRoom(int id);
        Task LeavePartyRoom(int id);
        Task<RoomDTO> GetRoomByUserNameAsync(string userName);
    }
}
