using FluentValidation;


namespace Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("User name is required.")
                .MaximumLength(50).WithMessage("User name must not exceed 50 characters.");

            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("Email address is required.");

            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("Total price is required.")
                .GreaterThan(0).WithMessage("Total price should be greater than zero.");
        }
    }
}
