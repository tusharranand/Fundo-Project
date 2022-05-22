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
    public class LabelRL : ILabelRL
    {
        FundooDbContext fundoo;
        public IConfiguration Configuration { get; }
        public LabelRL(FundooDbContext fundoo, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.fundoo = fundoo;
        }
        public async Task AddLabel(int UserID, int NoteID, string LabelName)
        {
            try
            {
                var user = fundoo.User.FirstOrDefaultAsync(u => u.UserID == UserID);
                if (user != null)
                {
                    Label labelData = new Label();
                    labelData.LabelName = LabelName;
                    labelData.UserID = UserID; 
                    labelData.NoteID = NoteID;
                    fundoo.Add(labelData);
                    await fundoo.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Label>> GetAllLabels(int UserID)
        {
            try
            {
                return await fundoo.Label.Where(u => u.UserID == UserID).Include(n => n.note).Include(u => u.user).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Label>> GetAllLabelsForANote(int UserID, int NoteID)
        {
            try
            {
                var label = fundoo.Label.FirstOrDefaultAsync(u => u.UserID == UserID && u.NoteID == NoteID);
                if (label != null)
                {
                    List<Label> labels = await fundoo.Label.Where(u => u.UserID == UserID && u.NoteID == NoteID).Include(n => n.note).Include(u => u.user).ToListAsync();
                    return labels;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteLabel(int UserID, int LabelID)
        {
            try
            {
                var label = fundoo.Label.FirstOrDefault(u => u.UserID == UserID && u.LabelID == LabelID);
                if (label != null)
                {
                    fundoo.Label.Remove(label);
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
