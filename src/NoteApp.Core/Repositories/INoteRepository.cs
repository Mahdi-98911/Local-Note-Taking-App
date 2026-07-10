using NoteApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoteApp.Core.Repositories
{
    interface INoteRepository
    {
        Task CreateNote(Note note);
        Task<List<Note>> ReadNotes();
        Task Update(int Id, string? Title, string? Body, List<Tag>? Tages);
        Task Delete(int Id);
    }
}
