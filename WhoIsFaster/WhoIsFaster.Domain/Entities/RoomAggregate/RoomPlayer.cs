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
        public int CorrectlyTypedTextIndex { get; private set; }
        public int IncorrectilyTypedTextEndIndex { get; private set; }
        public int CurrentTextIndex { get; private set; }
        public string CurrentWord { get; private set; }
        public int CorrectlyTypedWordNumber { get; private set; }
        public bool HasWon { get; private set; }
        public bool IsDone { get; private set; }
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
            CorrectlyTypedTextIndex = 0;
            IncorrectilyTypedTextEndIndex = 0;
            CurrentTextIndex = 0;
            CorrectlyTypedWordNumber = 0;
            HasWon = false;
            IsDone = false;
            CurrentInput = "";
        }

        public void UpdateRoomPlayer(RoomPlayer roomPlayer)
        {
            WordsPerMinute = roomPlayer.WordsPerMinute;
            CorrectlyTypedTextIndex = roomPlayer.CorrectlyTypedTextIndex;
            IncorrectilyTypedTextEndIndex = roomPlayer.IncorrectilyTypedTextEndIndex;
            CurrentTextIndex = roomPlayer.CurrentTextIndex;
            CorrectlyTypedWordNumber = roomPlayer.CorrectlyTypedWordNumber;
            HasWon = roomPlayer.HasWon;
            CurrentInput = roomPlayer.CurrentInput;
            IsRoomAdmin = roomPlayer.IsRoomAdmin;
        }

        public void UpdateWordsPerMinute(int wordsPerMinute)
        {
            WordsPerMinute = wordsPerMinute;
        }
        public void UpdateCorrectlyTypedTextIndex(int newCorrectlyTypedTextIndex)
        {
            CorrectlyTypedTextIndex = newCorrectlyTypedTextIndex;
        }
        public void UpdateIncorrectilyTypedTextEndIndex(int newIncorrectilyTypedTextEndIndex)
        {
            IncorrectilyTypedTextEndIndex = newIncorrectilyTypedTextEndIndex;
        }

        public void UpdateCurrentWord(string currentWord)
        {
            CurrentWord = currentWord;
        }

        public void UpdateCurrentInput(string currentInput)
        {
            CurrentInput = currentInput;
        }

        public void PlayerWon()
        {
            HasWon = true;
        }

        public void PlayerDone()
        {
            IsDone = true;
        }

        public void SetRoomAdmin()
        {
            IsRoomAdmin = true;
        }

        public bool CheckInput()
        {
            var correctIndex = GetCorrectlyTypedIndexOfWord(CurrentInput);
            var FinishedWord = false;
            if (correctIndex == CurrentWord.Length && CurrentInput.Length == CurrentWord.Length + 1 && CurrentInput[correctIndex] == ' ')
            {
                CurrentTextIndex = CurrentTextIndex + CurrentWord.Length + 1;
                CorrectlyTypedTextIndex = CurrentTextIndex;
                IncorrectilyTypedTextEndIndex = CurrentTextIndex;
                if (CurrentWord != Room.WordList[Room.WordList.Count - 1])
                {
                    CorrectlyTypedWordNumber += 1;
                }
                CurrentInput = "";
                FinishedWord = true;
            }
            else
            {
                CorrectlyTypedTextIndex = CurrentTextIndex + correctIndex;
                IncorrectilyTypedTextEndIndex = CurrentTextIndex + CurrentInput.Length;
            }
            return FinishedWord;
        }

        private int GetCorrectlyTypedIndexOfWord(string input)
        {
            int n = CurrentWord.Length;
            int m = input.Trim().Length;
            int i;
            for (i = 0; i < Math.Min(n, m); i++)
            {
                if (CurrentWord[i] != input[i])
                    break;
            }

            return i;
        }
    }
}