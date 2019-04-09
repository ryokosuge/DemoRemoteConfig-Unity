using UnityEngine;
using System.Collections;
using Fluct;

public class BannerSceneController : MonoBehaviour
{
    private FluctBanner banner;

    // Use this for initialization
    void Start()
    {
        banner = new FluctBanner();
        banner.Show(new Rect(0.0f, 0.0f, 320.0f, 50.0f), "0000000108");

    }

    private void OnDestroy()
    {
        banner.Hide();
    }
}
