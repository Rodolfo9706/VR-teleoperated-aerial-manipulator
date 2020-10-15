using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinzaBrazoY : MonoBehaviour {

    float velocidad = 2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Z))
        {
            transform.Rotate(new Vector3(0, 0, -velocidad));
        }

        if (Input.GetKey(KeyCode.X))
        {
            transform.Rotate(new Vector3(0, 0, velocidad));
        }
    }
}
