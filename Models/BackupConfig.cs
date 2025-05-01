using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyFiles.Models
{
    public class BackupConfig
    {
        public string DirectoriesToBackup { get; set; }
        public List<DirectoryBackup> DestinationRoot { get; set; }

        public BackupConfig()
        {
            DirectoriesToBackup = string.Empty; // ou um valor padrão
            DestinationRoot = new List<DirectoryBackup>();
        }
    }
}
