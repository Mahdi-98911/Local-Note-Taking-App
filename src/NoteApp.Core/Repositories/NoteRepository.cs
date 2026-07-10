using Microsoft.EntityFrameworkCore;
using NoteApp.Core.Data;
using NoteApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoteApp.Core.Repositories
{
    internal class NoteRepository : INoteRepository
    {
        private readonly NoteDbContext _context;
        public NoteRepository(NoteDbContext context)
        {
            _context = context;
        }
        public async Task CreateNote(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Note>> ReadNotes()
        {
            return await _context.Notes.ToListAsync();
        }
        public async Task Update(int id, string? title, string? body, List<Tag>? tags)
        {
            var note = await _context.Notes.FindAsync(id);

            if(note == null)
            {
                return;
            }

            if(title != null)
            {
                note.Title = title;
            }
            if(body != null)
            {
                note.Body = body;
            }
            if(tags != null)
            {
                note.Tags = tags;
            }
            await _context.SaveChangesAsync();

        }
        public async Task Delete(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if(note == null)
            {
                return;
            }
            _context.Notes.Remove(note);

            await _context.SaveChangesAsync();
        }
    }
}
