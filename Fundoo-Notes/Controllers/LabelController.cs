using Business_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Repository_Layer.FundooContext;
using System;
using System.Threading.Tasks;

namespace Fundoo_Notes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LabelController : ControllerBase
    {
        FundooDbContext fundoo;
        ILabelBL labelBL;

        public LabelController(FundooDbContext fundoo, ILabelBL labelBL)
        {
            this.fundoo = fundoo;
            this.labelBL = labelBL;
        }
        [HttpPost("AddLabel/{LabelName}")]
        public async Task<ActionResult> AddLabel(int UserID, int NoteID, string LabelName)
        {
            try
            {
                await this.labelBL.AddLabel(UserID, NoteID, LabelName);
                return this.Ok(new {success = true, message = "Label Added Successfully"});
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
