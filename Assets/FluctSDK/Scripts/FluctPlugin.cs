/**
 * FluctUnityPlugin V1.1.1
 */

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class FluctPlugin : MonoBehaviour
{

#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	private static extern void FluctSDKSetBannerConfiguration(string mediaId);
#else
    private static void FluctSDKSetBannerConfiguration(string mediaId) { UnityEngine.Debug.Log("FluctSDKSetBannerConfiguration()"); }
#endif

    // メディアIDの設定
    public static void SetMediaId(string mediaId)
    {
#if UNITY_IPHONE && !UNITY_EDITOR
		FluctSDKSetBannerConfiguration(mediaId);
#else
        FluctSDKSetBannerConfiguration(mediaId);
#endif
    }

}
