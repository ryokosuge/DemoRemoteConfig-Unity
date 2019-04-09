using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Fluct
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public class RewardedVideo : AndroidJavaProxy
#else
    public class RewardedVideo
#endif
    {
        public event EventHandler<RewardedVideoEventArgs> OnDidLoad;
        public event EventHandler<RewardedVideoEventArgs> OnDidOpen;
        public event EventHandler<RewardedVideoEventArgs> OnDidClose;
        public event EventHandler<RewardedVideoEventArgs> OnShouldReward;
        public event EventHandler<RewardedVideoErrorEventArgs> OnDidFailToLoad;
        public event EventHandler<RewardedVideoErrorEventArgs> OnDidFailToPlay;

        public delegate void DidLoadHandler(string groupId, string unitId);
        public delegate void DidOpenHandler(string groupId, string unitId);
        public delegate void DidCloseHandler(string groupId, string unitId);
        public delegate void ShouldRewardHandler(string groupId, string unitId);
        public delegate void DidFailToLoadHandler(string groupId, string unitId, string errorMessage);
        public delegate void DidFailToPlayHandler(string groupId, string unitId, string errorMessage);

#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void _FluctPlugin_RewardedVideo_Load(string groupId, string unitId, string userId);
        [DllImport("__Internal")]
        private static extern void _FluctPlugin_RewardedVideo_Present(string groupId, string unitId);
        [DllImport("__Internal")]
        private static extern bool _FluctPlugin_RewardedVideo_HasAdAvailable(string groupId, string unitId);
        [DllImport("__Internal")]
        private static extern void _FluctPlugin_RewardedVideo_SetDidLoadHandler(DidLoadHandler handler);
        [DllImport("__Internal")]
        private static extern void _FluctPlugin_RewardedVideo_SetDidAppearHandler(DidOpenHandler handler);
        [DllImport("__Internal")]
        private static extern void _FluctPlugin_RewardedVideo_SetDidDisappearHandler(DidCloseHandler handler);
        [DllImport("__Internal")]
        private static extern void _FluctPlugin_RewardedVideo_SetShouldRewardHandler(ShouldRewardHandler handler);
        [DllImport("__Internal")]
        private static extern void _FluctPlugin_RewardedVideo_SetDidFailToLoadHandler(DidFailToLoadHandler handler);
        [DllImport("__Internal")]
        private static extern void _FluctPlugin_RewardedVideo_SetDidFailToPlayHandler(DidFailToPlayHandler handler);
        [DllImport("__Internal")]
        private static extern void _FluctPlugin_Configure(string unityVersion, string pluginVersion);
#elif UNITY_ANDROID && !UNITY_EDITOR
        private AndroidJavaObject androidJavaObject = null;

        private void _FluctPlugin_RewardedVideo_Load(string groupId, string unitId, string userId)
        {
            androidJavaObject.Call("loadAd", groupId, unitId, userId);
        }

        private void _FluctPlugin_RewardedVideo_Present(string groupId, string unitId)
        {

            androidJavaObject.Call("show", groupId, unitId);
        }

        private bool _FluctPlugin_RewardedVideo_HasAdAvailable(string groupId, string unitId)
        {
            return androidJavaObject.Call<bool>("isAdLoaded", groupId, unitId);
        }
#else
        private void _FluctPlugin_RewardedVideo_Load(string groupId, string unitId, string userId)
        {
        }

        private void _FluctPlugin_RewardedVideo_Present(string groupId, string unitId)
        {
        }

        private bool _FluctPlugin_RewardedVideo_HasAdAvailable(string groupId, string unitId)
        {
            return false;
        }
#endif

        private string groupId;
        private string unitId;

#if UNITY_ANDROID && !UNITY_EDITOR
        public RewardedVideo(string groupId, string unitId) : base("com.voyagegroup.android.unity.plugins.FluctRewardedVideoUnityListener")
#else
        public RewardedVideo(string groupId, string unitId)
#endif
        {
            this.groupId = groupId;
            this.unitId = unitId;

#if UNITY_IOS && !UNITY_EDITOR
            _FluctPlugin_Configure(UnityEngine.Application.unityVersion, FluctSDK.Version);
            _FluctPlugin_RewardedVideo_SetDidLoadHandler(OnDidLoadHandler);
            _FluctPlugin_RewardedVideo_SetDidAppearHandler(OnDidOpenHandler);
            _FluctPlugin_RewardedVideo_SetDidDisappearHandler(OnDidCloseHandler);
            _FluctPlugin_RewardedVideo_SetShouldRewardHandler(OnShouldRewardHandler);
            _FluctPlugin_RewardedVideo_SetDidFailToLoadHandler(OnDidFailToLoadHandler);
            _FluctPlugin_RewardedVideo_SetDidFailToPlayHandler(OnDidFailToPlayHandler);
#elif UNITY_ANDROID && !UNITY_EDITOR
            androidJavaObject = new AndroidJavaObject("com.voyagegroup.android.unity.plugins.FluctRewardedVideoUnityPlugin", groupId, unitId, UnityEngine.Application.unityVersion, FluctSDK.Version, this);
#endif
        }

        private static void Dispose(string groupId, string unitId)
        {
            RewardedVideoInstanceManager.SafeRemoveObject(groupId + unitId);
        }

        /// <summary>
        /// 広告読み込み
        /// </summary>
        public void Load(AdRequestTargeting targeting)
        {
            RewardedVideoInstanceManager.SafeSetObject(groupId + unitId, this);
            _FluctPlugin_RewardedVideo_Load(groupId, unitId, targeting.UserId);
        }

        /// <summary>
        /// 広告再生
        /// </summary>
        public void Present()
        {
            _FluctPlugin_RewardedVideo_Present(groupId, unitId);
        }

        /// <summary>
        /// 広告再生可能な場合 true
        /// 広告再生不可能な場合 false
        /// </summary>
        public bool HasAdAvailable()
        {
            return _FluctPlugin_RewardedVideo_HasAdAvailable(groupId, unitId);
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        void OnDidLoadHandler(string groupId, string unitId)
#else
        [AOT.MonoPInvokeCallback(typeof(DidLoadHandler))]
        static void OnDidLoadHandler(string groupId, string unitId)

#endif
        {
            RewardedVideo value;
            if (!RewardedVideoInstanceManager.SafeGetObject(groupId + unitId, out value))
            {
                return;
            }
            EventHandler<RewardedVideoEventArgs> handler = value.OnDidLoad;
            if (handler == null)
            {
                return;
            }
            handler(value, new RewardedVideoEventArgs(groupId, unitId));
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        void OnDidOpenHandler(string groupId, string unitId)
#else
        [AOT.MonoPInvokeCallback(typeof(DidOpenHandler))]
        static void OnDidOpenHandler(string groupId, string unitId)
#endif
        {
            RewardedVideo value;
            if (!RewardedVideoInstanceManager.SafeGetObject(groupId + unitId, out value))
            {
                return;
            }
            EventHandler<RewardedVideoEventArgs> handler = value.OnDidOpen;
            if (handler == null)
            {
                return;
            }
            handler(value, new RewardedVideoEventArgs(groupId, unitId));
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        void OnDidCloseHandler(string groupId, string unitId)
#else
        [AOT.MonoPInvokeCallback(typeof(DidCloseHandler))]
        static void OnDidCloseHandler(string groupId, string unitId)
#endif
        {
            RewardedVideo value;
            if (!RewardedVideoInstanceManager.SafeGetObject(groupId + unitId, out value))
            {
                return;
            }
            EventHandler<RewardedVideoEventArgs> handler = value.OnDidClose;
            if (handler == null)
            {
                return;
            }
            RewardedVideo.Dispose(groupId, unitId);
            handler(value, new RewardedVideoEventArgs(groupId, unitId));
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        void OnShouldRewardHandler(string groupId, string unitId)
#else
        [AOT.MonoPInvokeCallback(typeof(ShouldRewardHandler))]
        static void OnShouldRewardHandler(string groupId, string unitId)
#endif
        {
            RewardedVideo value;
            if (!RewardedVideoInstanceManager.SafeGetObject(groupId + unitId, out value))
            {
                return;
            }
            EventHandler<RewardedVideoEventArgs> handler = value.OnShouldReward;
            if (handler == null)
            {
                return;
            }
            handler(value, new RewardedVideoEventArgs(groupId, unitId));
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        void OnDidFailToLoadHandler(string groupId, string unitId, string errorMessage)
#else
        [AOT.MonoPInvokeCallback(typeof(DidFailToLoadHandler))]
        static void OnDidFailToLoadHandler(string groupId, string unitId, string errorMessage)
#endif
        {
            RewardedVideo value;
            if (!RewardedVideoInstanceManager.SafeGetObject(groupId + unitId, out value))
            {
                return;
            }
            EventHandler<RewardedVideoErrorEventArgs> handler = value.OnDidFailToLoad;
            if (handler == null)
            {
                return;
            }
            handler(value,
                new RewardedVideoErrorEventArgs(
                    groupId,
                    unitId,
                    errorMessage
                )
            );
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        void OnDidFailToPlayHandler(string groupId, string unitId, string errorMessage)
#else
        [AOT.MonoPInvokeCallback(typeof(DidFailToPlayHandler))]
        static void OnDidFailToPlayHandler(string groupId, string unitId, string errorMessage)
#endif
        {
            RewardedVideo value;
            if (!RewardedVideoInstanceManager.SafeGetObject(groupId + unitId, out value))
            {
                return;
            }
            EventHandler<RewardedVideoErrorEventArgs> handler = value.OnDidFailToPlay;
            if (handler == null)
            {
                return;
            }
            handler(value,
                new RewardedVideoErrorEventArgs(
                    groupId,
                    unitId,
                    errorMessage
                )
            );
        }
    }

    public class RewardedVideoEventArgs : EventArgs
    {
        public string GroupId { get; private set; }

        public string UnitId { get; private set; }

        public RewardedVideoEventArgs(string groupId, string unitId)
        {
            GroupId = groupId;
            UnitId = unitId;
        }
    }

    public class RewardedVideoErrorEventArgs : RewardedVideoEventArgs
    {
        public string ErrorCode { get; private set; }

        public RewardedVideoErrorEventArgs(string groupId, string unitId, string errorCode)
            : base(groupId, unitId)
        {
            ErrorCode = errorCode;
        }
    }


    public class RewardedVideoInstanceManager
    {

        private static Dictionary<string, RewardedVideo> instances = new Dictionary<string, RewardedVideo>();

        public static bool SafeGetObject(string key, out RewardedVideo rv)
        {
            lock (instances)
            {
                return instances.TryGetValue(key, out rv);
            }
        }

        public static void SafeSetObject(string key, RewardedVideo rv)
        {
            lock (instances)
            {
                instances[key] = rv;
            }
        }

        public static void SafeRemoveObject(string key)
        {
            lock (instances)
            {
                instances.Remove(key);
            }
        }
    }
}