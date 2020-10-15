using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Windows;
using UnityEngine.Networking;
using System.Net.WebSockets;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using WebSocketSharp.Net.WebSockets;
using System.Net;
using System.Text;
using UnityEditor.Callbacks;

public class EchoTest : MonoBehaviour {
    [UPyPlot.UPyPlotController.UPyProbe]
    private float xVar;

    [UPyPlot.UPyPlotController.UPyProbe] // Add probe so this value will be plotted.
    private float yVar;

    [UPyPlot.UPyPlotController.UPyProbe] // Add probe so this value will be plotted.
    private float zVar;

    private float lastRndX = 0;
    private float lastRndY = 0;
    private float lastRndZ = 0;

    //GameObject Dron;
    WebSocket w;

    float buttom, middle, pinza, a, b = 0, rotation;

    string datas, dsend, ddd;

    //get rostation

    public void bbuttom(float bb)
    {
        buttom = bb;
    }

    public void bmiddle(float bm)
    {
        middle = bm;
    }

    public void bpinza(float bp)
    {
        pinza = bp;
    }

    public string Idd;
    string Idu = "ws://";    
    string  Advertise, JsonAd, ServoAbrir, SubscribeSend, AdvertiseSend, JsonAdS, JsonSS, JsonA, ServoCerrar, JsonC, Subscribe, JsonS, ServoDosAbrir, ServoDosCerrar, ServoTresCerrar, ServoTresAbrir, JsonDosA, JsonDosC, JsonTresA, JsonTresC;
    
    public DronMovement pose;

    //WebSocket ws = new WebSocket(new Uri("ws://172.30.10.250:9090"));
    //WebSocket ws = new WebSocket(new Uri("ws://172.30.10.165:9090"));
    float JoyAC,JoyGiro;

    public void JoyABRIR()
    {
        JoyAC = 1;
        //Debug.Log(JoyAC);
        
    }
    public void JoyCERRAR()
    {
        JoyAC = -1;
        //Debug.Log(JoyAC);
    }
    public void GiroUp()
    {
        JoyGiro = 1;
        //Debug.Log(JoyGiro);
    }
    public void GiroDown()
    {
        JoyGiro = -1;
        //Debug.Log(JoyGiro);
    }


    // Use this for initialization

    IEnumerator Start() {
        Idu += Idd;
        w = new WebSocket(new Uri(Idu));
        Advertise = Application.streamingAssetsPath + "/Advertise.json";
        JsonAd = File.ReadAllText(Advertise);

        AdvertiseSend = Application.streamingAssetsPath + "/AdvertiseSend.json";
        JsonAdS = File.ReadAllText(AdvertiseSend);

        ServoAbrir = Application.streamingAssetsPath + "/AbrirPinza.json";
        JsonA = File.ReadAllText(ServoAbrir);

        ServoCerrar = Application.streamingAssetsPath + "/Close.json";
        JsonC = File.ReadAllText(ServoCerrar);

        ServoDosAbrir = Application.streamingAssetsPath + "/ServoDosAbrir.json";
        JsonDosA = File.ReadAllText(ServoDosAbrir);

        ServoDosCerrar = Application.streamingAssetsPath + "/ServoDosCerrar.json";
        JsonDosC = File.ReadAllText(ServoDosCerrar);

        ServoTresAbrir = Application.streamingAssetsPath + "/ServoTresAbrir.json";
        JsonTresA = File.ReadAllText(ServoTresAbrir);

        ServoTresCerrar = Application.streamingAssetsPath + "/ServoTresCerrar.json";
        JsonTresC = File.ReadAllText(ServoTresCerrar);

        Subscribe = Application.streamingAssetsPath + "/Subscribe.json";
        JsonS = File.ReadAllText(Subscribe);

        SubscribeSend = Application.streamingAssetsPath + "/SubscribeSend.json";
        JsonSS = File.ReadAllText(SubscribeSend);

        //WebSocket ws = new WebSocket(new Uri("ws://172.30.10.165:9090"));
        yield return StartCoroutine(w.Connect());
        conectservo();

        int i = 0;
        while (true)    
		{
            xVar = Mathf.Lerp(lastRndX, middle, Time.deltaTime * 0.5f);
            lastRndX = xVar;

            yVar = Mathf.Lerp(lastRndY, buttom, Time.deltaTime * 0.5f);
            lastRndY = yVar;

            zVar = Mathf.Lerp(lastRndZ, pinza, Time.deltaTime * 0.5f);
            lastRndZ = zVar;
            a = middle + buttom + pinza;
            datas = buttom.ToString() + " " + middle.ToString() + " " + pinza.ToString();
            dsend = "{" + Environment.NewLine + "	\"op\": \"publish\"," + Environment.NewLine + "	\"topic\": \"brazo_data\"," + Environment.NewLine + "	\"msg\": {" + Environment.NewLine + "		\"data\": " + "\" " + datas + " \"" + Environment.NewLine + "	}" + Environment.NewLine + "}";
            //Debug.Log(a);
            //Debug.Log(b);
            if (b!=a)
            {
                //conectbrazo();
                datosbrazo();
            }

            if (Input.GetAxis("Gatillo") > 0 | Input.GetKey(KeyCode.X) | JoyAC == -1)
            {
                //conservo();
                //w.SendString(JsonA);
                JoyAC = 0;
            };
            if(Input.GetAxis("Gatillo") < 0 | Input.GetKey(KeyCode.Z) | JoyAC == 91)
            {
                //conservo();
                //w.SendString(JsonC);
                JoyAC = 0;
            };
            if (Input.GetKey(KeyCode.C) | JoyGiro == 1)
            {
                //conservo();
                //w.SendString(JsonDosA);
                JoyGiro = 0;
            };
            if (Input.GetKey(KeyCode.V) | JoyGiro == -1)
            {
                //conservo();
                //w.SendString(JsonDosC);
                JoyGiro = 0;
            };
            if (Input.GetKey(KeyCode.F))
            {
                //conservo();
                //w.SendString(JsonTresA);
            };
            if (Input.GetKey(KeyCode.G))
            {
                //conservo();
                //w.SendString(JsonTresC);
            };

            //---------------------------------------------------------
            //conservo();

            var reply = w.RecvString();
                        
			if (reply != null)
			{
                
                //-------------------all------------------------------------
                Debug.Log ("Received: "+reply);
                var reply1 = reply.Split(" "[0]);

                //-------------------x-------------------------------------
                float resultx;
                string newreplytox = reply1[4];
                string newreplytox1 = newreplytox.Replace("\"","");
                //Debug.Log("xxx  ");
                //Debug.Log(newreplytox1);
                resultx = float.Parse(newreplytox1, System.Globalization.CultureInfo.InvariantCulture);
                //--------------------y-------------------------------------
                float resulty;
                string newreplytoy = reply1[5];
                //string newreplytoy1 = newreplytoy.Replace("\\nz:", "");
                //Debug.Log("yyy");
                
                resulty = float.Parse(newreplytoy, System.Globalization.CultureInfo.InvariantCulture);
                //--------------------z-----------------------------------
                float resultz;
                string newreplytoz = reply1[6];
                //string newreplytoz1 = newreplytoz.Replace("\"},", "");
                //Debug.Log("zzz ");
                //Debug.Log(newreplytoz1);
                resultz = float.Parse(newreplytoz, System.Globalization.CultureInfo.InvariantCulture);
                //------------------rotation-------------------------------
                string newreplyrot = reply1[7];
                string newreplyrot1 = newreplyrot.Replace("\"},", "");
                rotation = float.Parse(newreplyrot1, System.Globalization.CultureInfo.InvariantCulture);
                rotation = Math.Abs(rotation);
                //string newreplytox = newreplytoxy.TrimStart();
                pose.PosGetX(resultx);
                pose.PosGetY(resulty);
                pose.PosGetZ(resultz);
                pose.PosGetRot(rotation*360);
                reply1 = null;
			}

            if (Input.GetKey("escape"))
            {
                w.Close();
            }
            

			if (w.error != null)
			{ 
				Debug.LogError ("Error: "+ w.error);
				break;
			};
            yield return 0;
        }
	}
    void conectservo()
    {
        w.SendString(JsonS);
        w.SendString(JsonAd);
        //w.SendString(JsonSS);
        w.SendString(JsonAdS);
    }

    void conectbrazo()
    {
        //w.SendString(JsonSS);
        w.SendString(JsonAdS);
    }

    void conservo()
    {
        w.SendString(JsonS);
        w.SendString(JsonAd);
    }

    void datosbrazo()
    {
        w.SendString(dsend);
    }
}
