using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoIsFaster.Domain.Entities.RoomAggregate
{
    public class Room
    {
        public int Id { get; private set; }

        private readonly List<RoomPlayer> roomPlayers = new List<RoomPlayer>();
        public IReadOnlyList<RoomPlayer> RoomPlayers => roomPlayers.AsReadOnly();
        public bool HasStarted { get; private set; }
        public bool IsStarting { get; private set; }
        public bool HasFinished { get; private set; }

        public int MaxPlayers { get; private set; }
        public int PlayersToStart { get; private set; }

        public double GameLengthSeconds { get; private set; }
        public double LengthOfStarting { get; private set; }

        public DateTime TimeStarted { get; private set; }
        public DateTime LastPlayerJoined { get; private set; }
        public DateTime StartEventTime { get; private set; }

        public RoomType RoomType { get; private set; }
        public List<String> WordList { get; private set; }
        public int TextId { get; private set; }
        public Text Text { get; private set; }
        public bool IsDeleted { get; private set; }

        public Room()
        { 
        }

        public Room(int maxPlayers, int playersToStart, Text text, double gameLengthSeconds, double lengthOfStarting, RoomType roomType)
        {
            MaxPlayers = maxPlayers;
            PlayersToStart = playersToStart;
            WordList = text.TextContent.Split(" ").ToList();
            Text = text;
            GameLengthSeconds = gameLengthSeconds;
            LengthOfStarting = lengthOfStarting;
            HasStarted = false;
            HasFinished = false;
            IsDeleted = false;
        }


        public void Delete()
        {
            IsDeleted = true;
        }

        public void Recover()
        {
            IsDeleted = false;
        }
    }
}
