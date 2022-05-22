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

    }
}
