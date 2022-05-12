using Common_Layer;
using Repository_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Layer.Interfaces
{
    public interface INoteRL
    {
        Task AddNote(int UserID, NotePostModel note);
        Task DeleteNote(int UserID, int NoteID);
        Task<List<Note>> GetAll(int UserID);
        Task ChangeColour(int UserID, int NoteID, string Colour);
        Task ArchiveNote(int UserID, int NoteID);
        Task PinNote(int UserID, int NoteID);
        Task Reminder(int UserID, int NoteID, DateTime ReminderDate);
        Task TrashNote(int UserID, int NoteID);

    }
}
