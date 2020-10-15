using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private Transform MyDron;
    
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public bool CameraSwitch = true;

    void Awake()
    {
        MyDron = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    public void ChangeCameraPerspective()
    {
        if (CameraSwitch == true) CameraSwitch = false;
        else if (CameraSwitch == false) CameraSwitch = true;
    }


    private Vector3 velocityCameraFollow;
    public Vector3 behindPosition = new Vector3(0, 2, -40);
    public Vector3 BesidePosition = new Vector3(50, 2, 0);
    public float angle;
    public float angle2 = -100;
    void FixedUpdate()
    {
        if (CameraSwitch == true)
        {
            transform.position = Vector3.SmoothDamp(transform.position, MyDron.transform.TransformPoint(behindPosition) + Vector3.up * Input.GetAxis("Vertical"), ref velocityCameraFollow, 0.1f);
            transform.rotation = Quaternion.Euler(new Vector3(angle, MyDron.GetComponent<DronMovement>().currentYRotation, 0));
        }
        else if(CameraSwitch == false)
        {
            transform.position = Vector3.SmoothDamp(transform.position, MyDron.transform.TransformPoint(BesidePosition) + Vector3.up * Input.GetAxis("Vertical"), ref velocityCameraFollow, 0.1f);
            transform.rotation = Quaternion.Euler(new Vector3(angle, angle2 + MyDron.GetComponent<DronMovement>().currentYRotation, 0));
        }
    }
}
    