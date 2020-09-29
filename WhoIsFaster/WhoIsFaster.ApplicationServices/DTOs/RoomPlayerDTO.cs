using WhoIsFaster.Domain.Entities.RoomAggregate;

namespace WhoIsFaster.ApplicationServices.DTOs
{
    public class RoomPlayerDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int WordsPerMinute { get; set; }
        public int CorrectlyTypedTextIndex { get; set; }
        public int IncorrectilyTypedTextEndIndex { get; set; }
        public int CurrentTextIndex { get; set; }
        public string CurrentWord { get; set; }
        public string CurrentInput { get; set; }
        public bool HasWon { get; set; }
        public bool IsDone { get; set; }
        public bool IsRoomAdmin { get; set; }

        public RoomPlayerDTO()
        {

        }

        public RoomPlayerDTO(RoomPlayer roomPlayer)
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
            IsDone = roomPlayer.IsDone;
            IsRoomAdmin = roomPlayer.IsRoomAdmin;
        }
    }

}