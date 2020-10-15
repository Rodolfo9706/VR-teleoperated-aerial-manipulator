using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBrazoArm : MonoBehaviour {
    public float velocidad = 0;
    public EchoTest brazo;

    float eulerx, eulery, eulerxmodified;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        eulerx = this.transform.localEulerAngles.x;
        eulery = this.transform.localEulerAngles.y;

        if (eulery <= 95)
        {
            if (eulerx >= 265)
            {
                eulerxmodified = eulerx - 270;
            }
            else if (eulerx <= 185)
            {
                eulerxmodified = eulerx + 90;
            }
        }
        else if (eulery >= 170)
        {
            if (eulerx < 100)
            {
                eulerxmodified = 270 - eulerx;
            }
            else if (eulerx > 200)
            {
                eulerxmodified = 630 - eulerx;
            }
        }


        if (Input.GetKey(KeyCode.F))
        {
            transform.Rotate(new Vector3(0, velocidad, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.G))
        {
            transform.Rotate(new Vector3(0, -velocidad, 0) * Time.deltaTime);
        }
        
        brazo.bbuttom(360 - eulery);
        //Debug.Log(eulerxmodified);
    }
}
