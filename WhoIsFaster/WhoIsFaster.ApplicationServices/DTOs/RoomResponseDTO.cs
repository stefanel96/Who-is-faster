using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsFaster.ApplicationServices.DTOs
{
    public class RoomResponseDTO
    {
        public int RoomId { get; set; }
        public bool IsNew { get; set; }

        public RoomResponseDTO(int roomId, bool isNew)
        {
            RoomId = roomId;
            IsNew = isNew;
        }
    }
}
