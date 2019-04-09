namespace Demo.RemoteConfig
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LoadButton : MonoBehaviour
    {

        public GameUIHandler uiHandler;

        public void OnClick()
        {
            Debug.Log("click");
            uiHandler.Load();
        }
        
    }
}
