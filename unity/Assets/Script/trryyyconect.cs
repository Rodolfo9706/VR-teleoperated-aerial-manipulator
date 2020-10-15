using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Windows;
using UnityEngine.Networking;
using WebSocketSharp;
using WebSocketSharp.Server;
using WebSocketSharp.Net.WebSockets;
using System.Net;
using System.Text;

public class trryyyconect : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		var ws = new WebSocket(new Uri("ws://192.168.0.8:9090"));
		yield return StartCoroutine(ws.Connect());

        while (true)
        {
			string reply = ws.RecvString();
			ws.SendString("START");

			if (reply != null)
			{
				Debug.Log("Received: " + reply);
			}

			if (ws.error != null)
			{
				Debug.LogError("Error: " + ws.error);
				break;
			}
			yield return 0;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
