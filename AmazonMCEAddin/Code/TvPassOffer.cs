using System;
using Newtonsoft.Json.Linq;

namespace AmazonMCEAddin
{
    public class TvPassOffer : SeasonPurchaseOffer
    {
        public TvPassOffer(JObject node)
            : base(node)
        {
        }
    }
}
