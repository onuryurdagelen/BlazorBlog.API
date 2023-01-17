using BlazorBlog.API.Dtos;
using FluentValidation;

namespace BlazorBlog.API.Validators.Email
{
    public class ContactEmailValidator:AbstractValidator<ContactEmailDto>
    {
        public ContactEmailValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .NotNull()
                    .WithMessage("FullName cannot be empty.")
                .MaximumLength(50)
                .MinimumLength(1)
                    .WithMessage("FullName must be greater than 0 character.");

            RuleFor(x => x.EmailAddress)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Email Address cannot be empty.")
                .MaximumLength(100)
                    .WithMessage("Email Addres must less than 100 characters")
                .EmailAddress()
                    .WithMessage("Enter a valid Email Address");

            RuleFor(x => x.SubjectTitle)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Subject Title cannot be empty.")
                .MaximumLength(100)
                    .WithMessage("Subject Title must less than 100 characters")
                .MinimumLength(1)
                    .WithMessage("Subject Title must greater than 0 characters");

            RuleFor(x => x.SubjectContent)
               .NotEmpty()
               .NotNull()
                   .WithMessage("Subject Title cannot be empty.")
               .MaximumLength(500)
                   .WithMessage("Subject Title must less than 500 characters")
               .MinimumLength(1)
                   .WithMessage("Subject Title must greater than 0 characters");
        }
    }
}
