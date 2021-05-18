using System;
using System.Collections.Generic;
using TF47_API.Database.Models.Services;

namespace TF47_API.Dto.ResponseModels
{
    public record GalleryResponse(long GalleryId, string Name, string Description, DateTime TimeCreated,
        IEnumerable<GalleryImageResponse> GalleryImageResponse);

    public record GalleryImageResponse(long GalleryImageId, string Name, string Description, UserInfo Uploader,
        string ImageUrl, string ImagePreviewUrl, bool VotingEnabled, DateTime TimeCreated,
        IEnumerable<GalleryImageCommentResponse> GalleryImageComments,
        IEnumerable<UserInfo> UpVotes, IEnumerable<UserInfo> DownVotes);

    public record GalleryImageCommentResponse(long GalleryImageCommentId, string Comment, string Username,
        Guid UserId, bool IsEdited, DateTime? TimeLastEdited, DateTime TimeCreated);

    public record UserInfo(Guid? UserId, string Username);
}