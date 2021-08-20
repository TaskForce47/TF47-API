using System;
using System.Collections.Generic;
using TF47_API.Dto.Response;

namespace TF47_API.Dto.ResponseModels
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Errors = null;
        }
    }
}