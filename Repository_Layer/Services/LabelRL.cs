using Microsoft.Extensions.Configuration;
using Repository_Layer.Entities;
using Repository_Layer.FundooContext;
using Repository_Layer.Interfaces;
using System;
using System.Collections.Generic;
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
                Label labelData = new Label();
                labelData.LabelName = LabelName;
                labelData.UserID = UserID; 
                labelData.NoteID = NoteID;
                fundoo.Add(labelData);
                await fundoo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
