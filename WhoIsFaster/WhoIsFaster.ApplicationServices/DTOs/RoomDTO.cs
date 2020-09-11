using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhoIsFaster.Domain.Entities;
using WhoIsFaster.Domain.Entities.RoomAggregate;

namespace WhoIsFaster.ApplicationServices.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public IEnumerable<RoomPlayerDTO> RoomPlayers { get; set; }
        public bool HasStarted { get; set; }
        public bool IsStarting { get; set; }
        public int MaxPlayers { get;  set; }
        public int PlayersToStart { get; set; }
        public double GameLengthSeconds { get; set; }
        public double LengthOfStarting { get; set; }
        public DateTime TimeStarted { get; set; }
        public DateTime TimeLastPlayerJoined { get; set; }
        public DateTime StartEventTime { get; set; }
        public string RoomType { get; set; }
        public List<String> WordList { get; set; }
        public TextDTO Text { get; set; }

        public RoomDTO()
        {

        }

        public RoomDTO(Room room)
        {
            Id = room.Id;
            RoomPlayers = room.RoomPlayers.Select(rp => new RoomPlayerDTO(rp));
            IsStarting = room.IsStarting;
            HasStarted = room.HasStarted;
            MaxPlayers = room.MaxPlayers;
            PlayersToStart = room.PlayersToStart;
            GameLengthSeconds = room.GameLengthSeconds;
            LengthOfStarting = room.LengthOfStarting;
            TimeStarted = room.TimeStarted;
            TimeLastPlayerJoined = room.LastPlayerJoined;
            StartEventTime = room.StartEventTime;
            RoomType = room.RoomType.ToString();
            WordList = room.WordList;
            Text = new TextDTO(room.Text);
        }

    }
}