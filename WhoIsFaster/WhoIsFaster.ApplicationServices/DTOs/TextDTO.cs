using System;
using System.Collections.Generic;
using System.Text;
using WhoIsFaster.Domain.Entities;

namespace WhoIsFaster.ApplicationServices.DTOs
{
    public class TextDTO
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public string TextContent { get; set; }

        public TextDTO()
        {

        }

        public TextDTO(Text text)
        {
            Id = text.Id;
            Source = text.Source;
            TextContent = text.TextContent;
        }
    }
}
