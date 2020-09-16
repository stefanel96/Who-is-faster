using System;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices.DTOs;
using WhoIsFaster.ApplicationServices.Interfaces;
using WhoIsFaster.Domain.Interfaces;

namespace WhoIsFaster.ApplicationServices.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> CreateAndJoinPartyRoomAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task StartRoom(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateAndJoinPracticeRoomAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<RoomDTO> GetRoomByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<RoomDTO> GetRoomByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task JoinOrCreateRoomAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task JoinPartyRoomAsync(string userName, int roomId)
        {
            throw new NotImplementedException();
        }

    }
}
