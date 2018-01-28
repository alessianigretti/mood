using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream.StreamService {

	[Serializable]
	public class Game {
		public string username = "user";
		public string socketId = "";

		public Game (string username) {
			this.username = username;
		}

		public Game (string username, string socketId) {
			this.username = username;
			this.socketId = socketId;
		}

	}

}
