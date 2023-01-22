using BlazorBlog.API.Dtos;
using FluentValidation;

namespace BlazorBlog.API.Validators.Article
{
    public class AddArticleDtoValidator: AbstractValidator<AddArticleDto>
    {
        public AddArticleDtoValidator()
        {
            //RuleFor(x => x.UploadedFile.)
        }
    }
}
