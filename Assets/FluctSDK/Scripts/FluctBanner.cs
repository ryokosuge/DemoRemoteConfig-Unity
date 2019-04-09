using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[System.SerializableAttribute]
public class Margin
{
    public float left = 0;
    public float top = 0;
    public float right = 0;
    public float bottom = 0;
}

public enum BannerPositionY : int
{
    TOP = 2,
    BOTTOM = 8,
    CENTER = 16,
}

public enum BannerPositionX : int
{
    LEFT = 1,
    RIGHT = 4,
    CENTER = 32,
}

public class FluctBanner : MonoBehaviour
{

    public static int MaxObjectId;
    [HideInInspector]
    public int ObjectId;

    public string MediaId = "";

    public Rect Banner = new Rect(0.0f, 0.0f, 320.0f, 50.0f);

#if UNITY_ANDROID && !UNITY_EDITOR
	static AndroidJavaObject AndroidPlugin = null;
#endif

    public float Width = 320.0f;
    public float Height = 50.0f;
    [SerializeField]
    public BannerPositionY PositionY;
    public BannerPositionX PositionX;
    public Margin margin;

    // FluctBannerView
#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	private static extern void FluctBannerViewCreate(int object_id, string media_id);
	[DllImport ("__Internal")]
	private static extern void FluctBannerViewDestroy(int object_id);
	[DllImport ("__Internal")]
	private static extern void FluctBannerViewExist(int object_id);
	[DllImport ("__Internal")]
	private static extern void FluctBannerViewSetMediaID(int object_id, string media_id);
	[DllImport ("__Internal")]
	private static extern void FluctBannerViewSetFrame(int object_id, float x, float y, float w, float h);
	[DllImport ("__Internal")]
	private static extern void FluctBannerViewSetPosition(int object_id, float width, float height, int position, float left, float top, float right, float bottom);
	[DllImport ("__Internal")]
	private static extern void FluctBannerViewShow(int object_id);
	[DllImport ("__Internal")]
	private static extern void FluctBannerViewDismiss(int object_id);
#elif UNITY_ANDROID && !UNITY_EDITOR
	private void FluctBannerViewCreate(int object_id, string media_id) {
		AndroidJavaObject activityContext;
		using (var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			activityContext = actClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		AndroidPlugin.Call("FluctBannerViewCreate", activityContext, object_id, media_id);
	}
	private void FluctBannerViewDestroy(int object_id) {
		AndroidPlugin.Call("FluctBannerViewDestroy", object_id);
	}
	private void FluctBannerViewExist(int object_id) {
		AndroidPlugin.Call("FluctBannerViewDestroy", object_id);
	}
	private void FluctBannerViewSetMediaID(int object_id, string media_id) {
		AndroidPlugin.Call("FluctBannerViewSetMediaID", object_id, media_id);
	}
	private void FluctBannerViewSetFrame(int object_id, float x, float y, float width, float height) {
		AndroidPlugin.Call("FluctBannerViewSetFrame", object_id, x, y, width, height);
	}
	private void FluctBannerViewSetPosition(int object_id, float width, float height, int position, float left, float top, float right, float bottom) {
		AndroidPlugin.Call("FluctBannerViewSetPosition", object_id, width, height, position, left, top, right, bottom);
	}
	private void FluctBannerViewShow(int object_id) {
		AndroidPlugin.Call("FluctBannerViewShow", object_id);
	}
	private void FluctBannerViewDismiss(int object_id) {
		AndroidPlugin.Call("FluctBannerViewDismiss", object_id);
	}
#else
    private void FluctBannerViewCreate(int object_id, string media_id) { UnityEngine.Debug.Log("FluctBannerViewCreate()"); }
    private void FluctBannerViewDestroy(int object_id) { UnityEngine.Debug.Log("FluctBannerViewDestroy()"); }
    private void FluctBannerViewExist(int object_id) { UnityEngine.Debug.Log("FluctBannerViewExist()"); }
    private void FluctBannerViewSetMediaID(int object_id, string media_id) { UnityEngine.Debug.Log("FluctBannerViewSetMediaID()"); }
    private void FluctBannerViewSetFrame(int object_id, float x, float y, float w, float h) { UnityEngine.Debug.Log("FluctBannerViewSetFrame()"); }
    private void FluctBannerViewSetPosition(int object_id, float width, float height, int position, float left, float top, float right, float bottom) { UnityEngine.Debug.Log("FluctBannerViewSetPosition()"); }
    private void FluctBannerViewShow(int object_id) { UnityEngine.Debug.Log("FluctBannerViewShow()"); }
    private void FluctBannerViewDismiss(int object_id) { UnityEngine.Debug.Log("FluctBannerViewDismiss()"); }
#endif

    public virtual void Awake()
    {
        ObjectId = MaxObjectId;
        MaxObjectId++;
    }

    public void Show(Rect? bannerRect = null, string media_id = null)
    {
        Rect b = bannerRect ?? Banner;
        string mid = string.IsNullOrEmpty(media_id) ? MediaId : media_id;

        float x = b.x;
        float y = b.y;
        float w = b.width;
        float h = b.height;

#if UNITY_IPHONE && !UNITY_EDITOR
		FluctBannerViewCreate(ObjectId,mid);
		FluctBannerViewExist(ObjectId);
		FluctBannerViewSetMediaID(ObjectId,mid);
		FluctBannerViewSetFrame(ObjectId,x,y,w,h);
		FluctBannerViewShow(ObjectId);
#elif UNITY_ANDROID && !UNITY_EDITOR
		if (null == AndroidPlugin) {
			AndroidPlugin = new AndroidJavaObject( "com.voyagegroup.android.unity.plugins.FluctUnityPlugins");
		}
		FluctBannerViewCreate(ObjectId,mid);
		FluctBannerViewSetFrame(ObjectId,x,y,w,h);
		FluctBannerViewShow(ObjectId);
#else
        FluctBannerViewCreate(ObjectId, mid);
        FluctBannerViewExist(ObjectId);
        FluctBannerViewSetMediaID(ObjectId, mid);
        FluctBannerViewSetFrame(ObjectId, x, y, w, h);
        FluctBannerViewShow(ObjectId);
#endif
    }

    public void ShowRelative(float? w = null, float? h = null, string media_id = null)
    {
        string mid = string.IsNullOrEmpty(media_id) ? MediaId : media_id;

        float pos_w = w ?? Width;
        float pos_h = h ?? Height;

        margin = margin ?? new Margin();

#if UNITY_IPHONE && !UNITY_EDITOR
		FluctBannerViewCreate(ObjectId,mid);
		FluctBannerViewExist(ObjectId);
		FluctBannerViewSetMediaID(ObjectId,mid);
		FluctBannerViewSetPosition(ObjectId, pos_w, pos_h, GetBitPosition (PositionX, PositionY), margin.left, margin.top, margin.right, margin.bottom);
		FluctBannerViewShow(ObjectId);
#elif UNITY_ANDROID && !UNITY_EDITOR
		if (null == AndroidPlugin) {
			AndroidPlugin = new AndroidJavaObject( "com.voyagegroup.android.unity.plugins.FluctUnityPlugins");
		}
		FluctBannerViewCreate(ObjectId,mid);
		FluctBannerViewSetPosition(ObjectId, pos_w, pos_h, GetBitPosition (PositionX, PositionY), margin.left, margin.top, margin.right, margin.bottom);
		FluctBannerViewShow(ObjectId);
#else
        FluctBannerViewCreate(ObjectId, mid);
        FluctBannerViewExist(ObjectId);
        FluctBannerViewSetMediaID(ObjectId, mid);
        FluctBannerViewSetPosition(ObjectId, pos_w, pos_h, GetBitPosition(PositionX, PositionY), margin.left, margin.top, margin.right, margin.bottom);
        FluctBannerViewShow(ObjectId);
#endif
    }

    public void Hide()
    {
#if UNITY_IPHONE && !UNITY_EDITOR
		FluctBannerViewExist(ObjectId);
		FluctBannerViewDismiss(ObjectId);
		FluctBannerViewDestroy(ObjectId);
#elif UNITY_ANDROID && !UNITY_EDITOR
		if (null == AndroidPlugin) {
			AndroidPlugin = new AndroidJavaObject( "com.voyagegroup.android.unity.plugins.FluctUnityPlugins");
		}
		FluctBannerViewDestroy(ObjectId);
#else
        FluctBannerViewExist(ObjectId);
        FluctBannerViewDismiss(ObjectId);
        FluctBannerViewDestroy(ObjectId);
#endif
    }

    private int GetBitPosition(BannerPositionX PositionX, BannerPositionY PositionY)
    {
        return (int)PositionX + (int)PositionY;
    }
}
