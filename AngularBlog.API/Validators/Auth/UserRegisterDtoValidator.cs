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
                .WithMessage("Email Address cannot be empty");

            RuleFor(x => x.EmailAddress)
                .EmailAddress()
                .WithMessage("Invalid Email Address.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty");

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full Name cannot be empty");

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
