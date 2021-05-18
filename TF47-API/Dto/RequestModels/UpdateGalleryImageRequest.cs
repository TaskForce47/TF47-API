namespace TF47_API.Dto
{
    public record UpdateGalleryImageRequest(string Name, string Description, bool? VotingEnabled, long? GalleryId);
}