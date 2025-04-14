using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBookingSystem.Common.Enums
{
    public enum BookingResult
    {
        Success,
        InvalidTimeFormat,
        OutOfBusinessHours,
        MaxBookingsReached,
        InvalidName
    }
}
