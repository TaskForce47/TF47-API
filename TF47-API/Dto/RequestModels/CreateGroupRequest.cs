using System.Collections.Generic;

namespace TF47_API.Dto.RequestModels
{
    public record CreateGroupRequest(string Name, string Description, string BackgroundColor, string TextColor, bool IsVisible, 
        IEnumerable<long> Permissions);
}