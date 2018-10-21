using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestOutlookAddin.Tools
{
    public class AttachmentInfo
    {
        public string Filename { get; set; }
        public string Path { get; set; }
        public bool isEmbedded { get; set; }
        public int AttachmentType { get; set; }

    }
}
