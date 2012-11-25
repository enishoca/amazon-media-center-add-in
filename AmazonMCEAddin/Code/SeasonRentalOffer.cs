using System;
using Newtonsoft.Json.Linq;

namespace AmazonMCEAddin
{
    public class SeasonRentalOffer : SubscriptionOffer
    {
        public SeasonRentalOffer(JObject node)
            : base(node)
        {
        }
    }
}
