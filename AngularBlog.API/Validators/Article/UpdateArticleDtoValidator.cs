using BlazorBlog.API.Dtos;
using FluentValidation;

namespace BlazorBlog.API.Validators.Article
{
    public class UpdateArticleDtoValidator:AbstractValidator<UpdateArticleDto>
    {
        public UpdateArticleDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotNull()
                .WithMessage("{PropertyName} cannot be empty.");
            RuleFor(x => x.Content)
                .NotNull()
                .WithMessage("{PropertyName} cannot be empty.");
        }
    }
}
