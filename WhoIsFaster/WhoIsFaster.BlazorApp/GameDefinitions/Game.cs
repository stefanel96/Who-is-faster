using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices.DTOs;
using WhoIsFaster.Domain.Entities;
using WhoIsFaster.Domain.Entities.RoomAggregate;
using WhoIsFaster.Domain.Interfaces;
using WhoIsFaster.Infrastructure.SignalRNotifications.NotificationManagerInterfaces;

namespace WhoIsFaster.BlazorApp.GameDefinitions
{
    public class Game
    {

        private static readonly Game instance = new Game();
        private IUnitOfWork _unitOfWork;
        private IGameNotificationManager _notificationManager;

        static Game() {}

        private Game() {}

        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void SetNotificationManager(IGameNotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        public static Game Instance
        {
            get
            {
                return instance;
            }
        }

        private ConcurrentDictionary<string, string> Players { get; set; } = new ConcurrentDictionary<string, string>();
        private ConcurrentBag<Room> StartedRooms { get; set; } = new ConcurrentBag<Room>();
        private ConcurrentBag<Room> NonStartedRoom { get; set; } = new ConcurrentBag<Room>();

        public async Task Update()
        {
            List<Task<Room>> tasksList = new List<Task<Room>>();

            while(!NonStartedRoom.IsEmpty)
            {
                Room tempRoom;
                NonStartedRoom.TryTake(out tempRoom);
                tasksList.Add(UpdateNonStartedRoom(tempRoom));
            }

            var nonStartedRooms = await Task.WhenAll(tasksList);

            foreach(Room room in nonStartedRooms)
            {
                if(room != null)
                {
                    NonStartedRoom.Add(room);
                }
            }

            tasksList.Clear();

            while(!StartedRooms.IsEmpty)
            {
                Room tempRoom;
                StartedRooms.TryTake(out tempRoom);
                tasksList.Add(UpdateStartedRoom(tempRoom));
            }

            var startedRooms = await Task.WhenAll(tasksList);

            foreach(Room room in startedRooms)
            {
                if (room != null)
                {
                    StartedRooms.Add(room);
                }
            }
        }

        public async Task EndRoom(Room room)
        {
            Room savedRoom = await _unitOfWork.RoomRepository.SecureGetByIdAsync(room.Id);
            savedRoom.UpdateRoom(room);

            RegularUser regularUser;
            foreach (RoomPlayer roomPlayer in savedRoom.RoomPlayers)
            {
                regularUser = await _unitOfWork.RegularUserRepository.GetByIdAsync(roomPlayer.RegularUserId);
                regularUser.UpdatePlayerStats(roomPlayer.WordsPerMinute, roomPlayer.HasWon);
                regularUser.LeaveRoom();
                string value;
                Players.TryRemove(roomPlayer.UserName, out value);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Room> UpdateStartedRoom(Room room)
        {
            foreach (RoomPlayer player in room.RoomPlayers)
            {
                room.UpdateRoomPlayerInput(player.UserName, Players[player.UserName]);
            }
            room.UpdateRoomPlayers();

            await _notificationManager.SendRoomInfoToGroup(room.Id.ToString(), JsonSerializer.Serialize(new RoomDTO(room)));

            if (room.CheckIfOver())
            {
                await EndRoom(room);
                return null;
            }

            return room;
        }

        public async Task<Room> UpdateNonStartedRoom(Room room)
        {
            Room savedRoom = await _unitOfWork.RoomRepository.SecureGetByIdAsync(room.Id);
            if (savedRoom.ShouldStart())
            {
                savedRoom.Start();
                this.StartRoom(savedRoom);
                await _unitOfWork.SaveChangesAsync();
                return null;
            }

            return savedRoom;
        }

        public void StartRoom(Room room)
        {
                StartedRooms.Add(room);
                foreach (RoomPlayer player in room.RoomPlayers)
                {
                    Players.TryAdd(player.UserName, "");
                }
        }

        public async Task AddRoom(int roomId)
        {
            Room room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
            NonStartedRoom.Add(room);
        }

        public void UpdateRoomPlayerInput(int roomId, string userName, string input)
        {
            try
            {
                Players.TryUpdate(userName, input, Players[userName]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

    }
}