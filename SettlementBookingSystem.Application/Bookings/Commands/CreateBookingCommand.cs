using MediatR;
using SettlementBookingSystem.Common.Models;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingCommand : IRequest<BookingAttemptResult>
    {
        public string Name { get; set; }
        public string BookingTime { get; set; }
    }
}
