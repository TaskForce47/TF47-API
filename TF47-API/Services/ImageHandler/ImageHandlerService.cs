using System;
using System.IO;
using System.IO.Pipelines;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using TF47_API.Database;
using TF47_API.Database.Models.Services;
using TF47_API.Helper;

namespace TF47_API.Services
{
    public class ImageHandlerService
    {
        private readonly ILogger<ImageHandlerService> _logger;
        private readonly DatabaseContext _database;
        private readonly IConfiguration _configuration;
        private readonly string _galleryFolder;

        public ImageHandlerService(
            ILogger<ImageHandlerService> logger,
            DatabaseContext database,
            IConfiguration configuration)
        {
            _logger = logger;
            _database = database;
            _configuration = configuration;
            _galleryFolder = PathCombiner.Combine(Environment.CurrentDirectory, "wwwroot", "gallery");

            if (!Directory.Exists(_galleryFolder))
                Directory.CreateDirectory(_galleryFolder);
        }
        
        public async Task<(GalleryUploadStatus, GalleryImage)> UploadImageAsync(Stream inputStream, CancellationToken cancellationToken)
        {
            using var cryptoProvider = SHA256.Create();
            inputStream.Position = 0;
            var computeHash = await cryptoProvider.ComputeHashAsync(inputStream, cancellationToken);
            var builder = new StringBuilder();  
            foreach (var t in computeHash)
            {
                builder.Append(t.ToString("x2"));
            }  
            var stringEncodedHash = builder.ToString();

            var imagePath = PathCombiner.Combine(_galleryFolder, $"{stringEncodedHash}.png");
            var imagePreviewPath = PathCombiner.Combine(_galleryFolder, $"{stringEncodedHash}_preview.png");
            if (File.Exists(imagePath)) return (GalleryUploadStatus.Repost, null);

            inputStream.Position = 0;
            try
            {
                var galleryImage = new GalleryImage {ImageFileName = stringEncodedHash};
                var physicalImage = await Image.LoadAsync(inputStream);

                if (physicalImage.Height < 400 || physicalImage.Width < 300)
                {
                    _logger.LogInformation(
                        $"Uploaded image did not meat the required size specifications, width: {physicalImage.Width} height: {physicalImage.Height}");
                    return (GalleryUploadStatus.WrongSize, null);
                }
                
                var pngEncoder = new PngEncoder
                {
                    CompressionLevel = PngCompressionLevel.Level9,
                    Quantizer = new WebSafePaletteQuantizer()
                };
                await physicalImage.SaveAsPngAsync(imagePath, pngEncoder, cancellationToken: cancellationToken);

                inputStream.Position = 0;
                physicalImage.Mutate(x => x.Resize(250, 250));
                
                await physicalImage.SaveAsPngAsync(imagePreviewPath, pngEncoder, cancellationToken);
                
                return (GalleryUploadStatus.Success, galleryImage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to transcode uploaded gallery image: {ex.Message}");
                return (GalleryUploadStatus.Error, null);
            }
        }
        
        public async Task<bool> RemoveImageAsync(GalleryImage galleryImage)
        {
            var imagePath = PathCombiner.Combine(_galleryFolder, $"{galleryImage.ImageFileName}.png");
            var imagePreviewPath = PathCombiner.Combine(_galleryFolder, $"{galleryImage.ImageFileName}_preview.png");
            if (File.Exists(imagePath) && File.Exists(imagePreviewPath))
            {
                try
                {
                    File.Delete(imagePath);
                    File.Delete(imagePreviewPath);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to remove image {galleryImage.Name}, fileId: {galleryImage.ImageFileName}, error: {ex.Message}");
                    return false;
                }
            }
            _logger.LogWarning($"Could not remove image, cannot find in file structure {galleryImage.Name}, fileId: {galleryImage.ImageFileName}");
            return false;
        }
        
        
    }
}