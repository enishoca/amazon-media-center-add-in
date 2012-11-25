using System;

namespace AmazonMCEAddin
{
    public enum OfferType
    {
        Subscription, 
        SeasonPurchase, 
        Purchase, 
        SeasonRental, 
        Rental,
        TvPass
    }

    public interface IOffer
    {
        string Asin { get; }

        bool Buyable { get; }

        OfferType OfferType { get; }
    }
}
