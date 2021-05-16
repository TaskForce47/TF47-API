using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_API.Database.Models.Services
{
    [Table("ServiceGalleries")]
    public class Gallery
    {
        public long GalleryId { get; set; }
        
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        
        public DateTime TimeCreated { get; set; } = DateTime.Now;
        
        public ICollection<GalleryImage> GalleryImages { get; set; }
    }

    [Table("GalleryImages")]
    public class GalleryImage
    {
        public long GalleryImageId { get; set; }
        public DateTime TimeCreated { get; set; }
        
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        public string ImageFileName { get; set; }
        public Gallery Gallery { get; set; }
        public long GalleryId { get; set; }
        
        public ICollection<GalleryImageComment> GalleryImageComments { get; set; }
        public ICollection<GalleryImageReaction> GalleryImageReactions { get; set; }
    }

    [Table("GalleryImageComments")]
    public class GalleryImageComment
    {
        public long GalleryImageCommentId { get; set; }

        public string Comment { get; set; }
        public bool IsEdited { get; set; } = false;
        public DateTime? TimeLastEdited { get; set; } = null;
        
        public GalleryImage GalleryImage { get; set; }
        public long GalleryImageId { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }
        
        public DateTime TimeCreated { get; set; } = DateTime.Now;
    }

    [Table("GalleryImageReactions")]
    public class GalleryImageReaction
    {
        public long GalleryImageReactionId { get; set; }
        public string EncodedReaction { get; set; }
        
        public GalleryImage GalleryImage { get; set; }
        public long GalleryImageId { get; set; }
        
        public ICollection<User> UsersReactions { get; set; }
        
        public DateTime TimeCreated { get; set; } = DateTime.Now;
    }
}