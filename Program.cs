// See https://aka.ms/new-console-template for more information
// ler arquivo json para obter diretórios de chegagem e destino de arquivos

using System.Text.Json;
using CopyFiles.Models;

public class Program
{
    public static JsonSerializerOptions GetJsonOptions()
    {
        return new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public static void Main()
    {
        string backupConfig = Path.Combine(Directory.GetCurrentDirectory(), "origins.json");
        string jsonString = File.ReadAllText(backupConfig);
        var jsonOptions = GetJsonOptions();
        var dataBackupConfig = JsonSerializer.Deserialize<BackupConfig>(jsonString, jsonOptions)!;
        if (dataBackupConfig == null) throw new ArgumentException(nameof(DirectoryBackup));

        if (dataBackupConfig?.DestinationRoot?.Count <= 0 || dataBackupConfig?.DestinationRoot?.Count <= 0) { return; }
        var today = DateTime.Today;
        if (dataBackupConfig?.DirectoriesToBackup == null)
        {
            throw new InvalidOperationException("O caminho de backup não está configurado.");
        }
        string todayPath = Path.Combine(dataBackupConfig.DirectoriesToBackup, today.ToString("dd.MM.yyyy"));

        for (int i = 0; i < dataBackupConfig?.DestinationRoot?.Count; i++)
        {
            if (!Path.Exists(dataBackupConfig.DestinationRoot[i].SourcePath) &&
                !Path.Exists(Path.Combine(dataBackupConfig.DirectoriesToBackup, dataBackupConfig.DestinationRoot[i].DestinationFolder))) { return; }
            if (!Path.Exists(todayPath)) { Directory.CreateDirectory(todayPath); }
            if (!Path.Exists(todayPath)) { return; }
            if (dataBackupConfig.DestinationRoot[i].SourcePath == null) { return; }
            var sourceFiles = new System.IO.DirectoryInfo(dataBackupConfig.DestinationRoot[i].SourcePath).GetFiles();
            if (sourceFiles.Length == 0) { return; }

            for (global::System.Int32 j = 0; j < dataBackupConfig?.DestinationRoot[i]?.FileFormats?.Count; j++)
            {
                HandlerFiles(dataBackupConfig.DestinationRoot[i], todayPath, dataBackupConfig.DestinationRoot[i].FileFormats[j]);
            }
        }
    }

    private static void HandlerFiles(DirectoryBackup directoryBackup, string todayPath, string fileFormat)
    {
        var originsFiles = new System.IO.DirectoryInfo(directoryBackup.SourcePath).GetFiles(fileFormat);
        if (originsFiles.Length == 0) { return; }

        for (int i = 0; i < originsFiles.Length; i++)
        {
            if (directoryBackup.IndividualTargetFormats)
            {
                _ = fileFormat switch
                {
                    ".mp4" => Mover(originsFiles[i], todayPath, originsFiles[i].Extension),
                    ".mp3" => Mover(originsFiles[i], todayPath, originsFiles[i].Extension),
                    ".m4a" => Mover(originsFiles[i], todayPath, originsFiles[i].Extension),
                    ".jpg" => Mover(originsFiles[i], todayPath, originsFiles[i].Extension),
                    _ => "",
                };
            } else
            {
                File.Move(originsFiles[i].FullName, Path.Combine(todayPath, originsFiles[i].Name));
            }
        }
    }
    private static object Mover(FileInfo fileInfos, string todayPath, string extension)
    {
        var pathWithExtension = Path.Combine(todayPath, extension);
        if (!Directory.Exists(pathWithExtension))
        {
            Directory.CreateDirectory(pathWithExtension);
        }
        File.Move(fileInfos.Name, Path.Combine(pathWithExtension, fileInfos.Name));
        return null;
    }
}
