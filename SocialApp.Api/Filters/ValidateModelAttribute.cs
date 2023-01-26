using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialApp.Contracts.Common;

namespace SocialApp.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var apiError = new ErrorResponse
            {
                Timestamp = DateTime.Now,
                StatusPhrase = "Bad request",
                StatusCode = 400,
            };
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(v => v.ErrorMessage);

            foreach (var error in errors)
            {
                apiError.Errors.Add(error);
            }

            context.Result = new JsonResult(apiError) {StatusCode = 400};
            // TO DO: make sure asp.net does not override operation result body
        }
    }

}