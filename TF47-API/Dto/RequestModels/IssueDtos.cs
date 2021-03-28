using System.Collections;

namespace TF47_API.Dto.RequestModels
{
    public record CreateIssueGroupRequest(string GroupName, string GroupDescription);
    public record CreateIssueItemRequest(long IssueId, string Message);
    public record CreateIssueTagRequest(string TagName, string Color);
    public record CreateIssueRequest(long IssueGroupId, long[] Tags, string Title);

    public record UpdateIssueTagRequest(string TagName, string Color);
    public record UpdateIssueItemRequest(string Message);
    public record UpdateIssueRequest(string Title, long[] Tags);
}
