using System;
using Newtonsoft.Json.Linq;

namespace AmazonMCEAddin
{
    public class SeasonPurchaseOffer : PurchaseOffer
    {
        private Price lowestChildPrice;
        private Price highestChildPrice; 

        public SeasonPurchaseOffer(JObject node)
            : base(node)
        {
            if (node["lowestChildPrice"] != null)
            {
               lowestChildPrice = new Price();
                lowestChildPrice.valueLong = (float)node["lowestChildPrice"]["valueLong"];
                lowestChildPrice.valueFormatted = (string)node["lowestChildPrice"]["valueFormatted"];
            }
            if (node["highestChildPrice"] != null)
            {
                highestChildPrice = new Price();
                highestChildPrice.valueLong = (float)node["highestChildPrice"]["valueLong"];
                highestChildPrice.valueFormatted = (string)node["highestChildPrice"]["valueFormatted"];
            }
        }

        public Price LowestChildPrice { get { return lowestChildPrice; } }

        public Price HighestChildPrice { get { return highestChildPrice; } }
    }
}
