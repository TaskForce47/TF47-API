﻿using System;
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
        public bool VotingEnabled { get; set; }
        
        public User Uploader { get; set; }
        public Guid UploaderId { get; set; }
        
        public ICollection<User> UpVotes { get; set; }
        public ICollection<User> DownVotes { get; set; }
        
        public ICollection<GalleryImageComment> GalleryImageComments { get; set; }
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
}