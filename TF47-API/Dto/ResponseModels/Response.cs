using System;
using System.Collections.Generic;
using TF47_API.Dto.Response;

namespace TF47_API.Dto.ResponseModels
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data)
        {
            Errors = null;
            Data = data;
        }
        public T Data { get; set; }
        public string[] Errors { get; set; }
    }
}