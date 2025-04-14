using FluentValidation;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingValidator()
        {
            RuleFor(b => b.Name).NotEmpty().WithMessage("Name must not be empty");
            RuleFor(b => b.BookingTime).NotEmpty().WithMessage("BookingTime is required")
                .Matches("[0-9]{1,2}:[0-9][0-9]").WithMessage("BookingTime must be in HH:mm format (24-hour)"); 
        }
    }
}
