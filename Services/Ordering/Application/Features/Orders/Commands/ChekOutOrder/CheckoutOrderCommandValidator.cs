using FluentValidation;

namespace Application.Features.Orders.Commands.ChekOutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("{UserName} is required.")
                .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} is required.")
                .EmailAddress().WithMessage("A valid {EmailAddress} is required.");

            RuleFor(p => p.TotalPrice)
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
        }
    }
}
