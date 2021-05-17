using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class GalleryImageMapping
    {
        public static GalleryImageResponse ToGalleryImageResponse(this GalleryImage data)
        {
            var imageAddress = $"{Settings.BaseUrl}/gallery/{data.ImageFileName}.png";
            return new GalleryImageResponse(data.GalleryImageId, data.Name, data.Description,
                new UserInfo(data.Uploader?.UserId, data.Uploader?.Username), imageAddress,
                data.TimeCreated, data.GalleryImageComments.ToGalleryImageCommentResponseIEnumerable(),
                data.UpVotes?.Select(x => new UserInfo(x?.UserId, x?.Username)),
                data.DownVotes?.Select(x => new UserInfo(x?.UserId, x?.Username)));
        }

        public static IEnumerable<GalleryImageResponse> ToGalleryImageResponseIEnumerable(
            this IEnumerable<GalleryImage> data)
        {
            return data?.Select(x => x.ToGalleryImageResponse());
        }
    }
}