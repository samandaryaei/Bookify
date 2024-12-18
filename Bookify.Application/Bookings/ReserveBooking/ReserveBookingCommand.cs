using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Apartments;

namespace Bookify.Application.Bookings.ReserveBooking;

public record ReserveBookingCommand(Guid apartmentId,Guid UserId,DateOnly StartDate,DateOnly EndDate) 
    : ICommand<Guid>;