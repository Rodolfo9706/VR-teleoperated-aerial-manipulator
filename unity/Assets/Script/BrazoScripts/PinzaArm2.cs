using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinzaArm2 : MonoBehaviour {

    float Joystickpinzaarm;
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
        if (Input.GetKey(KeyCode.Z) | Joystickpinzaarm == 1 | LeapAde == -1)
        {
            transform.Rotate(new Vector3(velocidad, 0, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.X) | Joystickpinzaarm == -1 | LeapAde == 1)
        {
            transform.Rotate(new Vector3(-velocidad, 0, 0) * Time.deltaTime);
        }
        Joystickpinzaarm = 0;
        LeapAde = 0;
    }
}
