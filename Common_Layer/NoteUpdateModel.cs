using System;
using System.Collections.Generic;
using System.Text;

namespace Common_Layer
{
    public class NoteUpdateModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Colour { get; set; }

        public bool IsPin { get; set; }
        public bool IsReminder { get; set; }
        public bool IsArchive { get; set; }
        public bool IsTrash { get; set; }

    }
}
