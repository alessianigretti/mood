using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

namespace TeamTheDream.StreamService {

	public class NetworkManager : MonoBehaviour {

		public static NetworkManager instance;

		[SerializeField] private SocketIOComponent socket = null;

		public bool IsConnected { get { return socket.IsConnected; } }

		private bool gameCreated = false;
		private int randomInt = 0;

		public GameObject[] enemyPrefabs;


		void Awake () {
			instance = this;
		}

		void Start () {
			randomInt = Random.Range (0, GameUtils.nameList.Length);
			socket.On ("open", Receive_Open);
			socket.On ("error", Receive_Error);
			socket.On ("close", Receive_Close);
		}

		void OnEnable () {
			// Check if there's a SocketIOComponent attached. Disable this script otherwise.
			if (socket == null) {
				Debug.LogWarning ("Please set a SocketIOComponent reference.");
				enabled = false;
			}
		}


		public void Connect () {
			socket.Connect ();
		}

		public void Disconnect () {
			socket.Close ();
		}


		public void SendJSON (string value, JSONObject json) {
			socket.Emit (value, json);
		}

		public void SendJSON (string value, string json) {
			socket.Emit (value, JSONObject.Create (json));
		}


		public void Receive_Open (SocketIOEvent e) {
			Debug.Log ("[SocketIO] Open received: " + e.name + " " + e.data);
			if (!gameCreated) {
				gameCreated = true;
				CreateGame (GameUtils.nameList [randomInt]);
			}
		}

		public void Receive_Close (SocketIOEvent e) {	
			Debug.Log ("[SocketIO] Close received: " + e.name + " " + e.data);
		}

		public void Receive_Error (SocketIOEvent e) {
			Debug.LogError ("[SocketIO] Error received: " + e.name + " " + e.data);
		}


		public void CreateGame (string username) {
			Debug.Log ("Creating game with username: " + username);
			SendJSON ("newgame", JsonUtility.ToJson (new Game (username)));
		}


		void OnApplicationQuit () {
			Disconnect ();
		}

	}

}
