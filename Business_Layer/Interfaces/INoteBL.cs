using Common_Layer;
using Repository_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Interfaces
{
    public interface INoteBL
    {
        Task AddNote(int UserID, NotePostModel note);
        Task DeleteNote(int UserID, int NoteID);
        Task<List<Note>> GetAll(int UserID);
        Task ChangeColour(int UserID, int NoteID, string Colour);
        Task ArchiveNote(int UserID, int NoteID);
        Task PinNote(int UserID, int NoteID);
        Task Reminder(int UserID, int NoteID, DateTime ReminderDate);
        Task TrashNote(int UserID, int NoteID);
        Task<Note> UpdateNote(int UserID, int NoteID, NoteUpdateModel updateNote);
    }
}
