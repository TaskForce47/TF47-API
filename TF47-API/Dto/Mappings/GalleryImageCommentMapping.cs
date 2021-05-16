using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class GalleryImageCommentMapping
    {
        public static GalleryImageCommentResponse ToGalleryImageCommentResponse(this GalleryImageComment data)
        {
            if (data == null) return null;
            return new GalleryImageCommentResponse(data.GalleryImageCommentId, data.Comment, data.User.Username, data.UserId, data.IsEdited, data.TimeLastEdited, data.TimeCreated);
        }

        public static IEnumerable<GalleryImageCommentResponse> ToGalleryImageCommentResponseIEnumerable(
            this IEnumerable<GalleryImageComment> data)
        {
            return data?.Select(x => ToGalleryImageCommentResponse(x));
        }
    }
}