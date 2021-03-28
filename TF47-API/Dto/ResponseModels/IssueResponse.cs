using System;
using System.Collections.Generic;

namespace TF47_API.Dto.ResponseModels
{
    public record IssueGroupResponse(long IssueGroupId, string GroupName, string GroupDescription,
        DateTime TimeGroupCreated, IEnumerable<IssueResponse> Issues);

    public record IssueItemResponse(long IssueItemId, Guid AuthorId, string AuthorName, string Message,
        DateTime TimeCreated, DateTime? TimeLastEdited);

    public record IssueTagResponse(long IssueTagId, string TagName, string Color);

    public record IssueResponse(long IssueId, string Title, bool IsClosed, Guid IssueCreator, string CreatorName,
        DateTime TimeCreated, DateTime TimeLastUpdated, IEnumerable<IssueItemResponse> IssueItems, 
        IEnumerable<IssueTagResponse> IssueTags);
}
