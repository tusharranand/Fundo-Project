using Business_Layer.Interfaces;
using Repository_Layer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Services
{
    public class LabelBL : ILabelBL
    {
        ILabelRL labelRL;
        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }
        public async Task AddLabel(int UserID, int NoteID, string LabelName)
        {
            try
            {
                await this.labelRL.AddLabel(UserID, NoteID, LabelName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
