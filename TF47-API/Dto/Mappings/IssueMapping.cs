using System.Collections.Generic;
using System.Linq;
using TF47_API.Database.Models.Services;
using TF47_API.Dto.ResponseModels;

namespace TF47_API.Dto.Mappings
{
    public static class IssueMapping
    {
        public static IssueGroupResponse ToIssueGroupResponse(this IssueGroup data)
        {
            return data == null
                ? null
                : new IssueGroupResponse(data.IssueGroupId, data.GroupName, data.GroupDescription, data.TimeGroupCreated,
                    data.Issues.ToSquadMemberResponseIEnumerable());
        }

        public static IEnumerable<IssueGroupResponse> ToIssueGroupResponseIEnumerable(this IEnumerable<IssueGroup> data)
        {
            return data?.Select(x => x.ToIssueGroupResponse());
        }
        
        public static IssueResponse ToIssueResponse(this Issue data)
        {
            return data == null
                ? null
                : new IssueResponse(data.IssueId, data.Title, data.IsClosed, data.IssueCreator.UserId,
                    data.IssueCreator.Username, data.TimeCreated,
                    data.TimeLastUpdated, data.IssueItems.ToIssueItemResponseIEnumerable(),
                    data.IssueTags.ToIssueTagResponseIEnumerable());
        }

        public static IEnumerable<IssueResponse> ToSquadMemberResponseIEnumerable(
            this IEnumerable<Issue> data)
        {
            return data?.Select(x => x.ToIssueResponse());
        }

        public static IssueItemResponse ToIssueItemResponse(this IssueItem data)
        {
            return data == null
                ? null
                : new IssueItemResponse(data.IssueItemId, data.Author.UserId, data.Author.Username, data.Message,
                    data.TimeCreated, data.TimeLastEdited);
        }

        public static IEnumerable<IssueItemResponse> ToIssueItemResponseIEnumerable(this IEnumerable<IssueItem> data)
        {
            return data?.Select(x => x.ToIssueItemResponse());
        }

        public static IssueTagResponse ToIssueTagResponse(this IssueTag data)
        {
            return data == null
                ? null
                : new IssueTagResponse(data.IssueTagId, data.TagName, data.Color);
        }

        public static IEnumerable<IssueTagResponse> ToIssueTagResponseIEnumerable(
            this IEnumerable<IssueTag> data)
        {
            return data?.Select(x => x.ToIssueTagResponse());
        }
    }
}