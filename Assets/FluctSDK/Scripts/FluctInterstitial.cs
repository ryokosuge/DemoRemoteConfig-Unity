using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class FluctInterstitial : MonoBehaviour
{
    public static int MaxInterstitialObjectId;
    [HideInInspector]
    public int InterstitialObjectId;

    public string MediaId = "";

    // HexColor
    public string HexColor = "";

#if UNITY_ANDROID && !UNITY_EDITOR
	static AndroidJavaObject AndroidPlugin = null;
#endif

    // FluctInterstitialView
#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	private static extern void FluctInterstitialViewCreate(int object_id, string medea_id);
	[DllImport ("__Internal")]
	private static extern void FluctInterstitialViewDestroy(int object_id);
	[DllImport ("__Internal")]
	private static extern void FluctInterstitialViewExist(int object_id);
	[DllImport ("__Internal")]
	private static extern void FluctInterstitialViewSetMediaID(int object_id, string media_id);
	[DllImport ("__Internal")]
	private static extern void FluctInterstitialViewShow(int object_id, string hex_color);
	[DllImport ("__Internal")]
	private static extern void FluctInterstitialViewDismiss(int object_id);
#elif UNITY_ANDROID && !UNITY_EDITOR
	private void FluctInterstitialViewCreate(string mediaId) {
		AndroidJavaObject activityContext;
		using (var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			activityContext = actClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		AndroidPlugin.Call("FluctInterstitialViewCreate", activityContext, mediaId);
	}

	private void FluctInterstitialViewShow() {
		AndroidPlugin.Call("FluctInterstitialViewShow");
	}
	private void FluctInterstitialViewDestroy() {
		AndroidPlugin.Call("FluctInterstitialViewDestroy");
	}
#else
    private void FluctInterstitialViewCreate(int object_id, string medea_id) { UnityEngine.Debug.Log("FluctInterstitialViewCreate()"); }
    private void FluctInterstitialViewDestroy(int object_id) { UnityEngine.Debug.Log("FluctInterstitialViewDestroy()"); }
    private void FluctInterstitialViewExist(int object_id) { UnityEngine.Debug.Log("FluctInterstitialViewExist()"); }
    private void FluctInterstitialViewSetMediaID(int object_id, string media_id) { UnityEngine.Debug.Log("FluctInterstitialViewSetMediaID()"); }
    private void FluctInterstitialViewShow(int object_id, string hex_color) { UnityEngine.Debug.Log("FluctInterstitialViewShow()"); }
    private void FluctInterstitialViewDismiss(int object_id) { UnityEngine.Debug.Log("FluctInterstitialViewDismiss()"); }
#endif

    public void Awake()
    {
        InterstitialObjectId = MaxInterstitialObjectId;
        MaxInterstitialObjectId++;
    }

    public void ShowInterstitial(string media_id = null, string hex_color = null)
    {

        string mid = string.IsNullOrEmpty(media_id) ? MediaId : media_id;
        string hc = string.IsNullOrEmpty(hex_color) ? HexColor : hex_color;

#if UNITY_IPHONE && !UNITY_EDITOR
		FluctInterstitialViewCreate(InterstitialObjectId,mid);
		FluctInterstitialViewExist(InterstitialObjectId);
		FluctInterstitialViewSetMediaID(InterstitialObjectId,mid);
		FluctInterstitialViewShow(InterstitialObjectId,hc);
#elif UNITY_ANDROID && !UNITY_EDITOR
		if (null == AndroidPlugin) {
			AndroidPlugin = new AndroidJavaObject( "com.voyagegroup.android.unity.plugins.FluctUnityPlugins");
		}
		FluctInterstitialViewCreate(mid);
		FluctInterstitialViewShow();
#else
        FluctInterstitialViewCreate(InterstitialObjectId, mid);
        FluctInterstitialViewExist(InterstitialObjectId);
        FluctInterstitialViewSetMediaID(InterstitialObjectId, mid);
        FluctInterstitialViewShow(InterstitialObjectId, hc);
#endif
    }
    public void HideInterstitial()
    {
#if UNITY_IPHONE && !UNITY_EDITOR
		FluctInterstitialViewExist(InterstitialObjectId);
		FluctInterstitialViewDismiss(InterstitialObjectId);
		FluctInterstitialViewDestroy(InterstitialObjectId);
#elif UNITY_ANDROID && !UNITY_EDITOR
		FluctInterstitialViewDestroy();
#else
        FluctInterstitialViewExist(InterstitialObjectId);
        FluctInterstitialViewDismiss(InterstitialObjectId);
        FluctInterstitialViewDestroy(InterstitialObjectId);
#endif
    }

}
