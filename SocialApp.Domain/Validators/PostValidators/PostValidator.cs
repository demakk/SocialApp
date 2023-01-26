using System.Data;
using FluentValidation;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Domain.Validators.PostValidators;

public class PostValidator : AbstractValidator<Post>
{
    public PostValidator()
    {
        RuleFor(post => post.TextContent)
            .NotNull().WithMessage("A post cannot be null")
            .MinimumLength(1).WithMessage("Post's text content must have at least 1 character")
            .MaximumLength(300).WithMessage("A post's text content cannot contain more than 500 characters");
    }
}