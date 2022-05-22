using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repository_Layer.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public virtual IList<Note> Notes { get; set; }
        //public virtual IList<Label> Label { get; set; }

    }
}
