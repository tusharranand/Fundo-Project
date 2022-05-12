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
                List<Note> list = new List<Note>();
                list = await fundoo.Note.Include(u => u.user).ToListAsync();
                return list;
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
    }
}

