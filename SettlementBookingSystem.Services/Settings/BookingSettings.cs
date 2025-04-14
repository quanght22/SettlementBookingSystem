using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBookingSystem.Services.Settings
{
    public class BookingSettings
    {
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public int MaxBookingsPerSlot { get; set; }
    }
}
