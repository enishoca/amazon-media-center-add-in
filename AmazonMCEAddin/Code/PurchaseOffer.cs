using System;
using Newtonsoft.Json.Linq;

namespace AmazonMCEAddin
{
    public struct Price
    {
        public float valueLong;
        public string valueFormatted;
    }

    public class PurchaseOffer : SubscriptionOffer
    {
        private Price price;
        private string purchaseButtonText;

        public PurchaseOffer()
        {
        }

        public PurchaseOffer(JObject node)
            : base(node)
        {
            if (node["price"] != null)
            {
                price = new Price();
                price.valueLong = (float)node["price"]["valueLong"];
                price.valueFormatted = (string)node["price"]["valueFormatted"];
            }
            purchaseButtonText = (string)node["purchaseButtonText"];
        }

        public Price Price { get { return price; } }

        public string PurchaseButtonText { get { return purchaseButtonText; } }
    }
}
