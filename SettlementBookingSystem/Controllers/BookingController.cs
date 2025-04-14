using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Common.Dtos;
using SettlementBookingSystem.Common.Enums;
using System;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingCommand command)
        {
            var result =  await _mediator.Send(command);
            return result.Result switch
            {
                BookingResult.Success => Ok(result.Booking),
                BookingResult.InvalidName => BadRequest("Invalid name."),
                BookingResult.InvalidTimeFormat => BadRequest("Invalid time format. Please use HH:mm."),
                BookingResult.OutOfBusinessHours => BadRequest("Booking time is out of business hours."),
                BookingResult.MaxBookingsReached => Conflict("Maximum bookings reached for this time slot."),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error.")
            };
        }
    }
}
