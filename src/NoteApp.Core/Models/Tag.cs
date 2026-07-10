using System;
using System.Collections.Generic;
using System.Text;

namespace NoteApp.Core.Models
{
    internal class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Note> Notes { get; set; } = new();
    }
}
