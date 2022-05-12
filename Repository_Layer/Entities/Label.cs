using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repository_Layer.Entities
{
    public class Label
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelID { get; set; }
        public string LabelName { get; set; }

        public int? UserID { get; set; }
        public virtual User user { get; set; }
        public int? NoteID { get; set; }
        public virtual Note note { get; set; }


    }
}
