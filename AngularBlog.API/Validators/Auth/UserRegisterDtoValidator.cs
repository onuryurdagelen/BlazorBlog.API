using BlazorBlog.API.Dtos;
using FluentValidation;

namespace BlazorBlog.API.Validators.Auth
{
    public class UserRegisterDtoValidator:AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty");

            RuleFor(x => x.EmailAddress)
                .EmailAddress()
                .WithMessage("Invalid {PropertyName}.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty");

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty");

            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.Password != x.PasswordConfirm)
                {
                    context.AddFailure(nameof(x.Password), "Passwords should match");
                }
            });
        }
    }
}
