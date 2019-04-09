using UnityEngine;
using System.Collections;

public class FluctStartShowInterstitial : FluctInterstitial
{

    void Start()
    {
        ShowInterstitial();
    }

    void OnDestroy()
    {
        HideInterstitial();
    }
}
