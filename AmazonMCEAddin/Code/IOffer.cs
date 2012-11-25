using System;

namespace AmazonMCEAddin
{
    public enum OfferType
    {
        Subscription, 
        SeasonPurchase, 
        Purchase, 
        SeasonRental, 
        Rental
    }

    public interface IOffer
    {
        string Asin { get; }

        bool Buyable { get; }

        OfferType OfferType { get; }
    }
}
