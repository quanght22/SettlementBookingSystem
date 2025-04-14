using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Exceptions;
using SettlementBookingSystem.Common.Enums;
using SettlementBookingSystem.Services.Classes;
using SettlementBookingSystem.Services.Interfaces;
using SettlementBookingSystem.Services.Settings;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SettlementBookingSystem.Application.UnitTests
{
    public class CreateBookingCommandHandlerTests
    {
        private IBookingService CreateBookingService(ILogger<BookingService> logger = null)
        {
            var settings = new BookingSettings
            {
                OpenTime = new TimeSpan(9, 0, 0),
                CloseTime = new TimeSpan(17, 0, 0),
                MaxBookingsPerSlot = 4
            };

            return new BookingService(
            Options.Create(settings),
                logger ?? new LoggerFactory().CreateLogger<BookingService>()
            );
        }

        [Fact]
        public async Task GivenValidBookingTime_WhenNoConflictingBookings_ThenBookingIsAccepted()
        {
            var command = new CreateBookingCommand
            {
                Name = "John",
                BookingTime = "09:15",
            };

            var bookingService = CreateBookingService();
            var handler = new CreateBookingCommandHandler(bookingService);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Booking.BookingId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GivenInvalidTimeFormat_WhenBooking_ThenReturnsInvalidTimeFormat()
        {
            var command = new CreateBookingCommand
            {
                Name = "Alex",
                BookingTime = "invalid",
            };

            var bookingService = CreateBookingService();
            var handler = new CreateBookingCommandHandler(bookingService);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Result.Should().Be(BookingResult.InvalidTimeFormat);
        }

        [Fact]
        public async Task GivenOutOfBusinessHours_WhenBooking_ThenReturnsOutOfBusinessHours()
        {
            var command = new CreateBookingCommand
            {
                Name = "Patrick",
                BookingTime = "16:15", 
            };

            var bookingService = CreateBookingService();
            var handler = new CreateBookingCommandHandler(bookingService);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Result.Should().Be(BookingResult.OutOfBusinessHours);
        }

        [Fact]
        public async Task GivenTooManyBookingsAtSameTime_WhenBooking_ThenReturnsMaxBookingsReached()
        {
            var bookingTime = "10:00";
            var bookingService = CreateBookingService();
            var handler = new CreateBookingCommandHandler(bookingService);

            // MaxBookingsPerSlot = 4
            for (int i = 0; i < 4; i++)
            {
                var command = new CreateBookingCommand
                {
                    Name = $"user{i}",
                    BookingTime = bookingTime
                };

                var result = await handler.Handle(command, CancellationToken.None);
                result.Result.Should().Be(BookingResult.Success);
            }

            // 5th booking at the same time should be rejected
            var finalCommand = new CreateBookingCommand
            {
                Name = "user5",
                BookingTime = bookingTime
            };

            var finalResult = await handler.Handle(finalCommand, CancellationToken.None);
            finalResult.Result.Should().Be(BookingResult.MaxBookingsReached);
        }

        [Fact]
        public void GivenEmptyName_WhenValidating_ThenShouldHaveValidationError()
        {
            var validator = new CreateBookingValidator();
            var command = new CreateBookingCommand
            {
                Name = "",
                BookingTime = "09:30"
            };

            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Name" && e.ErrorMessage == "Name must not be empty");
        }

        [Fact]
        public void GivenInvalidTimeFormat_WhenValidating_ThenReturnsValidationErrorForInvalidTimeFormat()
        {
            var validator = new CreateBookingValidator();
            var command = new CreateBookingCommand
            {
                Name = "Alex",
                BookingTime = "invalid", // Invalid time format
            };

            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "BookingTime" && e.ErrorMessage == "BookingTime must be in HH:mm format (24-hour)");
        }
    }
}
