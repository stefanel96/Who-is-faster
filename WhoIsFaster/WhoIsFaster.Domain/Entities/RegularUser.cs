using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsFaster.Domain.Entities
{
    public class RegularUser
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string UserName { get; private set; }
        public double AverageWordsPerMinute { get; private set; } = 0.0;
        public int Wins { get; private set; } = 0;
        public int Loses { get; private set; } = 0;
        public int CurrentRoomId { get; private set; }
        public bool IsInRoom { get; private set; }

        public RegularUser(string firstName, string lastName, string userName)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
        }

        public void JoinRoom(int currentRoomId)
        {
            CurrentRoomId = currentRoomId;
            IsInRoom = true;

        }

        public void LeaveRoom()
        {
            IsInRoom = false;
        }

        public void UpdatePlayerStats(double WordsPerMinute, bool hasWon) {
            if (hasWon)
            {
                Wins += 1;
            }
            else
            {
                Loses += 1;
            }
            AverageWordsPerMinute += (WordsPerMinute - AverageWordsPerMinute) / (Wins + Loses);
        }

        public void UpdateFirstName(string newFirstName)
        {
            FirstName = newFirstName;
        }

        public void UpdateLastName(string newLastName)
        {
            LastName = newLastName;
        }

    }
}
