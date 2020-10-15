using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DronMovement : MonoBehaviour {
    //[UPyPlot.UPyPlotController.UPyProbe]
    private float xVar;

    //[UPyPlot.UPyPlotController.UPyProbe] // Add probe so this value will be plotted.
    private float yVar;

    //[UPyPlot.UPyPlotController.UPyProbe] // Add probe so this value will be plotted.
    private float zVar;

    private float lastRndX = 0;
    private float lastRndY = 0;
    private float lastRndZ = 0;
    
    //private Transform DronEnab;
    Rigidbody MyDron;
    public bool Abrir, Cerrar;

    //posicion recivida drone arm ------------
    float posx;
    float posy;
    float posz;
    float posrot;
    //posicion drone virtual
    float dposx;
    float dposy;
    float dposz;
    float dposrot;

    //obtener posiciones
    public void PosGetX(float psgx)
    {
        posx = psgx;
    }

    public void PosGetY(float psgy)
    {
        posy = psgy;
    }

    public void PosGetZ(float psgz)
    {
        posz = psgz;
    }

    public void PosGetRot(float psrot)
    {
        posrot = psrot;
    }

    // Use this for initialization
    void Awake()
    {
        MyDron = GetComponent<Rigidbody>();
    }

    public bool DronControl;
    public void ChangeToDronControl()
    {

        //DronControl = true;
        if (DronControl == true) { DronControl = false; }

        else if (DronControl == false) { DronControl = true; }

    }
    void FixedUpdate()
    {
        //float axis = Input.GetAxis("Vertical1");

        if (DronControl == true)
        {
            MyDron.useGravity = true;

            dposx = transform.localPosition.x;
            dposy = transform.localPosition.y;
            dposz = transform.localPosition.z;
            dposrot = transform.localEulerAngles.y;

            //Debug.Log(dposx);
            //Debug.Log(posx);
            //Debug.Log("dposrotfadsfad");

            //Debug.Log(DronControl);
            MovementUpDown();
            MovementForward();
            Rotation();
            ClampingSpeedValues();
            Swerwe();
            MyDron.AddRelativeForce(Vector3.up * upForce);
            MyDron.rotation = Quaternion.Euler(new Vector3(tiltAmountForward, currentYRotation, -tiltAmountSideways));

        }
        else if (DronControl == false)
        {
            MyDron.useGravity = false;
            //MyDron.AddRelativeForce(Vector3.up * upForce);
            MyDron.rotation = Quaternion.Euler(
                new Vector3(tiltAmountForward, currentYRotation, -tiltAmountSideways));

        }
        
        //plot 
        xVar = Mathf.Lerp(lastRndX, dposx, Time.deltaTime * 0.5f);
        lastRndX = xVar;

        yVar = Mathf.Lerp(lastRndY, dposy, Time.deltaTime * 0.5f);
        lastRndY = yVar;

        zVar = Mathf.Lerp(lastRndZ, dposz, Time.deltaTime * 0.5f);
        lastRndZ = zVar;
        
    }


    private float movementForwardSpeed = 200.0f;
    private float tiltAmountForward = 0;
    private float tiltVelocityForward;
    float JIV;
    public void JIUP()
    {
        JIV = 1;
    }
    public void JIDOWN()
    {
        JIV = -1;
    }
    float LeapAde;


    void MovementForward()
    {
        //transform.position = new Vector3(posx * 10,posy * 10,posz * 10);
        float vely = 0;
        
        if ((dposz < posy - 2) && posy != 0)
        {
            vely = .3f;
        }else if ((dposz > posy + 2) && posy != 0)
        {
            vely = -.3f;
        }
        else
        {
            vely = 0;
        }
        

        float MoveForwar = Input.GetAxis("Vertical") + vely;
        float MoveForward = JIV + MoveForwar + LeapAde;// + posx;
        //Debug.Log(MoveForward);
        if (MoveForward != 0)
        {
            MyDron.AddRelativeForce(Vector3.forward * MoveForward * movementForwardSpeed);
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, MoveForward, ref tiltVelocityForward, 3.5f);
            JIV = 0;
            LeapAde = 0;
        }
        vely = 0;
    }

    //programacionjoystick
    float JoyVER;
    public void JoyUP()
    {
        JoyVER = 1;
    }
    public void JoyDOWN()
    {
        JoyVER = -1;
    }

    

    public float upForcee;
    float upForce;
    void MovementUpDown()
    {
        
        float velz = 0;
        
        if (dposy < posz - 2 && posz != 0)
        {
            velz = 1;
        }else if(dposy > posz + 2 && posz != 0)
        {
            velz = -1;
        }
        else
        {
            velz = 0;
        }
        
        upForce += upForcee;// + posz;
        if (Input.GetKey(KeyCode.I) | JoyVER == 1 | velz == 1)
        {
            upForce = 190;
        }
        else if (Input.GetKey(KeyCode.K) | JoyVER == -1 | velz == -1)
        {
            upForce = -20;
        }
        else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K))
        {
            upForce = 98.1f;
        }
        JoyVER = 0;
        velz = 0;
        
    }

    float JoyHOR;
    public void JoyRIGHT()
    {
        JoyHOR = 1;
    }
    public void JoyLEFT()
    {
        JoyHOR = -1;
    }


    private float wantedYRotation;
    [HideInInspector] public float currentYRotation;
    private float rotateAmoutByKey = 1.0f;
    private float rotationYVelocity;
    void Rotation()
    {
        float velrot = 0;
        if ((dposrot < posrot - 1 || dposrot > posrot + 1) && posrot != 0)
        {
            if (dposrot < posrot)
            {
                velrot = 1f;
            }
            else if ((dposrot > posrot) && posrot != 0)
            {
                velrot = -1f;
            }
        }
        else
        {
            velrot = 0;
        }

        if (Input.GetKey(KeyCode.J) | JoyHOR == -1 | velrot == -1)
        {
            wantedYRotation -= rotateAmoutByKey;
        }
        if (Input.GetKey(KeyCode.L) | JoyHOR == 1 | velrot == 1)
        {
            wantedYRotation += rotateAmoutByKey;
        }
        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.05f);
        JoyHOR = 0;
        velrot = 0;
    }
    private Vector3 velocityToSmoothDampToZero;
    void ClampingSpeedValues()
    {
        if (Mathf.Abs(JIV) > 0.2f && Mathf.Abs(JIH) > 0.2f)
        {
            MyDron.velocity = Vector3.ClampMagnitude(MyDron.velocity, Mathf.Lerp(MyDron.velocity.magnitude, 5.0f, Time.deltaTime * 2f));
        }
        if (Mathf.Abs(JIV) > 0.2f && Mathf.Abs(JIH) < 0.2f)
        {
            MyDron.velocity = Vector3.ClampMagnitude(MyDron.velocity, Mathf.Lerp(MyDron.velocity.magnitude, 5.0f, Time.deltaTime * 2f));
        }
        if (Mathf.Abs(JIV) < 0.2f && Mathf.Abs(JIH) > 0.2f)
        {
            MyDron.velocity = Vector3.ClampMagnitude(MyDron.velocity, Mathf.Lerp(MyDron.velocity.magnitude, 5.0f, Time.deltaTime * 2f));
        }
        if (Mathf.Abs(JIV) < 0.2f && Mathf.Abs(JIH) < 0.2f)
        {
            MyDron.velocity = Vector3.SmoothDamp(MyDron.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);
        }
    }

    private float sideMovementAmount = 250.0f;
    private float tiltAmountSideways;
    private float tiltAmountVelocity;
    //drone
    float JIH;
    public void JIRIGTH()
    {
        JIH = 1;
    }
    public void JILEFT()
    {
        JIH = -1;
    }


    void Swerwe()
    {
        float velx = 0;
        
        if ((posx < dposx - 1) && posx != 0)
        {
            velx = -.5f;
        }
        else if ((posx > dposx + 1) && posx != 0)
        {
            velx = .5f;
        }
        else
        {
            velx = 0;
        }
        

        float atc = Input.GetAxis("Horizontal");
        float acac = atc + JIH + velx;// + posy;

        if (Mathf.Abs(acac) > 0.2f)
        {
            MyDron.AddRelativeForce(Vector3.right * acac * sideMovementAmount);
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 15 * acac, ref tiltAmountVelocity, 5.1f);
        }
        else
        {
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltAmountVelocity, 5.1f);
        }
        JIH = 0;
        velx = 0;
    }
}

