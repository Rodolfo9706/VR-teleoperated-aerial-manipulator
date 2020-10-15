
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class ControlLeapMotion : MonoBehaviour {
    Controller controler;
    public GiroBrazoArm LeapRot;
    float HandPalmPitch;
    float HandPalmYam;
    float HandPaImRoll;
    float HandWristRot;

    public bool P;
    public bool R;
    public bool Y;
     // ROTACION
    /*float rll = -2f;
    float rlr = -.7f;
    float rrl = .7f;
    float rrr = 2f;*/
        //LEVITACION
    float lul = -1.05f;
    float lur = 1f;
    float ldl = -3f;
    float ldr = -1.6f;

    void Start () {
		
	}

    void Update() {
        controler = new Controller();
        Frame frame = controler.Frame();
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
                //LEVITATION
        if (HandPalmPitch >= lul && HandPalmPitch <= lur)
        {
            LeapRot.Leaplr();
        }
        if (HandPalmPitch >= ldl && HandPalmPitch <= ldr)
        {
            LeapRot.Leapll();
        }
                //ROTATION
                /*
        if (HandPaImRoll >= rll && HandPaImRoll <= rlr)
        {
            //LeapRot.LeapRL();
        }
        if (HandPaImRoll >= rrl && HandPaImRoll <= rrr)
        {
            //LeapRot.LeapRR();
        }
        */
    }
}
