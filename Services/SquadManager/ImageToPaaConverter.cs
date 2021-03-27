using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TF47_Backend.Services.SquadManager
{
    public class ImageToPaaConverter
    {
        private readonly string _path; 
        
        public ImageToPaaConverter()
        {
            _path = Path.Combine(Environment.CurrentDirectory, "Includes", "ImageToPaa.exe");
        }

        public async Task<bool> Convert(string inputFile, CancellationToken cancellationToken)
        {
            if (!File.Exists(_path))
                throw new Exception("ImageToPaa.exe not found");
            
            if (! File.Exists(inputFile))
                throw new Exception($"InputFile {inputFile} does not exist");
            
            var process = new Process();
            process.StartInfo = OperatingSystem.IsWindows()
                ? new ProcessStartInfo(_path, inputFile)
                : new ProcessStartInfo("wineconsole", $"{_path} {inputFile}");
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