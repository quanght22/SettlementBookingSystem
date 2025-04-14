using SettlementBookingSystem.Common.Enums;
using SettlementBookingSystem.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBookingSystem.Services.Interfaces
{
    public interface IBookingService
    {
        BookingAttemptResult AttemptBooking(string time, string name);
    }
}
