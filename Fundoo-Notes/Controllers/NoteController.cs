using Business_Layer.Interfaces;
using Common_Layer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Repository_Layer.Entities;
using Repository_Layer.FundooContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo_Notes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        FundooDbContext fundoo;
        INoteBL noteBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        public NoteController(FundooDbContext fundoo, INoteBL noteBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.fundoo = fundoo;
            this.noteBL = noteBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
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
        [HttpGet("GetAllNotesForAUser")]
        public async Task<ActionResult> GetAllNotes()
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
        [HttpGet("GetAParticularNote/{NoteID}")]
        public async Task<ActionResult> GetNote(int NoteID)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                Note note = await this.noteBL.GetNote(UserID, NoteID);
                return this.Ok(new { success = true, message = "Required note is:", data = note });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpGet("GetAllNotesRedis")]
        public async Task<ActionResult> GetAll_ByRadisCache()
        {
            try
            {
                string key = "getnote";
                string serializeNoteList;
                var noteList = new List<Note>();
                var redisNoteList = await distributedCache.GetAsync(key);
                if (redisNoteList != null)
                {
                    serializeNoteList = Encoding.UTF8.GetString(redisNoteList);
                    noteList = JsonConvert.DeserializeObject<List<Note>>(serializeNoteList);
                }
                else
                {
                    var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                    int UserID = Int32.Parse(userid.Value);
                    noteList = await this.noteBL.GetAll(UserID);
                    serializeNoteList = JsonConvert.SerializeObject(noteList);
                    redisNoteList = Encoding.UTF8.GetBytes(serializeNoteList);
                    var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20)).SetAbsoluteExpiration(TimeSpan.FromHours(6));
                    await distributedCache.SetAsync(key, redisNoteList, option);
                }
                return this.Ok(new { success = true, message = "Get note successful!!!", data = noteList });
            }
            catch (Exception ex)
            {
                throw ex;
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

        [Authorize]
        [HttpPut("UpdateNote")]
        public async Task<ActionResult> UpdateNote(int NoteID, NoteUpdateModel updateNote)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserID", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                var note = fundoo.Note.FirstOrDefault(x => x.UserID == UserID && x.NoteID == NoteID);
                if (note == null)
                    return this.BadRequest(new { success = false, message = "Sorry! This note does not exist." });
                Note noteResult = await this.noteBL.UpdateNote(UserID, NoteID, updateNote);
                return this.Ok(new { success = true, message = "Note was Updated" , data = noteResult});
            }
            catch (Exception) 
            { 
                throw; 
            }
        }
    }
}