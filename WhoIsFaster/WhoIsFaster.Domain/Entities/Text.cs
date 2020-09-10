using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsFaster.Domain.Entities
{
    public class Text
    {
        public int Id { get; private set; }
        public string Source { get; private set; }
        public string TextContent { get; set; }
        public bool IsDeleted { get; private set; }

        public Text(string source, string textContent)
        {
            Source = source;
            TextContent = textContent;
            IsDeleted = false;
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
