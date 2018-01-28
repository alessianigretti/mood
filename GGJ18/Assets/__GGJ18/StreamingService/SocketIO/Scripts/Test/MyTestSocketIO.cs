using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class MyTestSocketIO : MonoBehaviour {

	[SerializeField] private SocketIOComponent socket = null;


	public void Start () {
		socket.On ("open", TestOpen);
		socket.On ("boop", TestBoop);
		socket.On ("error", TestError);
		socket.On ("close", TestClose);
	}

	void OnEnable () {
		if (socket == null) {
			Debug.LogWarning ("Please set a SocketIOComponent reference.");
			enabled = false;
		}
	}


	public void Connect () {
		socket.Connect ();
	}

	public void Disconnect () {
		if (socket.IsConnected) {
			socket.Close ();
		} else {
			Debug.LogWarning ("Socket is not connected.");
		}
	}


	public void Beep () {
		if (socket.IsConnected) {
			socket.Emit ("beep");
		} else {
			Debug.LogWarning ("Socket is not connected.");
		}
	}

	public void SendJSON () {
		if (socket.IsConnected) {
			JSONObject json = new JSONObject ();
			json.AddField ("user", "Ocariz");
			json.AddField ("role", "Monkey");
			json.AddField ("skills", "None");
			json.AddField ("traits", "Trocha, Vendehumos");
			Debug.Log (json);
			string str = "{\"username\":\"Alice\"}";
			socket.Emit ("json", JSONObject.CreateStringObject (str));
		} else {
			Debug.LogWarning ("Socket is not connected.");
		}
	}


	public void TestOpen (SocketIOEvent e) {
		Debug.Log ("[SocketIO] Open received: " + e.name + " " + e.data);
	}

	public void TestBoop (SocketIOEvent e) {
		Debug.Log ("[SocketIO] Boop received: " + e.name + " " + e.data);
		Debug.Log (
			"#####################################################\n" +
			"username: " + e.data.GetField ("username").str + "\n" +
			"#####################################################"
		);
	}

	public void TestError (SocketIOEvent e) {
		Debug.Log ("[SocketIO] Error received: " + e.name + " " + e.data);
	}

	public void TestClose (SocketIOEvent e) {	
		Debug.Log ("[SocketIO] Close received: " + e.name + " " + e.data);
	}


	public bool IsConnected () {
		return socket.IsConnected;
	}


}
