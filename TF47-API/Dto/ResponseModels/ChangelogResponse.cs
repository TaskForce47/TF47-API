using System;

namespace TF47_API.Dto.ResponseModels
{
    public record ChangelogResponse(long ChangelogId, string Title, string[] Tags, string Text, DateTime TimeReleased)
    {
    }
    
}
