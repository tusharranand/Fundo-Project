﻿using Business_Layer.Interfaces;
using Common_Layer;
using Repository_Layer.Entities;
using Repository_Layer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Services
{
    public class NoteBL : INoteBL
    {
        INoteRL noteRL;
        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;
        }
        public async Task AddNote(int UserID, NotePostModel note)
        {
            try
            {
                await this.noteRL.AddNote(UserID, note);
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
                await this.noteRL.DeleteNote(UserID, NoteID);
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
                return await this.noteRL.GetAll(UserID);
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
                await this.noteRL.ChangeColour(UserID, NoteID, Colour);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ArchiveNote(int UserID, int NoteID)
        {
            try
            {
                await this.noteRL.ArchiveNote(UserID, NoteID);
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
                await this.noteRL.PinNote(UserID, NoteID);
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
                await this.noteRL.Reminder(UserID, NoteID, ReminderDate);
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
                await this.noteRL.TrashNote(UserID, NoteID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<Note> UpdateNote(int UserID, int NoteID, NoteUpdateModel updateNote)
        {
            try
            {
                return this.noteRL.UpdateNote(UserID, NoteID, updateNote);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<Note> GetNote(int UserID, int NoteID)
        {
            try
            {
                return this.noteRL.GetNote(UserID, NoteID); 
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
