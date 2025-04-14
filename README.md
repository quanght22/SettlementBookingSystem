# Settlement Booking API
This API enables one-hour appointment bookings for InfoTrack's settlement service, which operates with a fixed capacity of up to 4 simultaneous bookings per time slot.
## Requirements
- .NET 5.0 SDK or later
- Visual Studio 2022 or Visual Studio Code

## Implementation

1. Mediator Pattern

The BookingController uses the Mediator pattern, and booking data is stored in a ConcurrentDictionary<TimeSpan, List<Booking>> for efficient concurrent access.
This implementation also includes unit tests for key logic components.

## How to Run

dotnet build
dotnet run --project SettlementBookingSystem

## How to execute(Sample Request)

```
curl -X POST "https://localhost:5001/Booking" -H  "accept: text/plain" -H  "Content-Type: application/json" -d "{\"name\":\"Jonh\",\"bookingTime\":\"09:30\"}"

```
