using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using BunnyCDN.Net.Storage;

namespace CollegeKings.Local.Uploader;

public class Uploader
{
    public required MainWindow MainWindow;
    public required string Description;
    public required string FolderPath;

    private readonly string _tempDirectoryPath;
    
    private readonly BunnyCDNStorage _bunnyCdn = new("collegekingsstorage", "ba39c6ef-f9e1-4ea7-a132e6d0f8c2-0aa3-4d1a");
    private readonly string _bunnyRoot;
    
    public Uploader(string project, OsType osType)
    {
        string osTypeString = osType.ToString().ToLower();
        project = project.ToLower().Replace(' ', '_');
        
        _tempDirectoryPath = Path.Join(Path.GetTempPath(), "college_kings", osTypeString);
        _bunnyRoot = @$"/collegekingsstorage/__bcdn_perma_cache__/pullzone__collegekings__22373407/wp-content/uploads/secured/{project}/{osTypeString}";
    }
    
    private async Task<bool> DownloadManifest()
    {
        await MainWindow.AddLog("Downloading Manifest...");
        
        string bunnyPath = _bunnyRoot + "/manifest.json";
        Directory.CreateDirectory(_tempDirectoryPath);

        try
        {
            await _bunnyCdn.DownloadObjectAsync(bunnyPath, Path.Join(_tempDirectoryPath, "remote_manifest.json"));
            return true;
        }
        catch (HttpRequestException)
        {
            await MainWindow.AddLog("Remote manifest not found");
            return false;
        }
    }

    private static string GenerateHash(string filePath)
    {
        using (FileStream stream = File.OpenRead(filePath))
        {
            using (HashAlgorithm algorithm = SHA1.Create())
            {
                byte[] hashBytes = algorithm.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "");
            }
        }
    }

    private async Task<Dictionary<string, string>> GenerateManifest()
    {
        await MainWindow.AddLog("Generating Local Manifest...");

        string[] filePaths = Directory.GetFiles(FolderPath, "*.*", SearchOption.AllDirectories);
        Dictionary<string, string> manifestList = filePaths.ToDictionary(filePath => filePath.Replace(FolderPath, ""), GenerateHash);

        string jsonString = JsonSerializer.Serialize(manifestList);
        await File.WriteAllTextAsync(Path.Join(_tempDirectoryPath, "local_manifest.json"), jsonString);
        
        return manifestList;
    }

    private async Task<Dictionary<string, bool>> CompareManifest(Dictionary<string, string> localManifest, Dictionary<string, string>? remoteManifest)
    {
        await MainWindow.AddLog("Comparing Manifests...");

        if (remoteManifest is null)
        {
            return localManifest.ToDictionary(x => x.Key, _ => false);
        }
        
        Dictionary<string, bool> changedFiles = new();
        
        foreach (string filePath in localManifest.Keys)
        {
            if (!remoteManifest.ContainsKey(filePath))
            {
                changedFiles.Add(filePath, false);
                continue;
            }

            if (localManifest[filePath] != remoteManifest[filePath])
            {
                changedFiles.Add(filePath, false);
            }
        }

        return changedFiles;
    }

    private async Task UploadFile(string file, IDictionary<string, bool> changedFiles)
    {
        await _bunnyCdn.UploadAsync(Path.Join(FolderPath, file),
            _bunnyRoot + $"/{file.Replace(FolderPath, "")}");
        changedFiles[file] = true;
    }
    
    private async Task UploadFiles(Dictionary<string, bool> changedFiles)
    {
        List<Task> taskList = new();
        foreach ((string file, bool isUploaded) in changedFiles)
        {
            if (!isUploaded)
            {
                taskList.Add(UploadFile(file, changedFiles));
            }

            if (taskList.Count != 50) continue;
            
            await Task.WhenAll(taskList);
            taskList.Clear();
            await MainWindow.AddLog($"Upload Progress: {changedFiles.Count(x => x.Value)}/{changedFiles.Count}");
        }

        await Task.WhenAll(taskList);
    }
    
    public async Task Run()
    {
        Task<bool> task1 = DownloadManifest();
        Task<Dictionary<string, string>> task2 = GenerateManifest();

        await Task.WhenAll(task1, task2);

        Dictionary<string, string>? remoteManifest = new();
        if (task1.Result)
        {
            string jsonString = await File.ReadAllTextAsync(Path.Join(_tempDirectoryPath, "remote_manifest.json"));
            remoteManifest = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        }
        
        Dictionary<string, bool> changedFiles = await CompareManifest(task2.Result, remoteManifest);

        while (true)
        {
            try
            {
                await UploadFiles(changedFiles);
                break;
            }
            catch (HttpRequestException)
            {
                await Task.Delay(5000);
            }
        }

        await _bunnyCdn.UploadAsync(Path.Join(_tempDirectoryPath, "local_manifest.json"),
            _bunnyRoot + "/manifest.json");
        
        await MainWindow.AddLog("Upload Done");
    }
}