using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

namespace game_ideas
{
    public class AdMobManager : MonoBehaviour
    {
        public static AdMobManager instance;

        private string app_ID = "ca-app-pub-4711099415082411~8073202650";

        //private string ads_banner_unitID_test = "ca-app-pub-3940256099942544/6300978111"; // banner test id
        private string ads_interstitial_unitID_test = "ca-app-pub-3940256099942544/1033173712";


        //private string ads_banner_unitID_release = "ca-app-pub-4711099415082411/2434874926"; // banner release id
        private string ads_interstitial_unitID_release = "ca-app-pub-4711099415082411/8239889468";

        private BannerView bannerView;
        private InterstitialAd interstitialAd;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(app_ID);
        }

        /* 
         * Request banner ads
         * 
        public void RequestBanner()
        {
            AdSize adSize = new AdSize(320, 50);
            // Instantianted banner view
            this.bannerView = new BannerView(ads_banner_unitID_test, adSize, AdPosition.BottomRight);

            // Called when an ad request has successfully loaded.
            this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
            // Called when an ad request failed to load.
            this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            this.bannerView.OnAdOpening += this.HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            this.bannerView.OnAdClosed += this.HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            this.bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

            // Create an empyty ad request
            AdRequest adRequest = new AdRequest.Builder().Build();

            // Load the banner with the request
            this.bannerView.LoadAd(adRequest);

            this.bannerView.Show();
        } */

        /*
         * Hide Banner ads
         * 
        public void HideBannerAds()
        {
            bannerView.Hide();
        } */

        public void RequestInterstitial()
        {
            // Instantiated interstitial
            this.interstitialAd = new InterstitialAd(ads_interstitial_unitID_test);

            // Called when an ad request has successfully loaded.
            this.interstitialAd.OnAdLoaded += this.HandleOnAdLoaded;
            // Called when an ad request failed to load.
            this.interstitialAd.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            this.interstitialAd.OnAdOpening += this.HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            this.interstitialAd.OnAdClosed += this.HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            this.interstitialAd.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

            // Create an empty ad request
            AdRequest requestAd = new AdRequest.Builder().Build();

            // Load the interstitial with the request
            this.interstitialAd.LoadAd(requestAd);
        }

        public void ShowInterstitialAd()
        {
            if (this.interstitialAd.IsLoaded())
            {
                this.interstitialAd.Show();
            }
        }

        public void DestroyInterstitialAds()
        {
            interstitialAd.Destroy();
        }

        public void HandleOnAdLoaded(object sender, EventArgs args)
        {   
            // do something here..
            // MonoBehaviour.print("HandleAdLoaded event received");
        }

        public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            // do something here..
            // MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                               // + args.Message);
        }

        public void HandleOnAdOpened(object sender, EventArgs args)
        {
            // do something here..
            // MonoBehaviour.print("HandleAdOpened event received");
        }

        public void HandleOnAdClosed(object sender, EventArgs args)
        {
            // do something here..
            // MonoBehaviour.print("HandleAdClosed event received");
        }

        public void HandleOnAdLeavingApplication(object sender, EventArgs args)
        {
            // do something here..
            // MonoBehaviour.print("HandleAdLeavingApplication event received");
        }
    }
}
