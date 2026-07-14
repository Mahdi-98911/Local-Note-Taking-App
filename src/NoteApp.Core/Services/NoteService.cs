using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoteApp.Core.Data;
using NoteApp.Core.Models;
using NoteApp.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using NoteApp.Core.Exceptions;

namespace NoteApp.Core.Services
{
    internal class NoteService : INoteService
    {
        private readonly INoteRepository _repository;
        private readonly ILogger<NoteService> _logger;
        
        public NoteService(INoteRepository repository , ILogger<NoteService> logger ) { 
            _repository = repository;
            _logger = logger;
            
        }

        public async Task<Note> AddNoteAsync(string title, string body, List<string> tagNames)
        {
            var note = new Note();
            if (string.IsNullOrWhiteSpace(title)) {
                _logger.LogWarning("Tried to create note without title.");
                throw new ArgumentNullException("Note title is required");
            }
            if (string.IsNullOrWhiteSpace(body)) {
                _logger.LogWarning("Invalid empty body recieved");
                throw new ArgumentNullException("You need to enter a body for the note");
            }
            
            foreach(var tag in tagNames)
            {
                var existingTage = await _repository.GetTagByNameAsync(tag);

                Tag tagToUse;

                if(existingTage != null)
                {
                    tagToUse = existingTage;
                }
                else
                {
                    tagToUse = new Tag { Name = tag };
                    await _repository.AddTagAsync(tagToUse);

                }

                note.Tags.Add(tagToUse);
            }


            note.Title = title;
            note.Body = body;

            await _repository.AddAsync(note);
            return note;
        }

        public async Task<List<Note>> GetAllNotesAsync()
        {
            List<Note> notes = await _repository.GetAllAsync();
            if( !notes.Any())
            {
                _logger.LogInformation("there is no note saved in the database");
                return new List<Note>();
            }
            _logger.LogInformation("Retrieved {Count} notes from database.", notes.Count());
            return notes;
        }

        public async Task<List<Note>> SearchNotesAsync(string keyword)
        {
            var notes = await _repository.SearchAsync(keyword);

            if(notes == null || !notes.Any())
            {
                _logger.LogInformation("Couldn't find any note with this keyword");
                return new List<Note>();
            }
            _logger.LogInformation($"Found {notes.Count} notes");
            return notes;
        }

        public async Task<List<Note>> GetNotesByTagAsync(string tagName)
        {
            var notes = await _repository.GetByTagAsync(tagName);

            if (!notes.Any())
            {
                _logger.LogInformation("Couldn't find any note with this tag");
                return new List<Note>();
            }
            _logger.LogInformation($"Found {notes.Count} notes");
            return notes;
        }

        public async Task UpdateNoteAsync(int id, string? newTitle, string? newBody, List<string>? newTagName)
        {
            var note = await _repository.GetByIdAsync(id);

            if(note == null)
            {
                _logger.LogWarning("Note with ID {NoteId} not found .", id);
                throw new NotFoundException($"Note with id {id} was not found.");
            }

            if (!string.IsNullOrWhiteSpace(newTitle))
                note.Title = newTitle;

            if (!string.IsNullOrWhiteSpace(newBody))
                note.Body = newBody;

            if(newTagName != null)
            {
                note.Tags.Clear();
                foreach (var tag in newTagName)
                {
                    var existingTag = await _repository.GetTagByNameAsync(tag);

                    Tag tagToUse;
                    if (existingTag != null)
                    {
                        tagToUse = existingTag;
                    }
                    else
                    {
                        tagToUse = new Tag { Name = tag };
                        await _repository.AddTagAsync(tagToUse);
                    }
                    note.Tags.Add(tagToUse);
                }
            }



            note.LastUpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(note);
        }
        
        public async Task DeleteNoteAsync(int id)
        {
            var note = await _repository.GetByIdAsync(id);

            if( note == null)
            {
                _logger.LogWarning("Note with ID {Id} not found .", id);
                throw new NotFoundException($"Note with id {id} was not found.");
            }

            await _repository.DeleteAsync(id);
        }


    }
}
