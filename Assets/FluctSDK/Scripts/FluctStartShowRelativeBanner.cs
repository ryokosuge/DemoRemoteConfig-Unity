using UnityEngine;
using System.Collections;

public class FluctStartShowRelativeBanner : FluctBanner
{

    void Start()
    {
        ShowRelative();
    }

    void OnDestroy()
    {
        Hide();
    }
}
