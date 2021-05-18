using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.RequestModels;
using TF47_API.Services;

namespace TF47_API.Controllers.Gallery
{
    [Authorize]
    [Route("api/[controller]")]
    public class GalleryController : ControllerBase
    {
        private readonly ILogger<GalleryController> _logger;
        private readonly DatabaseContext _database;
        private readonly ImageHandlerService _imageHandlerService;
        private readonly IUserProviderService _userProviderService;

        public GalleryController(
            ILogger<GalleryController> logger,
            DatabaseContext database,
            ImageHandlerService imageHandlerService,
            IUserProviderService userProviderService)
        {
            _logger = logger;
            _database = database;
            _imageHandlerService = imageHandlerService;
            _userProviderService = userProviderService;
        }

        [HttpGet("{galleryId:long}")]
        public async Task<IActionResult> GetGallery(long galleryId)
        {
            var gallery = await _database.Galleries
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GalleryId == galleryId);
            if (gallery == null) return BadRequest("GalleryId provided does not exist");

            return Ok(gallery.ToGalleryResponse());
        }

        [HttpGet("")]
        public async Task<IActionResult> GetGalleries()
        {
            var galleries = await _database.Galleries.ToListAsync();
            return Ok(galleries.ToGalleryResponseIEnumerable());
        }

        [HttpPost("")]
        public async Task<IActionResult> CreatedGallery([FromBody] CreateGalleryRequest request)
        {
            var newGallery = new Database.Models.Services.Gallery
            {
                Name = request.Name,
                Description = request.Description,
                TimeCreated = DateTime.Now
            };

            _database.Galleries.Add(newGallery);
            await _database.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetGallery), new {galleryId = newGallery.GalleryId}, newGallery);
        }

        [HttpPut("{galleryId:long}")]
        public async Task<IActionResult> UpdateGallery(long galleryId, [FromBody] UpdateGalleryRequest request)
        {
            var gallery = await _database.Galleries.FirstOrDefaultAsync(x => x.GalleryId == galleryId);
            if (gallery == null) return BadRequest("GalleryId provided does not exist");

            if (!string.IsNullOrWhiteSpace(request.Name))
                gallery.Name = request.Name;
            if (!string.IsNullOrWhiteSpace(request.Description))
                gallery.Description = request.Description;

            await _database.SaveChangesAsync();
            return Ok(gallery.ToGalleryResponse());
        }

        [HttpDelete("{galleryId:long}")]
        public async Task<IActionResult> RemoveGallery(long galleryId)
        {
            var gallery = await _database.Galleries
                .Include(x => x.GalleryImages)
                .FirstOrDefaultAsync(x => x.GalleryId == galleryId);

            if (gallery == null) return BadRequest("GalleryId provided does not exist");
            if (gallery.GalleryImages.Count > 0)
                return BadRequest("Cannot remove non empty gallery. Gallery still contains images");

            _database.Galleries.Remove(gallery);
            await _database.SaveChangesAsync();
            return Ok();
        }
        
        [Authorize]
        [HttpPut("{galleryId:long}/uploadImage")]
        public async Task<IActionResult> UploadImage(long galleryId, IFormFile file, [FromForm] CreateGalleryImageRequest request)
        {
            var gallery = await _database.Galleries
                .FirstOrDefaultAsync(x => x.GalleryId == galleryId);

            var uploader = await _userProviderService.GetDatabaseUserAsync(HttpContext);
            _database.Attach(uploader);
            
            if (gallery == null) return BadRequest("GalleryId provided does not exist");
            
            if (file.ContentType is not ("image/png" or "image/jpeg")) 
                return BadRequest("Only jpeg or png images are allowed");
            
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var (galleryUploadStatus, newImage) = await _imageHandlerService.UploadImageAsync(stream, CancellationToken.None);
            
            stream.Close();

            switch (galleryUploadStatus)    
            {
                case GalleryUploadStatus.Success:
                    newImage.Uploader = uploader;
                    newImage.Name = request.Name;
                    newImage.Description = request.Description;
                    newImage.TimeCreated = DateTime.Now;
                    newImage.Gallery = gallery;
                    newImage.VotingEnabled = request.VotingEnabled;
                    newImage.DownVotes = new List<User>();
                    newImage.UpVotes = new List<User>();
                    newImage.GalleryImageComments = new List<GalleryImageComment>();

                    await _database.GalleryImages.AddAsync(newImage);
                    await _database.SaveChangesAsync();
                    return Ok(newImage.ToGalleryImageResponse());
                case GalleryUploadStatus.Repost:
                    return BadRequest("The Image provided does already exist. Repost?!");
                case GalleryUploadStatus.Error:
                    return Problem("Something went wrong while saving or transforming the image", "", 500,
                        "Failed to process");
                case GalleryUploadStatus.WrongSize:
                    return BadRequest("The image provided did not meet the required specification");
                default:
                    return Problem("unhandled error", "", 500);
            }
        }
    }
}