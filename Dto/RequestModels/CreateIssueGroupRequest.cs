using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TF47_Backend.Dto.RequestModels
{
    public class CreateIssueGroupRequest
    {
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
    }

    public class CreateIssueRequest
    {
        public long IssueGroupId { get; set; }
        public string[] Tags { get; set; }
        public string Title { get; set; }
    }

    public class UpdateIssueRequest
    {
        public string[] Tags { get; set; }
        public string Title { get; set; }
    }

    public class CreateIssueItemRequest
    {
        public long IssueId { get; set; }
        public string Message { get; set; }
    }

    public class UpdateIssueItemRequest
    {
        public string Message { get; set; }
    }
}
