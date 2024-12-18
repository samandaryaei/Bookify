using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Apartments.SearchApartments;

namespace Bookify.Application.Apartments;

public record SearchApartmentQuery(DateOnly StartDate, DateOnly EndDate)
    :IQuery<IReadOnlyList<ApartmentResponse>>;