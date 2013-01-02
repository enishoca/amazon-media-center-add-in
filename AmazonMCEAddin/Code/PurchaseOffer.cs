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
        protected string purchaseButtonText;

        public PurchaseOffer()
        {
        }

        public PurchaseOffer(JObject node, bool isHD = false)
            : base(node)
        {
            if (node["price"] != null)
            {
                price = new Price();
                price.valueLong = (float)node["price"]["valueLong"];
                price.valueFormatted = (string)node["price"]["valueFormatted"];
            }
            //purchaseButtonText = (string)node["purchaseButtonText"];
            purchaseButtonText = isHD ? "Buy HD " : "Buy ";
            purchaseButtonText += Price.valueFormatted;
        }

        public Price Price { get { return price; } }

        public string PurchaseButtonText { get { return purchaseButtonText; } }
    }
}
