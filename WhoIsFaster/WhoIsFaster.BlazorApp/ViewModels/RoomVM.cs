using System;
using System.Collections.Generic;
using System.Linq;
using WhoIsFaster.ApplicationServices.DTOs;

namespace WhoIsFaster.BlazorApp.ViewModels
{
    public class RoomVM
    {
        public int Id { get; private set; }
        public IEnumerable<RoomPlayerVM> RoomPlayers { get; private set; }
        public bool HasStarted { get; private set; }
        public bool HasFinished { get; private set; }
        public bool IsStarting { get; private set; }
        public int MaxPlayers { get; private set; }
        public int PlayersToStart { get; private set; }
        public double GameLengthSeconds { get; private set; }
        public double LengthOfStarting { get; private set; }
        public DateTime TimeStarted { get; private set; }
        public DateTime TimeLastPlayerJoined { get; private set; }
        public DateTime StartEventTime { get; private set; }
        public string RoomType { get; private set; }
        public List<String> WordList { get; private set; }
        public TextVM Text { get; private set; }

        public RoomVM(RoomDTO room)
        {
            Id = room.Id;
            RoomPlayers = room.RoomPlayers.Select(rp => new RoomPlayerVM(rp));
            IsStarting = room.IsStarting;
            HasStarted = room.HasStarted;
            HasFinished = room.HasFinished;
            MaxPlayers = room.MaxPlayers;
            PlayersToStart = room.PlayersToStart;
            GameLengthSeconds = room.GameLengthSeconds;
            LengthOfStarting = room.LengthOfStarting;
            TimeStarted = room.TimeStarted;
            TimeLastPlayerJoined = room.TimeLastPlayerJoined;
            StartEventTime = room.StartEventTime;
            RoomType = room.RoomType.ToString();
            WordList = room.WordList;
            Text = new TextVM(room.Text);
        }
    }
}
