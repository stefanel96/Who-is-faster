using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.Domain.Entities.RoomAggregate;
using WhoIsFaster.Domain.Interfaces.Repositories;

namespace WhoIsFaster.Infrastructure.Data.Persistance.Respositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly WhoIsFasterDbContext whoIsFasterDbContext;

        public RoomRepository(WhoIsFasterDbContext whoIsFasterDbContext)
        {
            this.whoIsFasterDbContext = whoIsFasterDbContext;
        }

        public async Task AddRoomAsync(Room room)
        {
            await whoIsFasterDbContext.Rooms.AddAsync(room);
        }

        public async Task DeleteAsync(int id)
        {
            var room = await GetByIdAsync(id);
            if (room != null)
            {
                room.Delete();
            }
        }

        public async Task<List<Room>> GetAllNotStartedPublicRooms()
        {
            //TODO - SetWordList
            return await whoIsFasterDbContext.Rooms
                            .Include(r => r.RoomPlayers)
                            .Include(r => r.Text)
                            .Where(r => r.HasStarted == false && r.RoomType == RoomType.Public).ToListAsync();
        }

        public async Task<Room> GetByIdAsync(int id)
        {
            Room room = await whoIsFasterDbContext.Rooms.Include(r => r.RoomPlayers)
                .Include(r => r.Text)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room != null)
            {
                room.SetWordList();
            }
            return room;
        }

        public async Task<Room> SecureGetByIdAsync(int id)
        {
            Room room = await whoIsFasterDbContext.Rooms.Include(r => r.RoomPlayers)
                .Include(r => r.Text)
                .FirstOrDefaultAsync(r => r.Id == id);
            await whoIsFasterDbContext.Entry(room).ReloadAsync();
            if (room != null)
            {
                room.SetWordList();
            }
            return room;
        }

        public async Task<Room> GetJoinedRoomForUserName(string userName)
        {
            Room room = await whoIsFasterDbContext.Rooms
                           .Include(r => r.RoomPlayers)
                           .Include(r => r.Text)
                           .Where(r => r.RoomPlayers.Any(rp => rp.UserName == userName))
                           .FirstOrDefaultAsync();
            if (room != null)
            {
                room.SetWordList();
            }
            return room;
        }

        public async Task<Room> GetRandomNotStartedJoinablePublicRoom()
        {
            Room room = await whoIsFasterDbContext.Rooms
                            .Include(r => r.RoomPlayers)
                            .Include(r => r.Text)
                            .Where(r =>
                                r.HasStarted == false &&
                                r.RoomType == RoomType.Public &&
                                r.MaxPlayers != r.RoomPlayers.Count
                            )
                            .OrderBy(r => Guid.NewGuid())
                            .Take(1)
                            .FirstOrDefaultAsync();
            if (room != null)
            {
                room.SetWordList();
            }
            
            return room;
        }
    }
}