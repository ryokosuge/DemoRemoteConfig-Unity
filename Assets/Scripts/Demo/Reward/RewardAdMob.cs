using UnityEngine.EventSystems;

namespace Demo.Reward
{
    using System;
    using GoogleMobileAds.Api;

    public sealed class RewardAdMob
    {
        public static readonly RewardAdMob Instance = new RewardAdMob();

        public event EventHandler<EventArgs> OnRewardLoaded;
        public event EventHandler<bool> OnRewardClosed; 

        private readonly RewardBasedVideoAd _rewardBasedVideoAd;
        private bool _isRewarded = false;

        private RewardAdMob()
        {
#if UNITY_ANDROID
            var appID = "ca-app-pub-3940256099942544~3347511713";
#elif UNITY_IOS
            var appID = "ca-app-pub-3940256099942544~1458002511";
#else
            var appID = "";
#endif
            MobileAds.Initialize(appID);
            _rewardBasedVideoAd = RewardBasedVideoAd.Instance;
            _rewardBasedVideoAd.OnAdLoaded += HandleRewardBaseVideoAdLoaded;
            _rewardBasedVideoAd.OnAdRewarded += HandleRewardBaseVideoAdRewarded;
            _rewardBasedVideoAd.OnAdClosed += HandleRewardBaseVideoAdClosed;
        }

        public void Load()
        {
#if UNITY_ANDROID
            var adUnitID = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IOS
            var adUnitID = "ca-app-pub-3940256099942544/1712485313";
#else
            var adUnitID = "";
#endif
            _isRewarded = false;
            var request = new AdRequest.Builder().Build();
            _rewardBasedVideoAd.LoadAd(request, adUnitID);
        }

        public void Show()
        {
            if (_rewardBasedVideoAd.IsLoaded())
            {
                _rewardBasedVideoAd.Show();
            }
        }

        private void HandleRewardBaseVideoAdLoaded(object sender, EventArgs eventArgs)
        {
            OnRewardLoaded?.Invoke(this, eventArgs);
        }

        private void HandleRewardBaseVideoAdRewarded(object sender, Reward reward)
        {
            _isRewarded = true;
        }

        private void HandleRewardBaseVideoAdClosed(object sender, EventArgs eventArgs)
        {
            OnRewardClosed?.Invoke(this, _isRewarded);
        }
    }

}