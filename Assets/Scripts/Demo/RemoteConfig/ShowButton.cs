namespace Demo.RemoteConfig
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ShowButton : MonoBehaviour
    {

        public GameUIHandler uiHandler;

        public void OnClick()
        {
            Debug.Log("click");
            uiHandler.Show();
        }
        
    }

}
