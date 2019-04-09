
using System;
using System.Threading.Tasks;
using Demo.Reward;
using Firebase.RemoteConfig;

namespace Demo.RemoteConfig
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameUIHandler : MonoBehaviour
    {
        private RewardType _rewardType = RewardType.FLUCT;
        private bool _isFirebaseInitialized = false;
        private Firebase.DependencyStatus _dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
        
        // Start is called before the first frame update
        public void Start()
        {

            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                {
                    _dependencyStatus = task.Result;
                    if (_dependencyStatus == Firebase.DependencyStatus.Available)
                    {
                        initializeFirebase();
                    }
                    else
                    {
                        Debug.LogError("Could not resolve all firebase dependencys: " + _dependencyStatus);
                    }
                });

        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Load()
        {
            Debug.Log("load");
            if (!_isFirebaseInitialized)
            {
                return;
            }
            
            fetchRemoteConfigDataAsync();
        }

        public void Show()
        {
            switch (_rewardType)
            {
                case RewardType.FLUCT:
                    RewardFluct.Instance.Show();
                    break;
                case RewardType.ADMOB:
                    RewardAdMob.Instance.Show();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void initializeFirebase()
        {
            var configSettings = new ConfigSettings();
            configSettings.IsDeveloperMode = true;
            FirebaseRemoteConfig.Settings = configSettings;
            _isFirebaseInitialized = true;
        }

        private void fetchRemoteConfigDataAsync()
        {
            Task
                fetchTask = FirebaseRemoteConfig.FetchAsync(TimeSpan.Zero);
            fetchTask.ContinueWith(fetchComplete);
            return;
        }

        private void fetchComplete(Task fetchTask)
        {
            if (fetchTask.IsCanceled)
            {
                Debug.Log("fetch cancelled");
            } else if (fetchTask.IsFaulted)
            {
                Debug.Log("fetch encontered an error.");
            }
            else if (fetchTask.IsCompleted)
            {
                Debug.Log("Fetch completed successfully!");
            }

            var info = FirebaseRemoteConfig.Info;
            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    FirebaseRemoteConfig.ActivateFetched();
                    break;
                case LastFetchStatus.Failure:
                    break;
                case LastFetchStatus.Pending:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            setValues();
            loadReward();
        }

        private void setValues()
        {
            var rewardType = FirebaseRemoteConfig.GetValue("reward_type").StringValue;
            switch (rewardType)
            {
                case "fluct":
                    _rewardType = RewardType.FLUCT;
                    break;
                case "admob":
                    _rewardType = RewardType.ADMOB;
                    break;
                default:
                    _rewardType = RewardType.FLUCT;
                    break;
            }
            
            Debug.Log("remote config reward type:    " + rewardType);
        }

        private void loadReward()
        {
            switch (_rewardType)
            {
                case RewardType.FLUCT:
                    RewardFluct.Instance.OnRewardLoaded += rewardLoaded;
                    RewardFluct.Instance.OnRewardClosed += rewardClosed;
                    RewardFluct.Instance.Load();
                    break;
                case RewardType.ADMOB:
                    RewardAdMob.Instance.OnRewardLoaded += rewardLoaded;
                    RewardAdMob.Instance.OnRewardClosed += rewardClosed;
                    RewardAdMob.Instance.Load();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void rewardLoaded(object sender, EventArgs eventArgs)
        {
            Debug.Log("reward loaded.");
        }

        private void rewardClosed(object sender, bool isRewarded)
        {
            Debug.Log("reward closed.");
        }
    }
}
