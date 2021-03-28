using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TF47_API.Helper;

namespace TF47_API.Services.SquadManager
{
    public class ImageToPaaConverter
    {
        private readonly string _path;

        public ImageToPaaConverter()
        {
            _path = PathCombiner.Combine(Environment.CurrentDirectory, "Includes", "armake_w64.exe");
        }

        public async Task<bool> Convert(string inputFile, CancellationToken cancellationToken)
        {

            if (! File.Exists(inputFile))
                throw new Exception($"InputFile {inputFile} does not exist");

            var outputFile = inputFile.Replace(".png", ".paa");
            if (File.Exists(outputFile))
                File.Delete(outputFile);
            
            var process = new Process();
            process.StartInfo = OperatingSystem.IsWindows()
                ? new ProcessStartInfo(_path, $"img2paa -f {inputFile} {outputFile}")
                
                //transform "/folder/to/picture.png" to "\folder\to\picture.png"
                : new ProcessStartInfo("wineconsole", $"{_path} img2paa -f {inputFile} {outputFile}");
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
