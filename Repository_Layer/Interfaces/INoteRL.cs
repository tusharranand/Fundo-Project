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

    }
}
