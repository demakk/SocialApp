using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialApp.Contracts.Common;

namespace SocialApp.Filters;

public class SocialAppExceptionHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var apiError = new ErrorResponse
        {
            Timestamp = DateTime.Now,
            StatusPhrase = "Internal server error",
            StatusCode = 500,
        };
        
        apiError.Errors.Add(context.Exception.Message);

        context.Result = new JsonResult(apiError) {StatusCode = 500};
    }
}