using Asp.Versioning;
using Bookify.Application.Apartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Apartments
{
    [Authorize]
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/apartments")]
    public class ApartmentsController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> SearchApartments(DateOnly startDate, DateOnly endDate,
            CancellationToken cancellationToken)
        {
            var query = new SearchApartmentQuery(startDate, endDate);
            var result = await sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }
    }
}