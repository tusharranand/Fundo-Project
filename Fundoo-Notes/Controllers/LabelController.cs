using Business_Layer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository_Layer.Entities;
using Repository_Layer.FundooContext;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [Authorize]
        [HttpPost("AddLabel/{LabelName}")]
        public async Task<ActionResult> AddLabel(int NoteID, string LabelName)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                await this.labelBL.AddLabel(UserID, NoteID, LabelName);
                return this.Ok(new {success = true, message = "Label Added Successfully"});
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpGet("GetAllLabels")]
        public async Task<ActionResult> GetAllLabel()
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                List<Label> list = new List<Label>();
                list = await this.labelBL.GetAllLabels(UserID);
                if (list == null)
                    return this.BadRequest(new { success = false, message = "No labels are availale for this user" });
                return this.Ok(new { success = true, message = "All added Labels are,", data = list });
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
