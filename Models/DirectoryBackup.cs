using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyFiles.Models
{
    public class DirectoryBackup
    {
        public string SourcePath { get; set; }
        public string DestinationFolder { get; set; }
        public bool IndividualTargetFormats { get; set; }
        public List<string>? FileFormats { get; set; }
        public DirectoryBackup() {
            FileFormats = new List<string>();
            IndividualTargetFormats = false;
            SourcePath = string.Empty;
            DestinationFolder = string.Empty;
        }
    }
}
