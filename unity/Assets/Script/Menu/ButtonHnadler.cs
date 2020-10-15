using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonHnadler : MonoBehaviour {
	public GameObject Dron3D;
    public GameObject Camera;
    public GameObject Menu;
    DronMovement DronOn;
    CameraFollow CameraBrazo;
    CameraFollow MenuSwitch;
    BrazoArm Pinza;
    public bool Arm, Dron;
    void Start()
    {
       DronOn = Dron3D.GetComponent<DronMovement>();
        CameraBrazo = Camera.GetComponent<CameraFollow>();
        MenuSwitch = Menu.GetComponent<CameraFollow>();
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Y))
        {

             DronOn.ChangeToDronControl();
            CameraBrazo.ChangeCameraPerspective();
            MenuSwitch.ChangeCameraPerspective();
            //if (DronOn.DronControl == true) DronOn.DronControl = false;
           // if (DronOn.DronControl == false) DronOn.DronControl = true;

           
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "VrController")
        {
			if (Dron == true)
            {

                CameraBrazo.ChangeCameraPerspective();
                MenuSwitch.ChangeCameraPerspective();
            }
			if (Arm == true)
            {

                DronOn.ChangeToDronControl();
                CameraBrazo.ChangeCameraPerspective();
                MenuSwitch.ChangeCameraPerspective();
            }

		}
    }

}
