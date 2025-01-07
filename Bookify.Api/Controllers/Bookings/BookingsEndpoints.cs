using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.ReserveBooking;
using MediatR;

namespace Bookify.Api.Controllers.Bookings
{
   public static class BookingsEndpoints
    {
        public static IEndpointRouteBuilder MapBookingsEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("bookings/{id}",GetBookings)
                .RequireAuthorization()
                .WithName(nameof(GetBookings));

            builder.MapPost("bookings", ReserveBooking)
                .RequireAuthorization();
            
            return builder;
        }

        private static async Task<IResult> GetBookings(Guid id,ISender sender,CancellationToken cancellationToken)
        {
            var query = new GetBookingQuery(id);
            var result = await sender.Send(query,cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        }

        private static async Task<IResult> ReserveBooking(ReserveBookingRequest request,ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new ReserveBookingCommand
                (
                    request.ApartmentId,
                    request.UserId,
                    request.StartDate,
                    request.EndDate
                );
            
            var result = await sender.Send(command, cancellationToken);
            
            if(result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.CreatedAtRoute(nameof(GetBookings), new { id = result.Value }, result.Value);
        }
    }
}