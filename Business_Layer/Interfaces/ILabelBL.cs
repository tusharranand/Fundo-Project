﻿using Repository_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Interfaces
{
    public interface ILabelBL
    {
        Task AddLabel(int UserID, int NoteID, string LabelName);
        Task<List<Label>> GetAllLabels(int UserID);
        Task<List<Label>> GetAllLabelsForANote(int UserID, int NoteID);
        Task DeleteLabel(int UserID, int LabelID);
        Task UpdateLabel(int UserID, int LabelID, string LabelName, int NoteID);
    }
}
