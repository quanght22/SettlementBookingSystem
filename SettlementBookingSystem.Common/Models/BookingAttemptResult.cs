using SettlementBookingSystem.Common.Dtos;
using SettlementBookingSystem.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBookingSystem.Common.Models
{
    public class BookingAttemptResult
    {
        public BookingResult Result { get; set; }
        public BookingDto? Booking { get; set; }
    }
}
