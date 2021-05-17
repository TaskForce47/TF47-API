using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TF47_API.Database;
using TF47_API.Dto.Mappings;
using TF47_API.Dto.RequestModels;
using TF47_API.Services;

namespace TF47_API.Controllers.Gallery
{
    [Route("api/[controller]")]
    public class GalleryImageCommentController : ControllerBase
    {
        private readonly ILogger<GalleryImageCommentController> _logger;
        private readonly DatabaseContext _database;
        private readonly UserProviderService _userProviderService;

        public GalleryImageCommentController(
            ILogger<GalleryImageCommentController> logger,
            DatabaseContext database,
            UserProviderService userProviderService)
        {
            _logger = logger;
            _database = database;
            _userProviderService = userProviderService;
        }

        [HttpGet("{galleryImageCommentId:long}")]
        public async Task<IActionResult> GetGalleryImageComment(long galleryImageCommentId)
        {
            var galleryImageComment = await _database.GalleryImageComments
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GalleryImageCommentId == galleryImageCommentId);

            if (galleryImageComment == null) return BadRequest("GalleryImageCommentId provided does not exist");
            return Ok(galleryImageComment.ToGalleryImageCommentResponse());
        }

        [Authorize]
        [HttpPut("{galleryImageCommentId:long}", Name = "GetGalleryImageComment")]
        public async Task<IActionResult> UpdateComment(long galleryImageCommentId,
            UpdateGalleryImageCommentRequest request)
        {
            var galleryImageComment = await _database.GalleryImageComments
                .FirstOrDefaultAsync(x => x.GalleryImageCommentId == galleryImageCommentId);

            var user = await _userProviderService.GetDatabaseUserAsync(HttpContext);
            
            if (galleryImageComment == null) return BadRequest("GalleryImageCommentId provided does not exist");
            if (galleryImageComment.UserId != user.UserId) return BadRequest("You can only alter your own comments");

            galleryImageComment.Comment = request.Comment;
            galleryImageComment.IsEdited = true;
            galleryImageComment.TimeLastEdited = DateTime.Now;

            await _database.SaveChangesAsync();
            return Ok(galleryImageComment.ToGalleryImageCommentResponse());
        }
    }
}