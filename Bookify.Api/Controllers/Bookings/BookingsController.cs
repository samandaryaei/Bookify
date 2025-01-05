using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.ReserveBooking;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Bookings
{
    [Authorize]
    [Route("api/bookings")]
    [ApiController]
    public class BookingsController(ISender sender) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookings(Guid id,CancellationToken cancellationToken)
        {
            var query = new GetBookingQuery(id);
            var result = await sender.Send(query,cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ReserveBooking(ReserveBookingRequest request,
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
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetBookings), new { id = result.Value }, result.Value);
        }
    }
}