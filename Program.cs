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

            Parallel.ForEach(sourceFiles, file =>
            {
                try
                {
                    if (dataBackupConfig.DestinationRoot[i].IndividualTargetFormats)
                    {
                        ProcessFileByFormat(file, todayPath, dataBackupConfig.DestinationRoot[i]);
                    } else 
                    {
                        SafeFileMove(file, todayPath);
            }
        }
                catch (Exception ex)
    {

                    throw;
                }
            });
        }
    }
    private static void SafeFileMove(FileInfo fileInfos, string todayPath)
        {
        if (!Directory.Exists(todayPath))
            {
            Directory.CreateDirectory(todayPath);
            }
        File.Move(fileInfos.Name, Path.Combine(todayPath, fileInfos.Name));
        }
    private static void ProcessFileByFormat(FileInfo sourceFile, string todayPath, DirectoryBackup directoryBackup)
    {
        for (int i = 0; i < directoryBackup?.FileFormats?.Count; i++) {
            if (sourceFile.Extension != directoryBackup.FileFormats[i]) { continue; }

            var destinationFolder = Path.Combine(todayPath, directoryBackup.DestinationFolder);
            if (!Directory.Exists(destinationFolder))
        {
                Directory.CreateDirectory(destinationFolder);
            }
            File.Move(sourceFile.FullName, Path.Combine(destinationFolder, sourceFile.Name));
        }
        File.Move(fileInfos.Name, Path.Combine(pathWithExtension, fileInfos.Name));
        return null;
    }
}
