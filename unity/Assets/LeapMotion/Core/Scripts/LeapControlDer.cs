using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapControlDer : MonoBehaviour {

    Controller controller;
    public PinzaArm LeapRot;
    public PinzaArm LeapRot2;
    public PinzaArm LeapRot3;
    public PinzaArm LeapRot4;
    public PinzaArm LeapRot5;
    public PinzaArm LeapRot6;
    public GiroBrazoArm LeapRot7;
    public EchoTest control3;
    float HandPalmPitch;
    float HandPalmYam;
    float HandPaImRoll;
    float HandWristRot;

    public bool P;
    public bool R;
    public bool Y;
    // ROTACION
    float rll1 = -2f;               //RotationLeftLow
    float rlr1 = -.7f;          
    float rrl1 = .7f;
    float rrr1 = 2f;
    //LEVITACION
    float lul1 = -1f;
    float lur1 = .9f;
    float ldl1 = -3f;
    float ldr1 = -2f;

    void Start()
    {

    }

    void Update()
    {
        controller = new Controller();
        Frame frame = controller.Frame();
        List<Hand> hands = frame.Hands;
        if (frame.Hands.Count > 0)
        {
            Hand fristHand = hands[0];
        }

        HandPalmPitch = hands[0].PalmNormal.Pitch;
        HandPaImRoll = hands[0].PalmNormal.Roll;
        HandPalmYam = hands[0].PalmNormal.Yaw;

        HandWristRot = hands[0].WristPosition.Pitch;

        if (P == true)
        {
            Debug.Log("Pitch :" + HandPalmPitch);
        }
        if (R == true)
        {
            Debug.Log("Roll :" + HandPaImRoll);
        }
        if (Y == true)
        {
            Debug.Log("Yam :" + HandPalmYam);
        }
        if (HandPaImRoll >= rll1 && HandPaImRoll <= rlr1)
        {
            LeapRot.LeapAdel();
            LeapRot2.LeapAdel();
            LeapRot3.LeapAdel();
            LeapRot4.LeapAdel();
            LeapRot5.LeapAdel();
            LeapRot6.LeapAdel();
            control3.JoyABRIR();

        }
        if (HandPaImRoll >= rrl1 && HandPaImRoll <= rrr1)
        {
            LeapRot.LeapAtra();
            LeapRot2.LeapAtra();
            LeapRot3.LeapAtra();
            LeapRot4.LeapAtra();
            LeapRot5.LeapAtra();
            LeapRot6.LeapAtra();
            control3.JoyCERRAR();
        }
        //ROTATION
        if (HandPalmPitch >= lul1 && HandPalmPitch <= lur1)
        {
            LeapRot7.Leaplr();
            control3.GiroUp();
        }
        if (HandPalmPitch >= ldl1 && HandPalmPitch <= ldr1)
        {
            LeapRot7.Leapll();
            control3.GiroDown();
        }
    }

}
