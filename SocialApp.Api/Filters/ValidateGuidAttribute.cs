﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialApp.Contracts.Common;

namespace SocialApp.Filters;

public class ValidateGuidAttribute : ActionFilterAttribute
{
    private readonly List<string> _keys;

    public ValidateGuidAttribute(params string[] keys)
    {
        _keys = keys.ToList();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var hasError = false;

        var apiError = new ErrorResponse();
        _keys.ForEach(k =>
        {
            if (!context.ActionArguments.TryGetValue(k, out var value) ||
                !Guid.TryParse(value?.ToString(), out _)) hasError = true;


            if (hasError)
            {
                apiError.Errors.Add($"The identifier for {k} is not a correct GUID format");
            }
        });

        if (!hasError) return;
        
        apiError.StatusCode = 400;
        apiError.StatusPhrase = "Bad Request";
        apiError.Timestamp = DateTime.Now;

        context.Result = new JsonResult(apiError){StatusCode = 400};
    }
}