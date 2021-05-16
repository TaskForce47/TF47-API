using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class GalleryImageReactionMapping
    {
        public static GalleryImageReactionResponse ToGalleryImageReactionResponse(
            this GalleryImageReaction data)
        {
            if (data == null) return null;
            return new GalleryImageReactionResponse(data.GalleryImageReactionId, data.EncodedReaction, data.TimeCreated,
                data.UsersReactions.Select(x => new UserInfo(x.UserId, x.Username)));
        }

        public static IEnumerable<GalleryImageReactionResponse> ToGalleryImageReactionResponseIEnumerable(
            this IEnumerable<GalleryImageReaction> data)
        {
            return data?.Select(x => x.ToGalleryImageReactionResponse());
        }
    }
}