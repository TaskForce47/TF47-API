using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace TF47_Api.CustomStatusCodes
{
    public class ServerError : IActionResult
    {
        private string _errorMessage;

        public ServerError(string errorMessage = "")
        {
            _errorMessage = errorMessage;
        }


        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(new
            {
                ErrorMessage = _errorMessage
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            await objectResult.ExecuteResultAsync(context);
        }
    }
}
