using System;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices.DTOs;
using WhoIsFaster.ApplicationServices.Interfaces;
using WhoIsFaster.Domain.Entities;
using WhoIsFaster.Domain.Entities.RoomAggregate;
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
            RegularUser regularUser = await _unitOfWork.RegularUserRepository.GetByUserNameAsync(userName);
            if (regularUser != null && !regularUser.IsInRoom)
            {
                Text text = await _unitOfWork.TextRepository.GetRandomTextAsync();
                var room = new Room(4, 2, text, 1200, 5, RoomType.Party);

                room.PlayerJoin(regularUser);

                await _unitOfWork.RoomRepository.AddRoomAsync(room);

                regularUser.JoinRoom(room.Id);

                await _unitOfWork.SaveChangesAsync();

                return room.Id;
            }

            return -1;
        }

        public async Task StartRoom(int id)
        {
            Room room = await _unitOfWork.RoomRepository.GetByIdAsync(id);
            room.SetIsStarting();
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> CreateAndJoinPracticeRoomAsync(string userName)
        {
            RegularUser regularUser = await _unitOfWork.RegularUserRepository.GetByUserNameAsync(userName);
            if (regularUser != null && !regularUser.IsInRoom)
            {
                Text text = await _unitOfWork.TextRepository.GetRandomTextAsync();
                var room = new Room(1, 1, text, 1200, 5, RoomType.Practice);

                room.PlayerJoin(regularUser);

                await _unitOfWork.RoomRepository.AddRoomAsync(room);

                regularUser.JoinRoom(room.Id);

                await _unitOfWork.SaveChangesAsync();

                return room.Id;
            }

            return -1;
        }

        public async Task<RoomDTO> GetRoomByIdAsync(int id)
        {
            Room room = await _unitOfWork.RoomRepository.GetByIdAsync(id);
            return room == null ? null : new RoomDTO(room);
        }

        public async Task<RoomDTO> GetRoomByUserNameAsync(string userName)
        {
            Room room = await _unitOfWork.RoomRepository.GetJoinedRoomForUserName(userName);
            return room == null ? null : new RoomDTO(room);
        }

        public async Task<int> JoinOrCreateRoomAsync(string userName)
        {
            RegularUser regularUser = await _unitOfWork.RegularUserRepository.GetByUserNameAsync(userName);
            if (regularUser != null && !regularUser.IsInRoom)
            {
                Room room = await _unitOfWork.RoomRepository.GetRandomNotStartedJoinablePublicRoom();
                if (room == null)
                {
                    Text text = await _unitOfWork.TextRepository.GetRandomTextAsync();
                    room = new Room(4, 2, text, 1200, 5, RoomType.Practice);
                    await _unitOfWork.RoomRepository.AddRoomAsync(room);
                }

                room.PlayerJoin(regularUser);
                regularUser.JoinRoom(room.Id);

                await _unitOfWork.SaveChangesAsync();

                return room.Id;
            }

            return -1;
        }

        public async Task<int> JoinPartyRoomAsync(string userName, int roomId)
        {
            Room room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
            RegularUser regularUser = await _unitOfWork.RegularUserRepository.GetByUserNameAsync(userName);

            if (room != null && regularUser != null)
            {
                room.PlayerJoin(regularUser);
                regularUser.JoinRoom(room.Id);
                await _unitOfWork.SaveChangesAsync();
                return room.Id;
            }

            return -1;
        }

    }
}
