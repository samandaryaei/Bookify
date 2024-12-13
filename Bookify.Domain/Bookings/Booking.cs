using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings;

public sealed class Booking : Entity
{
    public Guid Id { get; }

    public Booking(
        Guid id,
        Guid apartmentId,
        Guid userId,
        DateRange duration,
        Money priceForPeriod,
        Money cleaningFee,
        Money amentitiesUpCharge,
        Money totalPrice,
        BookingStatus status,
        DateTime createdOnUtc)
        : base(id)
    {
        ApartmentId = apartmentId;
        UserId = userId;
        Duration = duration;
        PriceForPeriod = priceForPeriod;
        CleaningFee = cleaningFee;
        AmentitiesUpCharge = amentitiesUpCharge;
        TotalPrice = totalPrice;
        Status = status;
        CreatedOnUtc = createdOnUtc;
    }

    public Guid ApartmentId { get; private set; }
    public Guid UserId { get; private set; }
    public DateRange Duration { get; private set; }
    public Money PriceForPeriod { get; private set; }
    public Money CleaningFee { get; private set; }
    public Money AmentitiesUpCharge { get; private set; }
    public Money TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ConfirmedOnUtc { get; private set; }
    public DateTime RejectedOnUtc { get; private set; }
    public DateTime CompletedOnUtc { get; private set; }
    public DateTime CanceledOnUtc { get; private set; }

    public static Booking Reserve(Apartment apartment, Guid userId, DateRange duration, DateTime utcNow,
        PricingService pricingService)
    {
        var pricingDetails = pricingService.CalculatePrice(apartment, duration);
        
        var booking = new Booking(Guid.NewGuid(), apartment.Id, userId, duration, pricingDetails.PriceForPeriod,
            pricingDetails.CleaningFee, pricingDetails.AmenitiesUpCharge, pricingDetails.TotalPrice,
            BookingStatus.Reserved, utcNow);
        
        booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.Id));

        apartment.LastBookedOnUtc = utcNow;

        return booking;
    }

    public Result Confirm(DateTime utcTime)
    {
        if (Status != BookingStatus.Reserved)
            return Result.Failure(BookingErrors.NotReserved);
        
        Status = BookingStatus.Confirmed;
        ConfirmedOnUtc = utcTime;
        
        RaiseDomainEvent(new BookingConfirmedDomainEvent(Id));

        return Result.Success();
    }

    public Result Reject(DateTime utcTime)
    {
        if(Status != BookingStatus.Reserved)
            return Result.Failure(BookingErrors.NotReserved);
        
        Status = BookingStatus.Rejected;
        RejectedOnUtc = utcTime;
        
        RaiseDomainEvent(new BookingRejectedDomainEvent(Id));
        
        return Result.Success();
    }
    
    public Result Complete(DateTime utcTime)
    {
        if(Status != BookingStatus.Confirmed)
            return Result.Failure(BookingErrors.NotConfirmed);
        
        Status = BookingStatus.Completed;
        CompletedOnUtc = utcTime;
        
        RaiseDomainEvent(new BookingCompletedDomainEvent(Id));
        
        return Result.Success();
    }
    
    public Result Cancel(DateTime utcTime)
    {
        if(Status != BookingStatus.Confirmed)
            return Result.Failure(BookingErrors.NotConfirmed);

        var currentDate = DateOnly.FromDateTime(utcTime);
        if (currentDate > Duration.Start)
            return Result.Failure(BookingErrors.AlreadyStarted);
        
        Status = BookingStatus.Cancelled;
        CompletedOnUtc = utcTime;
        
        RaiseDomainEvent(new BookingCancelledDomainEvent(Id));
        
        return Result.Success();
    }
}