using WhoIsFaster.ApplicationServices.DTOs;

namespace WhoIsFaster.BlazorApp.ViewModels
{
    public class RoomPlayerVM
    {
        public int Id { get; private set; }
        public string UserName { get; private set; }
        public int WordsPerMinute { get; private set; }
        public int CorrectlyTypedTextIndex { get; private set; }
        public int IncorrectilyTypedTextEndIndex { get; private set; }
        public int CurrentTextIndex { get; private set; }
        public string CurrentWord { get; private set; }
        public string CurrentInput { get; private set; }
        public bool HasWon { get; private set; }
        public bool IsRoomAdmin { get; private set; }


        public RoomPlayerVM(RoomPlayerDTO roomPlayer)
        {
            Id = roomPlayer.Id;
            UserName = roomPlayer.UserName;
            WordsPerMinute = roomPlayer.WordsPerMinute;
            CorrectlyTypedTextIndex = roomPlayer.CorrectlyTypedTextIndex;
            IncorrectilyTypedTextEndIndex = roomPlayer.IncorrectilyTypedTextEndIndex;
            CurrentTextIndex = roomPlayer.CurrentTextIndex;
            CurrentWord = roomPlayer.CurrentWord;
            CurrentInput = roomPlayer.CurrentInput;
            HasWon = roomPlayer.HasWon;
            IsRoomAdmin = roomPlayer.IsRoomAdmin;
        }
    }
}
