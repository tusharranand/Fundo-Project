using Repository_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Layer.Interfaces
{
    public interface ILabelRL
    {
        Task AddLabel(int UserID, int NoteID, string LabelName);
        Task<List<Label>> GetAllLabels(int UserID);
        Task<List<Label>> GetAllLabelsForANote(int UserID, int NoteID);

    }
}
