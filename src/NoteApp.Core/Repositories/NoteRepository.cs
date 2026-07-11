using Microsoft.EntityFrameworkCore;
using NoteApp.Core.Data;
using NoteApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
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
        public async Task<Note> AddAsync(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }
        public async Task<List<Note>> GetAllAsync()
        {
            return await _context.Notes.Include(n => n.Tags).ToListAsync();
        }
        public async Task<Note?> GetByIdAsync(int id)
        {
            var note = await _context.Notes
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.Id == id);
            if(note == null)
            {
                return null;
            }
            return note;
        }
        public async Task<List<Note>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<Note>();
            }
            keyword = keyword.Trim().ToLower();

            return await _context.Notes
                .Include(n => n.Tags)
                .Where(n =>
                EF.Functions.Like(n.Title, $"%{keyword}%") ||
                EF.Functions.Like(n.Body, $"%{keyword}%") ||
                n.Tags.Any(t => EF.Functions.Like(t.Name, $"%{keyword}%")))
                .OrderByDescending(n => n.LastUpdatedAt)
                .ToListAsync();

        }

        public async Task<List<Note>> GetByTagAsync(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                return new List<Note>();
            }

            tagName = tagName.Trim().ToLower();

            return await _context.Notes
                .Include(n => n.Tags)
                .Where(n => n.Tags.Any(t => t.Name.ToLower() == tagName))
                .OrderByDescending(n => n.LastUpdatedAt)
                .ToListAsync();
        }
        public async Task UpdateAsync(Note updatedNote)
        {
            _context.Notes.Update(updatedNote);
            await _context.SaveChangesAsync();

        }
        public async Task DeleteAsync(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if(note == null)
            {
                return;
            }
            _context.Notes.Remove(note);

            await _context.SaveChangesAsync();
        }

        public async Task<Tag?> GetTagByNameAsync(string name)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == name);
            if(tag == null)
            {
                return null;
            }
            return tag;
        }
        public async Task<Tag> AddTagAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return tag;
        }
    }
}
