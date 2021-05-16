using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Database.Models.Services;
using TF47_API.Dto;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.RequestModels;
using TF47_API.Services;

namespace TF47_API.Controllers.Gallery
{
    [Authorize]
    [Route("api/[controller]")]
    public class GalleryImageController : ControllerBase
    {
        private readonly ILogger<GalleryImageController> _logger;
        private readonly DatabaseContext _database;
        private readonly ImageHandlerService _imageHandlerService;

        public GalleryImageController(
            ILogger<GalleryImageController> logger,
            DatabaseContext database, 
            ImageHandlerService imageHandlerService)
        {
            _logger = logger;
            _database = database;
            _imageHandlerService = imageHandlerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetImages()
        {
            var galleryImages = await _database.GalleryImages
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.GalleryImageComments)
                .Include(x => x.GalleryImageReactions)
                .ThenInclude(x => x.UsersReactions)
                .ToListAsync();
            return Ok(galleryImages.ToGalleryImageResponseIEnumerable());
        }

        [HttpGet("{galleryImageId:long}")]
        public async Task<IActionResult> GetImage(long galleryImageId)
        {
            var galleryImage = await _database.GalleryImages
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.GalleryImageComments)
                .Include(x => x.GalleryImageReactions)
                .ThenInclude(x => x.UsersReactions)
                .FirstOrDefaultAsync(x => x.GalleryImageId == galleryImageId);

            if (galleryImage == null) return BadRequest("GalleryImageId provided does not exist");

            return Ok(galleryImage.ToGalleryImageResponse());
        }

        [HttpGet("recent")]
        [HttpGet("recent/{max:int}")]
        [HttpGet("recent/{max:int}/{skip:int}")]
        public async Task<IActionResult> GetRecentImages(int max = 100, int skip = 0)
        {
            var galleryImages = await _database.GalleryImages
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.GalleryImageComments)
                .Include(x => x.GalleryImageReactions)
                .ThenInclude(x => x.UsersReactions)
                .Skip(skip)
                .Take(max)
                .OrderByDescending(x => x.GalleryImageId)
                .ToListAsync();

            return Ok(galleryImages.ToGalleryImageResponseIEnumerable());
        }
        
        [HttpGet("recentByGallery/{galleryId:long}")]
        [HttpGet("recentByGallery/{galleryId:long}/{max:int}")]
        [HttpGet("recentByGallery/{galleryId:long}/{max:int}/{skip:int}")]
        public async Task<IActionResult> GetRecentImages(long galleryId, int max = 100, int skip = 0)
        {
            var galleryImages = await _database.GalleryImages
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.GalleryImageComments)
                .Include(x => x.GalleryImageReactions)
                .ThenInclude(x => x.UsersReactions)
                .Where(x => x.GalleryId == galleryId)
                .Skip(skip)
                .Take(max)
                .OrderByDescending(x => x.GalleryImageId)
                .ToListAsync();

            return Ok(galleryImages.ToGalleryImageResponseIEnumerable());
        }

        [HttpPut("{galleryImageId:long}")]
        public async Task<IActionResult> UpdateImage(long galleryImageId, [FromBody] UpdateGalleryImageRequest request)
        {
            var galleryImage = await _database.GalleryImages
                .FirstOrDefaultAsync(x => x.GalleryImageId == galleryImageId);

            if (galleryImage == null) return BadRequest("GalleryImageId provided does not exist");

            if (request.GalleryId.HasValue)
            {
                var gallery = await _database.Galleries
                    .FirstOrDefaultAsync(x => x.GalleryId == request.GalleryId.Value);
                if (gallery == null)
                    return BadRequest("New GalleryId provided in body does not exist");
                galleryImage.Gallery = gallery;
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
                galleryImage.Name = request.Name;
            if (!string.IsNullOrWhiteSpace(request.Description))
                galleryImage.Description = request.Description;

            await _database.SaveChangesAsync();
            
            return Ok(galleryImage.ToGalleryImageResponse());
        } 

        [HttpDelete("{galleryImageId:long}")]
        public async Task<IActionResult> RemoveImage(long galleryImageId)
        {
            var galleryImage = await _database.GalleryImages
                .FirstOrDefaultAsync(x => x.GalleryImageId == galleryImageId);

            if (galleryImage == null) return BadRequest("GalleryImageId provided does not exist");

            var result = await _imageHandlerService.RemoveImageAsync(galleryImage);
            if (!result)
                return Problem("Could not remove image, either it does not exist or some other error occured", "", 500,
                    "Failed to remove image");

            _database.Remove(galleryImage);
            await _database.SaveChangesAsync();
            return Ok();
        }
    }
}