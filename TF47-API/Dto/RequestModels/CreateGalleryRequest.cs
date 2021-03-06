﻿namespace TF47_API.Dto.RequestModels
{
    public record CreateGalleryRequest(string Name, string Description);

    public record UpdateGalleryRequest(string Name, string Description);

    public record CreateGalleryImageRequest(string Name, string Description, bool VotingEnabled);

    public record CreateGalleryImageCommentRequest(string Comment);

    public record UpdateGalleryImageCommentRequest(string Comment);
}