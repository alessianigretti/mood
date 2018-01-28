using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream.StreamService {

	public class StreamerGamePanel : MonoBehaviour {

		public Game game;

		public void Connect () {
			ViewerManager.instance.currentGame = game;
			LobbyManager.instance.JoinGame (game);
		}

	}

}
