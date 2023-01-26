using FluentValidation;
using Social.Domain.Aggregates.PostAggregates;

namespace Social.Domain.Validators.PostValidators;

public class PostCommentValidator : AbstractValidator<PostComment>
{
    public PostCommentValidator()
    {
        RuleFor(com => com.Text)
            .NotNull().WithMessage("Post's comment cannot be null")
            .NotEmpty().WithMessage("Post's comment cannot be empty")
            .MinimumLength(1).WithMessage("Post's comment text cannot have length less than 1")
            .MaximumLength(200).WithMessage("Post's comment text cannot have more than 200 characters");
        
    }
}