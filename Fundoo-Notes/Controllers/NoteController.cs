using Business_Layer.Interfaces;
using Common_Layer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class NoteController : ControllerBase
    {
        FundooDbContext fundoo;
        INoteBL noteBL;

        public NoteController(FundooDbContext fundoo, INoteBL noteBL)
        {
            this.fundoo = fundoo;
            this.noteBL = noteBL;
        }

        [Authorize]
        [HttpPost("AddNote")]
        public async Task<ActionResult> AddNote(NotePostModel note)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);

                await this.noteBL.AddNote(UserId, note);
                return this.Ok(new { success = true, message = "Note Added Successfully" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpDelete("RemoveNote/{NoteID}")]
        public async Task<ActionResult> DeleteNote(int NoteID)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if (note == null)
                    return this.BadRequest(new { success = false, message = "Sorry! This note does not exist." });
                await this.noteBL.DeleteNote(UserID, NoteID);
                return this.Ok(new { success = true, message = "Note Removed Successfully" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetAllNotes")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                List<Note> notes = await this.noteBL.GetAll(UserID);
                return this.Ok(new { success = true, message = "These notes are:", data = notes });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPut("ChangeColour/{NoteID}/{Colour}")]
        public async Task<ActionResult> ChangeColour(int NoteID, string Colour)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if (note == null)
                    return this.BadRequest(new { success = false, message = "Sorry! This note does not exist." });
                await this.noteBL.ChangeColour(UserID, NoteID, Colour);
                return this.Ok(new { success = true, message = "Colour Changed Successfully" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPut("ArchiveNote/{NoteID}")]
        public async Task<ActionResult> ArchiveNote(int NoteID)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if (note == null)
                    return this.BadRequest(new { success = false, message = "Sorry! This note does not exist." });
                await this.noteBL.ArchiveNote(UserID, NoteID);
                return this.Ok(new { success = true, message = "Archive Changed Successfully" });
            }
            catch (Exception) 
            { 
                throw; 
            }
        }
        [Authorize]
        [HttpPut("PinNote/{NoteID}")]
        public async Task<ActionResult> PinNote(int NoteID)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if (note == null)
                    return this.BadRequest(new { success = false, message = "Sorry! This note does not exist." });
                await this.noteBL.PinNote(UserID, NoteID);
                return this.Ok(new { success = true, message = "Pin Changed Successfully" });
            }
            catch (Exception) 
            { 
                throw; 
            }
        }
        [Authorize]
        [HttpPut("Reminder/{NoteID}")]
        public async Task<ActionResult> Reminder(int NoteID, DateTime ReminderDate)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if (note == null)
                    return this.BadRequest(new { success = false, message = "Sorry! This note does not exist." });
                await this.noteBL.Reminder(UserID, NoteID, ReminderDate);
                return this.Ok(new { success = true, message = "Reminder Changed Successfully" });
            }
            catch (Exception) 
            { 
                throw; 
            }
        }
        [Authorize]
        [HttpPut("TrashNote/{NoteID}")]
        public async Task<ActionResult> TrashNote(int NoteID)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if (note == null)
                    return this.BadRequest(new { success = false, message = "Sorry! This note does not exist." });
                await this.noteBL.TrashNote(UserID, NoteID);
                return this.Ok(new { success = true, message = "Trash Changed Successfully" });
            }
            catch (Exception) 
            { 
                throw; 
            }
        }
    }
}