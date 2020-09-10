using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsFaster.Domain.Entities.RoomAggregate
{
    public class RoomPlayer
    {
        public int Id { get; private set; }
        public int RegularUserId { get; private set; }
        public RegularUser RegularUser { get; private set; }
        public string UserName { get; private set; }
        public int RoomId { get; private set; }
        public Room Room { get; private set; }
        public int WordsPerMinute { get; private set; }
        public string CurrentWord { get; private set; }
        public int CorrectlyTypedWordNumber { get; private set; }
        public bool HasWon { get; private set; }
        public string CurrentInput { get; private set; }
        public bool IsRoomAdmin { get; private set; }

        public RoomPlayer()
        {

        }


        public RoomPlayer(Room room, RegularUser regularUser, string currentWord)
        {
            Room = room;
            UserName = regularUser.UserName;
            RegularUser = regularUser;
            CurrentWord = currentWord;
            WordsPerMinute = 0;
            CorrectlyTypedWordNumber = 0;
            HasWon = false;
            CurrentInput = "";
        }

    }
}
