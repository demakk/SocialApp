using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialApp.Contracts.Common;

namespace SocialApp.Filters;

public class ValidateGuidAttribute : ActionFilterAttribute
{
    private readonly string _key;

    public ValidateGuidAttribute(string key)
    {
        _key = key;
    }
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ActionArguments.TryGetValue(_key, out var value)) return;

        if (Guid.TryParse(value?.ToString(), out _)) return;
        
        var apiError = new ErrorResponse
        {
            Timestamp = DateTime.Now,
            StatusPhrase = "Bad request",
            StatusCode = 400,
        };
        
        apiError.Errors.Add($"The identifier for {_key} is not a correct GUID format");
        context.Result = new JsonResult(apiError) { StatusCode = 400 };
    }
    
    
}