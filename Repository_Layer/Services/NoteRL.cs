using Common_Layer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository_Layer.Entities;
using Repository_Layer.FundooContext;
using Repository_Layer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Layer.Services
{
    public class NoteRL : INoteRL
    {
        FundooDbContext fundoo;
        public IConfiguration Configuration { get; }
        public NoteRL(FundooDbContext fundoo, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.fundoo = fundoo;
        }
        public async Task AddNote(int UserID, NotePostModel note)
        {
            try
            {
                Note noteData = new Note();
                noteData.UserID = UserID;
                noteData.Title = note.Title;
                noteData.Description = note.Description;
                noteData.Colour = note.Colour;
                noteData.IsPin = false;
                noteData.IsArchive = false;
                noteData.IsReminder = false;
                noteData.RegisterDate = DateTime.Now;
                noteData.ModifyDate = DateTime.Now;
                fundoo.Add(noteData);
                await fundoo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteNote(int UserID, int NoteID)
        {
            try
            {
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if (note != null)
                {
                    fundoo.Remove(note);
                    await fundoo.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Note>> GetAll(int UserID)
        {
            try
            {
                return await fundoo.Note.Where(x => x.UserID == UserID).Include(u => u.user).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Note> GetNote(int UserID, int NoteID)
        {
            try
            {
                return await fundoo.Note.FirstOrDefaultAsync(x => x.UserID == UserID && x.NoteID == NoteID);
            }
            catch (Exception)
            {
                throw;
            }        
        }

        public async Task ChangeColour(int UserID, int NoteID, string Colour)
        {
            try
            {
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if(note != null)
                {
                    note.Colour = Colour;
                    await fundoo.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }        
        }
        public bool Toggle(bool input)
        {
            if (input)
                input = false;
            else input = true;
            return input;
        }

        public async Task ArchiveNote(int UserID, int NoteID)
        {
            try
            {
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if (note != null)
                {
                    note.IsArchive = Toggle(note.IsArchive);
                    await fundoo.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task PinNote(int UserID, int NoteID)
        {
            try
            {
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if (note != null)
                {
                    note.IsPin = Toggle(note.IsPin);
                    await fundoo.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task Reminder(int UserID, int NoteID, DateTime ReminderDate)
        {
            try
            {
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if (note != null)
                {
                    note.IsReminder = Toggle(note.IsReminder);
                    if (note.IsReminder == true)
                        note.ReminderDate = ReminderDate;
                    await fundoo.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task TrashNote(int UserID, int NoteID)
        {
            try
            {
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if (note != null)
                {
                    note.IsTrash= Toggle(note.IsTrash);
                    await fundoo.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Note> UpdateNote(int UserID, int NoteID, NoteUpdateModel updateNote)
        {
            try
            {
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if (note != null)
                {
                    if(updateNote.Title != "")
                        note.Title = updateNote.Title;
                    if (updateNote.Description != "")
                        note.Description = updateNote.Description;
                    if (updateNote.Colour != "")
                        note.Colour = updateNote.Colour;
                    note.IsArchive = updateNote.IsArchive;
                    note.IsPin = updateNote.IsPin;
                    note.IsReminder = updateNote.IsReminder;
                    note.IsTrash = updateNote.IsTrash;
                    note.ModifyDate = DateTime.Now;
                    await fundoo.SaveChangesAsync();
                }
                return await fundoo.Note.Where(x => x.UserID == UserID && x.NoteID == NoteID).Include(u => u.user).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

