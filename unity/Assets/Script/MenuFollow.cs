using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFollow : MonoBehaviour
{

    private Transform MyCamera;


    void Awake()
    {
        MyCamera = GameObject.FindGameObjectWithTag("Cam").transform;

    }
    private Vector3 velocityCameraFollow;
    public Vector3 behindPosition = new Vector3(0, 2, 3);
    public float angle;

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, MyCamera.transform.TransformPoint(behindPosition), ref velocityCameraFollow, 0.1f);
		transform.rotation = Quaternion.Euler(new Vector3(angle, MyCamera.rotation.y, 0));

    }
}