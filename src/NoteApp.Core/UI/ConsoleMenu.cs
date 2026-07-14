using NoteApp.Core.Models;
using NoteApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;

namespace NoteApp.Core.UI
{
    internal class ConsoleMenu
    {
        private readonly INoteService _noteService;
        public ConsoleMenu(INoteService noteService)
        {
            _noteService = noteService;
        }


        private void DisplayNote(Note note)
        {
            Console.WriteLine($"ID: {note.Id} | {note.Title}");
            Console.WriteLine($"Body: {note.Body}");
            Console.WriteLine($"Tags: {string.Join(", ", note.Tags.Select(t => t.Name))}");
            Console.WriteLine($"Created: {note.CreatedAt:yyyy-MM-dd} | Updated: {note.LastUpdatedAt:yyyy-MM-dd}");
            Console.WriteLine("─────────────────────");
        }
        public async Task Run()
        {


            while (true)
            {
                Console.WriteLine("Enter your choice:");
                Console.WriteLine("1.Add note");
                Console.WriteLine("2.Get all notes");
                Console.WriteLine("3.Search note");
                Console.WriteLine("4.Get note by tag");
                Console.WriteLine("5.Update note");
                Console.WriteLine("6.Delete note");
                Console.WriteLine("7.Exit");

                if (!int.TryParse(Console.ReadLine(), out int choice) || choice > 7 || choice < 1)
                {
                    Console.WriteLine("You should enterr a number between 1-7");
                    continue;
                }
                if(choice == 1)
                {
                    await AddNoteAsync();
                }else if(choice == 2)
                {
                    await GetAllNotesAsync();
                }else if(choice == 3)
                {
                    await SearchNotesAsync();
                }else if(choice == 4)
                {
                    await GetByTagAsync();
                }else if(choice == 5)
                {
                    await UpdateNoteAsync();
                }else if(choice == 6)
                {
                    await DeleteNoteAsync();
                }else if(choice == 7)
                {
                    return;
                }
            }
        }

        private async Task GetAllNotesAsync()
        {
            var notes = await _noteService.GetAllNotesAsync();
            foreach(var note in notes)
            {
                DisplayNote(note);
            }
        }
        private async Task AddNoteAsync()
        {
            try
            {
                Console.WriteLine("Enter title: ");
                var title  = Console.ReadLine() ?? string.Empty;

                Console.WriteLine("Enter body: ");
                var body =  Console.ReadLine() ?? string.Empty;

                Console.WriteLine("Enter tags (comma separated):");
                var tagNames = Console.ReadLine()?
                    .Split(',')
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList() ?? new List<string>();

                await _noteService.AddNoteAsync(title, body, tagNames);
                Console.WriteLine("Note added succefully");
            }catch(ArgumentNullException ex)
            {
                Console.WriteLine($"Error message: {ex.Message}");
            }
        }
        private async Task SearchNotesAsync()
        {
            Console.WriteLine("search: ");
            var keyword = Console.ReadLine();
            var notes = await _noteService.SearchNotesAsync(keyword);
            foreach (var note in notes)
            {
                DisplayNote(note);
                foreach (var tag in note.Tags)
                {
                    Console.WriteLine(tag + " ");
                }
            }
        }
        private async Task GetByTagAsync()
        {
            Console.WriteLine("Enter the tag: ");
            var tag = Console.ReadLine();
            var notes = await _noteService.GetNotesByTagAsync(tag);

            foreach (var note in notes)
            {
                DisplayNote(note);
                foreach (var tag1 in note.Tags)
                {
                    Console.WriteLine(tag1 + " ");
                }
            }
        }
        private async Task UpdateNoteAsync()
        {
            int id;
            while (true)
            {
                Console.WriteLine("Enter the id of the note: ");
                if (int.TryParse(Console.ReadLine(), out id))
                {
                    break;
                }
                Console.WriteLine("Enter a valid number!");
            }
            Console.WriteLine("Enter title: ");
            var Title = Console.ReadLine();

            Console.WriteLine("Enter body: ");
            var Body = Console.ReadLine();

            Console.WriteLine("Enter tags (comma separated):");
            var tagNames = Console.ReadLine()?
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList() ?? new List<string>();

            await _noteService.UpdateNoteAsync(id, Title, Body, tagNames);
        }
        private async Task DeleteNoteAsync()
        {
            int Id;
            while (true)
            {
                Console.WriteLine("Enter id of the note that you want to delete: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine(" enter a valid id!");
                    continue;
                }
                Id = id;
                break;
            }
            await _noteService.DeleteNoteAsync(Id);
        }
    }
}
