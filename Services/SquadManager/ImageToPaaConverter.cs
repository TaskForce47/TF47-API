using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TF47_Backend.Helper;

namespace TF47_Backend.Services.SquadManager
{
    public class ImageToPaaConverter
    {
        private readonly string _path; 
        
        public ImageToPaaConverter()
        {
            _path = PathCombiner.Combine(Environment.CurrentDirectory, "Includes", "ImageToPAA.exe");
        }

        public async Task<bool> Convert(string inputFile, CancellationToken cancellationToken)
        {
            var fileInfo = new FileInfo(_path);
            if (!File.Exists(_path))
                throw new Exception("ImageToPaa.exe not found");
            
            if (! File.Exists(inputFile))
                throw new Exception($"InputFile {inputFile} does not exist");
            
            var process = new Process();
            process.StartInfo = OperatingSystem.IsWindows()
                ? new ProcessStartInfo(_path, $"\"{inputFile}\"")
                //transform "/folder/to/picture.png" to "\folder\to\picture.png"
                : new ProcessStartInfo("wineconsole", $"{_path} \"{inputFile.Replace("/", "\\")}\"");
            //process.StartInfo.CreateNoWindow = true;
                
            process.Start();
            var exitCode = await Task.Run(async () =>
            {
                while (!process.HasExited)
                {
                    if (cancellationToken.IsCancellationRequested)
                        process.Kill();
                    await Task.Delay(200, cancellationToken);
                }
                return process.ExitCode;
            }, cancellationToken);

            return exitCode == 0;
        }
    }
}