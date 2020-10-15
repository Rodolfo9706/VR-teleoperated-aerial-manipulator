using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JVRDerecho : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    public DronMovement JOYDER; //drone

    public PinzaArm BrazDER; //pinza
    public PinzaArm BRAZZ;   //pinza
    public PinzaArm ARMMM;   //pinza
    public PinzaArm KDSV;    //pinza

    public PinzaArm2 sBrazDER; //pinza
    public PinzaArm2 sBRAZZ;   //pinza
    public PinzaArm2 sARMMM;   //pinza
    public PinzaArm2 sKDSV;    //pinza

    public EchoTest JOYCA;
    public EchoTest JBA;

    public GiroBrazoArm GPA; //brazo

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
            GPA.BRAZOUP();
            JBA.GiroUp();
        }
        if (Controller.GetHairTrigger())
        {
            BrazDER.JOYSTICKPINZADOWN();
            BRAZZ.JOYSTICKPINZADOWN();
            ARMMM.JOYSTICKPINZADOWN();
            KDSV.JOYSTICKPINZADOWN();

            sBrazDER.JOYSTICKPINZADOWN();
            sBRAZZ.JOYSTICKPINZADOWN();
            sARMMM.JOYSTICKPINZADOWN();
            sKDSV.JOYSTICKPINZADOWN();

            JOYCA.JoyCERRAR();
        }
        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));
            if (touchpad.y > 0.7f)
            {
                JOYDER.JoyUP();
            }

            else if (touchpad.y < -0.7f)
            {
                JOYDER.JoyDOWN();
            }

            if (touchpad.x > 0.7f)
            {
                JOYDER.JoyRIGHT();

            }

            else if (touchpad.x < -0.7f)
            {
                JOYDER.JoyLEFT();
            }

        }
    }
}
