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
        public bool IsRemoved { get; private set; }
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
            WordList = text.TextContent?.Split(" ").ToList();
            Text = text;
            GameLengthSeconds = gameLengthSeconds;
            LengthOfStarting = lengthOfStarting;
            RoomType = roomType;
            HasStarted = false;
            HasFinished = false;
            IsDeleted = false;
            IsRemoved = false;
        }

        public void UpdateRoom(Room room)
        {
            HasStarted = room.HasStarted;
            HasFinished = room.HasFinished;
            TimeStarted = room.TimeStarted;
            LastPlayerJoined = room.LastPlayerJoined;
            foreach (RoomPlayer roomPlayer in RoomPlayers)
            {
                roomPlayer.UpdateRoomPlayer(room.RoomPlayers.FirstOrDefault(rp => rp.Id == roomPlayer.Id));
            }
        }

        public bool PlayerJoin(RegularUser regularUser)
        {
            var joined = false;
            if (!HasStarted || !IsFull())
            {
                joined = true;
                if (roomPlayers.Count == 0)
                    roomPlayers.Add(new RoomPlayer(this, regularUser, WordList[0], true));
                else
                    roomPlayers.Add(new RoomPlayer(this, regularUser, WordList[0], false));
                LastPlayerJoined = DateTime.Now;
                if (roomPlayers.Count >= 2 && RoomType == RoomType.Public)
                {
                    this.SetIsStarting();
                }
            }

            return joined;
        }

        public void Start()
        {
            HasStarted = true;
            TimeStarted = DateTime.Now;
        }


        public void SetIsStarting()
        {
            IsStarting = true;
            StartEventTime = DateTime.Now;
        }

        public void LeaveRoom()
        {
            IsRemoved = true;
        }


        public void ChangeText(Text newText)
        {
            if (!HasStarted)
            {
                WordList = newText.TextContent.Split(" ").ToList();
                Text = newText;
            }
        }

        public bool IsFull()
        {
            return roomPlayers.Count == MaxPlayers;
        }


        public void UpdateRoomPlayers()
        {
            if (!HasFinished)
            {

                foreach (var player in roomPlayers)
                {
                    UpdateRoomPlayer(player);
                }
            }
        }

        public void UpdateRoomPlayerInput(string userName, string input)
        {
            if (!HasFinished)
            {
                var player = roomPlayers.FirstOrDefault(rp => rp.UserName == userName);
                player.UpdateCurrentInput(input);
            }
        }

        public void UpdateRoomPlayer(RoomPlayer player)
        {
            if (!HasFinished && !player.IsDone)
            {
                var FinishedWord = player.CheckInput();
                if (FinishedWord)
                {
                    if (player.CurrentWord != WordList[WordList.Count - 1])
                    {
                        player.UpdateCurrentWord(WordList[player.CorrectlyTypedWordNumber]);
                    }
                    else
                    {
                        player.PlayerDone();
                    }
                    player.UpdateWordsPerMinute(Convert.ToInt32(player.CorrectlyTypedWordNumber / (DateTime.Now - TimeStarted).TotalMinutes));
                }
            }
        }

        public bool CheckIfOver()
        {
            var isOver = false;
            if ((DateTime.Now - TimeStarted).TotalSeconds >= GameLengthSeconds)
            {
                isOver = true;
                HasFinished = true;
                int maxWords = -1;
                RoomPlayer maxWordPlayer = null;
                foreach (var player in roomPlayers)
                {
                    if (player.CorrectlyTypedWordNumber > maxWords)
                    {
                        maxWords = player.CorrectlyTypedWordNumber;
                        maxWordPlayer = player;
                    }
                }
                maxWordPlayer.PlayerWon();
            }
            else
            {
                foreach (var player in roomPlayers)
                {
                    if (player.IsDone && roomPlayers.Where(p => p.HasWon == true).Count() == 0)
                    {
                        player.PlayerWon();
                    }
                }
                if (roomPlayers.All(p => p.IsDone))
                {
                    isOver = true;
                    HasFinished = true;
                }
            }
            return isOver;
        }

        public bool ShouldStart()
        {
            if (RoomType == RoomType.Public && roomPlayers.Count >= PlayersToStart && (DateTime.Now - LastPlayerJoined).TotalSeconds >= LengthOfStarting)
            {
                return true;
            }
            else if (RoomType != RoomType.Public && IsStarting && (DateTime.Now - StartEventTime).TotalSeconds >= LengthOfStarting)
            {
                return true;
            }

            return false;
        }

        public void SetWordList()
        {
            WordList = Text.TextContent.Split(" ").ToList();
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
