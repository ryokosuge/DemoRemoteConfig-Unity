
namespace Demo.Reward
{
    using System;
    using Fluct;
    public class RewardFluct
    {
        public static readonly RewardFluct Instance = new RewardFluct();

        public event EventHandler<EventArgs> OnRewardLoaded;
        public event EventHandler<bool> OnRewardClosed; 

        private readonly RewardedVideo _rewardedVideo;
        private bool _isRewarded = false;

        private RewardFluct()
        {
#if UNITY_ANDROID
            var groupID = "";
            var unitID = "";
#elif UNITY_IOS
            var groupID = "1000085420";
            var unitID = "1000127865";
#else
            var groupID = "";
            var unitID = "";
#endif
            _rewardedVideo = new RewardedVideo(groupID, unitID);
            _rewardedVideo.OnDidLoad += RewardedVideoLoaded;
            _rewardedVideo.OnShouldReward += RewardedVideoRewarded;
            _rewardedVideo.OnDidClose += RewardedVideoClosed;
        }

        public void Load()
        {
            _rewardedVideo.Load(new AdRequestTargeting());
        }

        public void Show()
        {
            if (_rewardedVideo.HasAdAvailable())
            {
                _rewardedVideo.Present();
            }
        }

        private void RewardedVideoLoaded(object sender, RewardedVideoEventArgs eventArgs)
        {
            OnRewardLoaded?.Invoke(this, eventArgs);
        }

        private void RewardedVideoRewarded(object sender, RewardedVideoEventArgs eventArgs)
        {
            _isRewarded = true;
        }

        private void RewardedVideoClosed(object sender, RewardedVideoEventArgs eventArgs)
        {
            OnRewardClosed?.Invoke(this, _isRewarded);
        }
    }
    
}