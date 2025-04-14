using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SettlementBookingSystem.Common.BaseClasses;
using SettlementBookingSystem.Common.Dtos;
using SettlementBookingSystem.Common.Enums;
using SettlementBookingSystem.Common.Models;
using SettlementBookingSystem.Domain.Entities;
using SettlementBookingSystem.Services.Interfaces;
using SettlementBookingSystem.Services.Settings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SettlementBookingSystem.Services.Classes
{
    public class BookingService : BaseService, IBookingService
    {
        private readonly BookingSettings _bookingSettings;

        private readonly ConcurrentDictionary<TimeSpan, List<Booking>> _bookings = new ConcurrentDictionary<TimeSpan, List<Booking>>();

        public BookingService(
            IOptions<BookingSettings> bookingSettings,
            ILogger<BookingService> logger) : base(logger)
        {
            _bookingSettings = bookingSettings.Value;
        }

        public BookingAttemptResult AttemptBooking(string time, string name)
        {
            // Parse time
            if (!TimeSpan.TryParseExact(time, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan bookingTime))
            {
                Logger.LogWarning("Invalid time format: {Time}", time);
                return new BookingAttemptResult { Result = BookingResult.InvalidTimeFormat };
            }

            // Check business hours
            if (bookingTime < _bookingSettings.OpenTime || bookingTime > _bookingSettings.CloseTime - TimeSpan.FromHours(1))
            {
                Logger.LogWarning("Booking time {BookingTime} is out of business hours ({Open} - {Close})", bookingTime, _bookingSettings.OpenTime, _bookingSettings.CloseTime);
                return new BookingAttemptResult { Result = BookingResult.OutOfBusinessHours };
            }

            // Get or create slot
            var bookingsAtTime = _bookings.GetOrAdd(bookingTime, _ => new List<Booking>());

            lock (bookingsAtTime)
            {
                if (bookingsAtTime.Count >= _bookingSettings.MaxBookingsPerSlot)
                {
                    Logger.LogWarning("Max bookings reached at {BookingTime} ({Count}/{Max})", bookingTime, bookingsAtTime.Count, _bookingSettings.MaxBookingsPerSlot);
                    return new BookingAttemptResult { Result = BookingResult.MaxBookingsReached };
                }

                var booking = new BookingDto();

                bookingsAtTime.Add(new Booking
                {
                    Id = booking.BookingId,
                    Time = bookingTime,
                    Name = name
                });

                Logger.LogInformation("Booking successful: {Name} at {Time} (Total: {Count})", name, bookingTime, bookingsAtTime.Count);

                return new BookingAttemptResult
                {
                    Result = BookingResult.Success,
                    Booking = booking
                };
            }
        }
    }
}
