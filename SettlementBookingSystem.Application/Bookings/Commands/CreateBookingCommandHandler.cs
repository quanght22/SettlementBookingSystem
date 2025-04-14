using MediatR;
using SettlementBookingSystem.Common.Enums;
using SettlementBookingSystem.Common.Models;
using SettlementBookingSystem.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingAttemptResult>
    {
        private readonly IBookingService _bookingService;
        public CreateBookingCommandHandler(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public Task<BookingAttemptResult> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var result = _bookingService.AttemptBooking(request.BookingTime, request.Name);

            return Task.FromResult(result);
        }
    }
}
