using System;
using Newtonsoft.Json.Linq;

namespace AmazonMCEAddin
{
    public struct RentalExpiryTerm
    {
        public long valueMillis;
        public string valueFormatted;
    }

    public class RentalOffer : PurchaseOffer
    {
        private RentalExpiryTerm rentalExpiryTermFromPurchase;
        private RentalExpiryTerm rentalExpiryTermFromStart;

        public RentalOffer()
        {
        }

        public RentalOffer(JObject node)
            : base(node)
        {
            rentalExpiryTermFromPurchase = new RentalExpiryTerm();
            rentalExpiryTermFromPurchase.valueMillis = (long)node["rentalExpiryTermFromPurchase"]["valueMillis"];
            rentalExpiryTermFromPurchase.valueFormatted = (string)node["rentalExpiryTermFromPurchase"]["valueFormatted"];
            rentalExpiryTermFromStart = new RentalExpiryTerm();
            rentalExpiryTermFromStart.valueMillis = (long)node["rentalExpiryTermFromPurchase"]["valueMillis"];
            rentalExpiryTermFromStart.valueFormatted = (string)node["rentalExpiryTermFromPurchase"]["valueFormatted"];
        }

        public RentalExpiryTerm RentalExpiryTermFromPurchase { get { return rentalExpiryTermFromPurchase; } }

        public RentalExpiryTerm RentalExpiryTermFromStart { get { return rentalExpiryTermFromStart; } }
    }
}
