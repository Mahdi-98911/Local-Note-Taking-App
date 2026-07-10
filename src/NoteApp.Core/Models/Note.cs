using System;
using System.Collections.Generic;
using System.Text;

namespace NoteApp.Core.Models
{
    internal class Note
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set;} = DateTime.UtcNow;
        public List<Tag> Tags { get; set; } = new();
    }
}
