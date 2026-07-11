using NoteApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoteApp.Core.Services
{
    interface INoteService
    {
        Task<Note> AddNoteAsync(string title, string Body, List<string> tagNames);
        Task<List<Note>> GetAllNotesAsync();
        Task<List<Note>> SearchNotesAsync(string keyword);
        Task<List<Note>> GetNotesByTagAsync(string tagName);
        Task UpdateNoteAsync(int id, string? newTitle, string? newBody, List<string>? newTagName);
        Task DeleteNoteAsync(int id);
    }
}
