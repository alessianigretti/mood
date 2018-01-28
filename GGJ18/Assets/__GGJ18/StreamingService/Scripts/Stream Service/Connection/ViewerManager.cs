using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIO;

namespace TeamTheDream.StreamService {

	public class ViewerManager : MonoBehaviour {

		public static ViewerManager instance;

		public SocketIOComponent socket = null;
		public Game currentGame = null;

		public List<Synchronizable> synchronizables = new List<Synchronizable> ();
		public GameObject[] enemyPrefabs;

		public string viewerGameScene = "Stream_Viewer_Game";


		void Awake () {
			if (instance == null) {
				instance = this;
			} else {
				Destroy (gameObject);
				return;
			}
			DontDestroyOnLoad (gameObject);
		}

		void Start () {
			socket.On ("open", Receive_Open);
			socket.On ("error", Receive_Error);
			socket.On ("close", Receive_Close);
			socket.On ("streamerlist", LobbyManager.instance.Receive_StreamerList);
			socket.On ("streamjoined", Receive_Join);
			socket.On ("syncposition", Receive_Position);
			socket.On ("syncrotation", Receive_Rotation);
			socket.On ("spawnenemy", Receive_Spawn);
			socket.On ("killenemy", Receive_Kill);
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
		}

		public void Receive_Close (SocketIOEvent e) {	
			Debug.Log ("[SocketIO] Close received: " + e.name + " " + e.data);
		}

		public void Receive_Error (SocketIOEvent e) {
			Debug.LogError ("[SocketIO] Error received: " + e.name + " " + e.data);
		}

		public void Receive_Join (SocketIOEvent e) {
			SceneManager.LoadScene (viewerGameScene);
		}

		public void Receive_Position (SocketIOEvent e) {
			PositionSync position = JsonUtility.FromJson<PositionSync> (e.data.ToString ());
			for (int i = 0; i < synchronizables.Count; i++) {
				if (synchronizables [i].id == position.id) {
					if (synchronizables [i].syncPosition) {
						synchronizables [i].transform.position = new Vector3 (position.posX, position.posY, position.posZ);
					}
				}
			}
		}

		public void Receive_Rotation (SocketIOEvent e) {
			RotationSync rotation = JsonUtility.FromJson<RotationSync> (e.data.ToString ());
			for (int i = 0; i < synchronizables.Count; i++) {
				if (synchronizables [i].id == rotation.id) {
					if (synchronizables [i].syncRotation) {
						synchronizables [i].transform.rotation = Quaternion.Euler (new Vector3 (rotation.rotX, rotation.rotY, rotation.rotZ));
					}
				}
			}
		}

		public void Receive_Spawn (SocketIOEvent e) {
			ObjectSync objSync = JsonUtility.FromJson<ObjectSync> (e.data.ToString ());
			// Create the game object using the JSON's prefabId
			GameObject obj = Instantiate (enemyPrefabs [objSync.prefabId]);
			Synchronizable sync = obj.GetComponent<Synchronizable> ();
			sync.isServer = false;
			sync.id = objSync.id;
			// Destroy Enemy component if it exists.
			Enemy enemy = obj.GetComponent<Enemy> ();
			if (enemy != null) {
				Destroy (enemy);
			}
		}

		public void Receive_Kill (SocketIOEvent e) {
			ObjectSync objSync = JsonUtility.FromJson<ObjectSync> (e.data.ToString ());
			// Kill the game object using the JSON's id.
			for (int i = 0; i < synchronizables.Count; i++) {
				if (synchronizables [i].id == objSync.id) {
					Destroy (synchronizables [i].gameObject);
				}
			}
		}


		void OnApplicationQuit () {
			Disconnect ();
		}

	}

}
