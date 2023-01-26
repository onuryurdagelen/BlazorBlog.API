using BlazorBlog.API.Dtos;
using FluentValidation;

namespace BlazorBlog.API.Validators.Auth
{
    public class UserLoginDtoValidator:AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty");

            RuleFor(x => x.EmailAddress)
               .EmailAddress()
               .WithMessage("{PropertyName} Email Address.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} cannot be empty");
        }
    }
}
