using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class GalleryMapping
    {
        public static GalleryResponse ToGalleryResponse(this Gallery data)
        {
            if (data == null) return null;

            return new GalleryResponse(data.GalleryId, data.Name, data.Description, data.TimeCreated,
                data.GalleryImages.ToGalleryImageResponseIEnumerable());
        }

        public static IEnumerable<GalleryResponse> ToGalleryResponseIEnumerable(this IEnumerable<Gallery> data)
        {
            return data?.Select(x => ToGalleryResponse(x));
        }
    }
}