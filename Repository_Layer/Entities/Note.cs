using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repository_Layer.Entities
{
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Colour { get; set; }

        public bool IsPin { get; set; }
        public bool IsReminder { get; set; }
        public bool IsArchive { get; set; }
        public bool IsTrash { get; set; }

        public DateTime RegisterDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public DateTime ReminderDate { get; set; }

        public int UserID { get; set; }
        public virtual User user { get; set; }
        public virtual IList<Label> Label { get; set; }

    }
}
