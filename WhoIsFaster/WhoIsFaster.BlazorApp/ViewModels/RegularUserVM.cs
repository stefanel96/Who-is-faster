using WhoIsFaster.ApplicationServices.DTOs;

namespace WhoIsFaster.BlazorApp.ViewModels
{
    public class RegularUserVM
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string UserName { get; private set; }
        public double AverageWordsPerMinute { get; private set; }
        public int Wins { get; private set; }
        public int Loses { get; private set; }
        public int CurrentRoomId { get; private set; }
        public bool IsInRoom { get; private set; }


        public RegularUserVM(RegularUserDTO regularUser)
        {
            Id = regularUser.Id;
            FirstName = regularUser.FirstName;
            LastName = regularUser.LastName;
            UserName = regularUser.UserName;
            AverageWordsPerMinute = regularUser.AverageWordsPerMinute;
            Wins = regularUser.Wins;
            Loses = regularUser.Loses;
            CurrentRoomId = regularUser.CurrentRoomId;
            IsInRoom = regularUser.IsInRoom;
        }
    }
}
