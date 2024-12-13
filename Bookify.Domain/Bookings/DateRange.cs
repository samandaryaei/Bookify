namespace Bookify.Domain.Bookings;

public record DateRange
{
    private DateRange()
    {
    }

    public DateOnly Start { get; init; }
    public DateOnly End { get; init; }
    public int LenghInDays => End.DayNumber - Start.DayNumber;

    public static DateRange Create(DateOnly start, DateOnly end)
    {
        if (end < start)
            throw new ApplicationException("End date precedes start date");

        return new DateRange
        {
            Start = start,
            End = end
        };
    }
}