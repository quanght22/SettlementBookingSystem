using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBookingSystem.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }
        public TimeSpan Time { get; set; }
        public string Name { get; set; }
    }
}
