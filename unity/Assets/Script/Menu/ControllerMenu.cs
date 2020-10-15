using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerMenu : MonoBehaviour {
    SteamVR_TrackedObject obj;
    public GameObject buttonHolder;
    public bool buttonEnable;
    private void Start()
    {
        obj = GetComponent<SteamVR_TrackedObject>();
        buttonHolder.SetActive(false);
        buttonEnable = false;

    }
    private void Update()
    {
        var device = SteamVR_Controller.Input((int)obj.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            if(buttonEnable == false)
            {
                buttonHolder.SetActive(true);
                buttonEnable = true;
            }
            else if(buttonEnable == true)
            {
                buttonHolder.SetActive(false);
                buttonEnable = false;
            }
        }
    }

}
