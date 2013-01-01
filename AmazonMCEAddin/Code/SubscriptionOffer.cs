using System;
using Newtonsoft.Json.Linq;

namespace AmazonMCEAddin
{
    public class SubscriptionOffer : IOffer
    {
        private string asin;
        private bool buyable;
        private OfferType offerType;

        public SubscriptionOffer()
        {
        }

        public SubscriptionOffer(JObject node)
        {
            asin = (string)node["asin"];
            buyable = (bool)node["buyable"];

            switch ((string)node["offerType"])
            {
                case "SUBSCRIPTION":
                    offerType = OfferType.Subscription;
                    break;
                case "PURCHASE":
                    offerType = OfferType.Purchase;
                    break;
                case "RENTAL":
                    offerType = OfferType.Rental;
                    break;
                case "SEASON_PURCHASE":
                    offerType = OfferType.SeasonPurchase;
                    break;
                case "SEASON_RENTAL":
                    offerType = OfferType.SeasonRental;
                    break;
                case "TV_PASS":
                    offerType = OfferType.TvPass;
                    break;
            }
        }

        public string Asin { get { return asin; } }

        public bool Buyable { get { return buyable; } }

        public OfferType OfferType { get { return offerType; } }
    }
}
