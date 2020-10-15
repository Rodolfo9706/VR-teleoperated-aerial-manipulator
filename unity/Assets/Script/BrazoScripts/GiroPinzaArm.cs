using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiroPinzaArm : MonoBehaviour {
    public float velocidad = 0;
    float Brazorot;
    public void BRAZOUP()
    {
        Brazorot = 1;
    }
    public void BRAZODOWN()
    {
        Brazorot = -1;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.B) | Brazorot == 1)
        {
            transform.Rotate(new Vector3(0, velocidad, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.N) | Brazorot == -1)
        {
            transform.Rotate(new Vector3(0, -velocidad, 0) * Time.deltaTime);
        }
        Brazorot = 0;
    }
}
