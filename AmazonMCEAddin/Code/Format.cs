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
        Stereo,
        AC3
    }

    public class Format
    {
        private VideoFormatType videoFormatType;
        private string coverArtSmallUri;
        private Image coverArtSmall;
        private string coverArtLargeUri;
        private Image coverArtLarge;
        private SubscriptionOffer subscriptionOffer = new SubscriptionOffer();
        private PurchaseOffer purchaseOffer = new PurchaseOffer();
        private RentalOffer rentalOffer = new RentalOffer();
        private SeasonPurchaseOffer seasonPurchaseOffer;
        private SeasonRentalOffer seasonRentalOffer;
        private TvPassOffer tvPassOffer;
        private float videoAspectRatio;
        private AudioFormatType audioFormatType;
        private bool hasEncode;
        private bool hasTrailerEncode;
        private bool hasMobileEncode;
        private bool hasMobileTrailerEncode;

        public Format()
        {
        }

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
                        purchaseOffer = new PurchaseOffer(offer, videoFormatType == VideoFormatType.HD);
                        break;
                    case "RENTAL":
                        rentalOffer = new RentalOffer(offer, videoFormatType == VideoFormatType.HD);
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
                    //case "STEREO":
                    //    audioFormatType = AudioFormatType.Stereo;
                    //    break;
                    case "AC_3_5_1":
                        audioFormatType = AudioFormatType.AC3;
                        break;
                }
            }
            hasEncode = node["hasEncode"] != null ? (bool)node["hasEncode"] : false;
            hasTrailerEncode = node["hasTrailerEncode"] != null ? (bool)node["hasTrailerEncode"] : false;
            hasMobileEncode = node["hasMobileEncode"] != null ? (bool)node["hasMobileEncode"] : false;
            hasMobileTrailerEncode = node["hasMobileTrailerEncode"] != null ? (bool)node["hasMobileTrailerEncode"] : false;
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

        public bool SubscriptionAvailable { get { return subscriptionOffer.Asin != null; } }

        public bool PurchaseAvailable { get { return !SubscriptionAvailable && purchaseOffer.Asin != null; } }

        public bool RentalAvailable { get { return !SubscriptionAvailable && rentalOffer.Asin != null; } }
    }
}
