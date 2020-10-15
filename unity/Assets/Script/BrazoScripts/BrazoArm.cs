using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazoArm : MonoBehaviour
{
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
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
            if (Input.GetKey(KeyCode.Z) | Joystickpinzaarm == 1)
            {
                transform.Rotate(new Vector3(0, 0, velocidad) * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.X) | Joystickpinzaarm == -1)
            {
                transform.Rotate(new Vector3(0, 0, -velocidad) * Time.deltaTime);
            }
        Joystickpinzaarm = 0;
    }
}