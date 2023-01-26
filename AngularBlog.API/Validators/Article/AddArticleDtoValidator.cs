using BlazorBlog.API.Dtos;
using FluentValidation;

namespace BlazorBlog.API.Validators.Article
{
    public class AddArticleDtoValidator: AbstractValidator<AddArticleDto>
    {
        public AddArticleDtoValidator()
        {
            RuleFor(x => x.UploadedFile.FileExtension)
                .NotNull()
                .WithMessage("{PropertyName} cannot be empty.");
 

            RuleFor(x => x.UploadedFile.FileName)
                .NotNull()
                .WithMessage("{PropertyName} cannot be empty.");

            RuleFor(x => x.UploadedFile.FileContent)
                .NotNull()
                .WithMessage("{PropertyName} cannot be empty.");

            //RuleFor(x => x).Custom((x, context) =>
            //{
            //    if(x.UploadedFile != null)
            //    {
            //        if (!x.UploadedFile.FileExtension.Contains(".jpg") &&
            //        !x.UploadedFile.FileExtension.Contains(".jpeg") &&
            //        !x.UploadedFile.FileExtension.Contains(".png"))
            //        {
            //            context.AddFailure(nameof(x.UploadedFile.FileExtension), "Invalid File Extension.Please choose .jpg , .jpeg or .png file.");
            //        }
            //    }
                
            //});

            //RuleFor(x => x).Custom((x, context) =>
            //{
            //   if(x.UploadedFile != null)
            //    {
            //        if (x.UploadedFile.FileSize > 2)
            //        {
            //            context.AddFailure(nameof(x.UploadedFile.FileSize), "File Size cannot be greater than 2MB or more.");
            //        }
            //    }
            //});

        }
        
    }
}
