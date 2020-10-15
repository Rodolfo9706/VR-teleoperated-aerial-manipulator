using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinzaArm : MonoBehaviour
{
    float Joystickpinzaarm;
    public EchoTest brazo;
    float peulerx, peulery, peulerxmodified;
    public void JOYSTICKPINZAUP()
    {
        Joystickpinzaarm = 1;
    }
    public void JOYSTICKPINZADOWN()
    {
        Joystickpinzaarm = -1;
    }
    public float velocidad = 0;
    float LeapRot;
    public void LeapRL()
    {
        LeapRot = 1;
    }
    public void LeapRR()
    {
        LeapRot = -1;
    }


    float LeapAde;
    public void LeapAdel()
    {
        LeapAde = 1;
    }
    public void LeapAtra()
    {
        LeapAde = -1;
    }


    float LeapDI;
    public void LeapDer()
    {
        LeapDI = 1;
    }
    public void LeapIzq()
    {
        LeapDI = -1;
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    float minRotation = 30;
    float maxRotation = 60;
    void Update()
    {
        peulerx = this.transform.localEulerAngles.x;
        peulery = this.transform.localEulerAngles.y;

        if (peulery <= 95)
        {
            peulerxmodified = 360 - peulerx;
            /*
            if (peulerx >= 265)
            {
                peulerxmodified = peulerx - 270;
            }
            else if (peulerx <= 185)
            {
                peulerxmodified = peulerx + 90;
            }
            */
        }else if (peulery >= 165)
        {
            if (peulerx < 100)
            {
                peulerxmodified = peulerx + 180;
            }
            else if (peulerx > 200)
            {
                peulerxmodified = peulerx - 180;
            }
            
        }


        if (Input.GetKey(KeyCode.Z) | Joystickpinzaarm == 1 | LeapAde == -1)
        {
            transform.Rotate(new Vector3(velocidad, 0, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.X) | Joystickpinzaarm == -1 | LeapAde == 1)
        {
            transform.Rotate(new Vector3(-velocidad, 0, 0) * Time.deltaTime);
        }

        brazo.bpinza(360 - peulerxmodified);
        LeapAde = 0;
        Joystickpinzaarm = 0;
    }
}