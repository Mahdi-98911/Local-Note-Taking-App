using Microsoft.EntityFrameworkCore;
using NoteApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoteApp.Core.Data
{
    internal class NoteDbContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        
        public NoteDbContext(DbContextOptions<NoteDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasIndex(t => t.Name)
                        .IsUnique();
            });
        }
    }
}