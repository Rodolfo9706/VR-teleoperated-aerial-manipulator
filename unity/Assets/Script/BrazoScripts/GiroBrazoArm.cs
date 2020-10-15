using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiroBrazoArm : MonoBehaviour {
    float Joystickgiro;
    float eulerxmodified;
    float eulerx;
    float eulery;
    public EchoTest brazo;
    public void BRAZOUP()
    {
        Joystickgiro = 1;
    }
    public void BRAZODOWN()
    {
        Joystickgiro = -1;
    }
    public float velocidad = 0;
    float LeapRote;
    public void Leaplr()
    {
        LeapRote = -1;
    }
    public void Leapll()
    {
        LeapRote = 1;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        eulerx=this.transform.localEulerAngles.x;
        eulery=this.transform.localEulerAngles.y;
        if (eulery < 10)
        {
            if (eulerx >= 265)
            {
                eulerxmodified = eulerx - 270;
            }else if (eulerx <= 190)
            {
                eulerxmodified = eulerx + 90;
            }
            
        }else if (eulery >= 165)
        {
            if (eulerx < 100)
            {
                eulerxmodified = 270 - eulerx;
            }else if (eulerx > 200)
            {
                eulerxmodified = 630 - eulerx;
            }
            
        }
        
        
        if (Input.GetKey(KeyCode.C) | Joystickgiro == 1 | LeapRote == 1)
        {
            transform.Rotate(new Vector3(0, velocidad, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.V) | Joystickgiro == -1 | LeapRote == -1)
        {
            transform.Rotate(new Vector3(0, -velocidad, 0) * Time.deltaTime);
        }
        
        brazo.bmiddle(360 - eulery);
        Joystickgiro = 0;
        LeapRote = 0;
        //Debug.Log(eulerxmodified);
    }
}
