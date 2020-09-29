using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsFaster.ApplicationServices.DTOs
{
    public class RoomResponseDTO
    {
        public RoomDTO Room { get; set; }
        public bool IsNew { get; set; }

        public RoomResponseDTO(RoomDTO room, bool isNew)
        {
            Room = room;
            IsNew = isNew;
        }
    }
}
