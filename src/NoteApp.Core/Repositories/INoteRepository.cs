using NoteApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoteApp.Core.Repositories
{
    interface INoteRepository
    {
        Task<List<Note>> GetAllAsync();
        Task<Note?> GetByIdAsync(int id);
        Task<List<Note>> SearchAsync(string keyword);
        Task<List<Note>> GetByTagAsync(string tagName);
        Task<Note> AddAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(int id);
    }
}
