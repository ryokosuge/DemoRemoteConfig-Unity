using UnityEngine;
using System.Collections;

public class FluctStartShowBanner : FluctBanner
{

    void Start()
    {
        Show();
    }

    void OnDestroy()
    {
        Hide();
    }

}
