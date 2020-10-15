using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JVRIzquierdo : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    public DronMovement JOYIZQuierdo;
    public PinzaArm Brazo;
    public PinzaArm lfbrsv;
    public PinzaArm lsfnefgkj;
    public PinzaArm laernvk;

    public PinzaArm2 bBrazo;
    public PinzaArm2 blfbrsv;
    public PinzaArm2 blsfnefgkj;
    public PinzaArm2 blaernvk;

    public GiroBrazoArm GPAI; //brazo
    public EchoTest JAC;
    public EchoTest JBA;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();

    }

    void FixedUpdate()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            GPAI.BRAZODOWN();
            JBA.GiroDown();
}

        if (Controller.GetHairTrigger())
        {
            Brazo.JOYSTICKPINZAUP();
            lfbrsv.JOYSTICKPINZAUP();
            lsfnefgkj.JOYSTICKPINZAUP();
            laernvk.JOYSTICKPINZAUP();

            bBrazo.JOYSTICKPINZAUP();
            blfbrsv.JOYSTICKPINZAUP();
            blsfnefgkj.JOYSTICKPINZAUP();
            blaernvk.JOYSTICKPINZAUP();

            JAC.JoyABRIR();
        }
        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

            if (touchpad.y > 0.7f)
            {
                JOYIZQuierdo.JIUP();
            }

            else if (touchpad.y < -0.7f)
            {
                JOYIZQuierdo.JIDOWN();
            }

            if (touchpad.x > 0.7f)
            {
                //JOYDER.JoyRIGHT();
                JOYIZQuierdo.JILEFT();
            }

            else if (touchpad.x < -0.7f)
            {

                JOYIZQuierdo.JIRIGTH();
            }

        }
    }
}
