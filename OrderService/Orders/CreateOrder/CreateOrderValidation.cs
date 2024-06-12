using FluentValidation;

namespace OrderService.Orders.CreateOrder
{
    /// <summary>
    /// Credit cad validation rule
    /// </summary>
    public class CreateOrderValidation : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidation()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("It is not valid email address");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Cant be empty");

            RuleFor(x => x.Items).NotEmpty().WithMessage("Cant be ampty");
        }
    }
}
