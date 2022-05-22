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
        [Authorize]
        [HttpGet("GetAllLabelsForANote/{NoteID}")]
        public async Task<ActionResult> GetAllLabelsForANote(int NoteID)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                var note = fundoo.Note.FirstOrDefaultAsync(u => u.UserID == UserID && u.NoteID == NoteID);
                List<Label> list = new List<Label>();
                list = await this.labelBL.GetAllLabelsForANote(UserID, NoteID);
                if(list.Count==0)
                    return this.BadRequest(new { success = false, message = "No labels are availale for this note" });
                return this.Ok(new { success = true, message = "All added Labels for given note ID are,", data = list });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpDelete("DeleteLabel/{LabelID}")]
        public async Task<ActionResult> DeleteLabel(int LabelID)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                await this.labelBL.DeleteLabel(UserID, LabelID);
                return this.Ok(new { success = true, message = "The label was deleted."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPut("UpdateNote/{LabelID}/{LabelName}/{NoteID}")]
        public async Task<ActionResult> UpdateNote(int LabelID, string LabelName, int NoteID)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                await this.labelBL.UpdateLabel(UserID, LabelID, LabelName, NoteID);
                return this.Ok(new { success = true, message = "The label was updated." });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
