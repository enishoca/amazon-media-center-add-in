using System;
using Microsoft.MediaCenter.UI;
using Newtonsoft.Json.Linq;

namespace AmazonMCEAddin
{
    public enum VideoFormatType
    {
        SD,
        HD
    }

    public enum AudioFormatType
    {
        Stereo
    }

    public class Format
    {
        private VideoFormatType videoFormatType;
        private string coverArtSmallUri;
        private Image coverArtSmall;
        private string coverArtLargeUri;
        private Image coverArtLarge;
        private SubscriptionOffer subscriptionOffer;
        private PurchaseOffer purchaseOffer;
        private RentalOffer rentalOffer;
        private SeasonPurchaseOffer seasonPurchaseOffer;
        private SeasonRentalOffer seasonRentalOffer;
        private TvPassOffer tvPassOffer;
        private float videoAspectRatio;
        private AudioFormatType audioFormatType;
        private bool hasEncode;
        private bool hasTrailerEncode;
        private bool hasMobileEncode;
        private bool hasMobileTrailerEncode;

        public Format(JObject node)
        {
            switch ((string)node["videoFormatType"])
            {
                case "SD":
                    videoFormatType = VideoFormatType.SD;
                    break;
                case "HD":
                    videoFormatType = VideoFormatType.HD;
                    break;
            }
            coverArtSmallUri = (string)node["images"][0]["uri"];
            coverArtLargeUri = (string)node["images"][1]["uri"];

            foreach (JObject offer in node["offers"])
            {
                switch ((string)offer["offerType"])
                {
                    case "SUBSCRIPTION":
                        subscriptionOffer = new SubscriptionOffer(offer);
                        break;
                    case "PURCHASE":
                        purchaseOffer = new PurchaseOffer(offer);
                        break;
                    case "RENTAL":
                        rentalOffer = new RentalOffer(offer);
                        break;
                    case "SEASON_PURCHASE":
                        seasonPurchaseOffer = new SeasonPurchaseOffer(offer);
                        break;
                    case "SEASON_RENTAL":
                        seasonRentalOffer = new SeasonRentalOffer(offer);
                        break;
                    case "TV_PASS":
                        tvPassOffer = new TvPassOffer(offer);
                        break;
                }
            }
            videoAspectRatio = (float)node["videoAspectRatio"];
            foreach (string audioFormat in node["audioFormatTypes"])
            {
                switch (audioFormat)
                {
                    case "STEREO":
                        audioFormatType = AudioFormatType.Stereo;
                        break;
                }
            }
            hasEncode = (bool)node["hasEncode"];
            hasTrailerEncode = (bool)node["hasTrailerEncode"];
            hasMobileEncode = (bool)node["hasMobileEncode"];
            hasMobileTrailerEncode = (bool)node["hasMobileTrailerEncode"];
        }

        public VideoFormatType VideoFormatType { get { return videoFormatType; } }

        public Image CoverArtSmall 
        { 
            get 
            { 
                if (coverArtSmall == null)
                    coverArtSmall = new Image(coverArtSmallUri);
                return coverArtSmall;    
            } 
        }

        public Image CoverArtLarge
        {
            get
            {
                if (coverArtLarge == null)
                    coverArtLarge = new Image(coverArtLargeUri);
                return coverArtLarge;
            }
        }

        //temp workaround for code merge - this is needed to allow delayed web request for image from virtual list
        public string CoverArtLargeUri
        {
            get
            {
                return coverArtLargeUri;
            }
        }

        public SubscriptionOffer SubscriptionOffer { get { return subscriptionOffer; } }

        public PurchaseOffer PurchaseOffer { get { return purchaseOffer; } }

        public RentalOffer RentalOffer { get { return rentalOffer; } }

        public SeasonPurchaseOffer SeasonPurchaseOffer { get { return seasonPurchaseOffer; } }

        public SeasonRentalOffer SeasonRentalOffer { get { return seasonRentalOffer; } }

        public TvPassOffer TvPassOffer { get { return tvPassOffer; } }

        public float VideoAspectRatio { get { return videoAspectRatio; } }

        public AudioFormatType AudioFormatType { get { return audioFormatType; } }

        public bool HasEncode { get { return hasEncode; } }

        public bool HasTrailerEncode { get { return hasTrailerEncode; } }

        public bool HasMobileEncode { get { return hasMobileEncode; } }

        public bool HasMobileTrailerEncode { get { return hasMobileTrailerEncode; } }
    }
}
