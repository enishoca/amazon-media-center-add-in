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

        public RentalOffer(JObject node, bool isHD = false)
            : base(node)
        {
            rentalExpiryTermFromPurchase = new RentalExpiryTerm();
            rentalExpiryTermFromPurchase.valueMillis = (long)node["rentalExpiryTermFromPurchase"]["valueMillis"];
            rentalExpiryTermFromPurchase.valueFormatted = (string)node["rentalExpiryTermFromPurchase"]["valueFormatted"];
            rentalExpiryTermFromStart = new RentalExpiryTerm();
            rentalExpiryTermFromStart.valueMillis = (long)node["rentalExpiryTermFromStart"]["valueMillis"];
            rentalExpiryTermFromStart.valueFormatted = (string)node["rentalExpiryTermFromStart"]["valueFormatted"];
            purchaseButtonText = (rentalExpiryTermFromStart.valueMillis / 3600000).ToString();
            purchaseButtonText += isHD ? " Hour HD Rental " : " Hour Rental ";
            purchaseButtonText += Price.valueFormatted;
        }

        public RentalExpiryTerm RentalExpiryTermFromPurchase { get { return rentalExpiryTermFromPurchase; } }

        public RentalExpiryTerm RentalExpiryTermFromStart { get { return rentalExpiryTermFromStart; } }
    }
}
