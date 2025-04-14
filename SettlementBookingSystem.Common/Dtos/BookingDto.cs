using System;
using System.Collections.Generic;
using System.Text;

namespace SettlementBookingSystem.Common.Dtos
{
    public class BookingDto
    {
        public BookingDto()
        {
            BookingId = Guid.NewGuid();
        }

        public Guid BookingId { get; }
    }
}
