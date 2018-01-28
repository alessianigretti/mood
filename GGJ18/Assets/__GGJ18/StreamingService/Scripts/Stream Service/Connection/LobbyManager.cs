using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

namespace TeamTheDream.StreamService {

	public class LobbyManager : MonoBehaviour {

		public static LobbyManager instance;
	
		[SerializeField] private GameObject streamerGamePrefab = null;
		[SerializeField] private Transform scrollViewContent = null;
		[SerializeField] private Transform loadingOverlay = null;

		private Game[] gamesList;


		void Awake () {
			instance = this;
		}

		void Start () {
			loadingOverlay.gameObject.SetActive (false);
		}


		public void ClearView () {
			for (int i = 0; i < scrollViewContent.childCount; i++) {
				Destroy (scrollViewContent.GetChild (i).gameObject);
			}
		}

		public void GetStreamersList () {
			ClearView ();
			ViewerManager.instance.socket.Emit ("getstreamers");
		}

		public void Receive_StreamerList (SocketIOEvent e) {
			JSONObject json = e.data.GetField ("streamers");
			gamesList = JSONHelper.GetJsonArray<Game> (json.ToString ());
			ShowGames (gamesList);
		}

		public void ShowGames (Game[] gamesList) {
			for (int i = 0; i < gamesList.Length; i++) {
				// Instantiate a new game panel.
				GameObject gamePanel = Instantiate (streamerGamePrefab);
				gamePanel.GetComponent<StreamerGamePanel> ().game = gamesList [i];
				// Set the streamer name.
				gamePanel.transform.Find ("Name").GetComponent<Text> ().text = gamesList [i].username;
				// Put them in the scroll view content.
				gamePanel.transform.SetParent (scrollViewContent);
			}
		}


		public void JoinGame (Game game) {
			loadingOverlay.gameObject.SetActive (true);
			JSONObject json = JSONObject.Create (JsonUtility.ToJson (game));
			ViewerManager.instance.socket.Emit ("joinstream", json);
		}

	}
}
