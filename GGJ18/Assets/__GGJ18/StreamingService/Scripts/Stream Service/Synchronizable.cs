using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream.StreamService {
	
	public class Synchronizable : MonoBehaviour {

		public static int idCount = 1;
		public int id = 1;
		public int prefabId = 0;

		public bool isPlayer = false;

		public bool syncPosition = true;
		public bool syncRotation = true;

		public float syncTime = 0.1f;

		private ObjectSync objSync;
		private PositionSync position;
		private RotationSync rotation;

		public bool isServer = true;


		void Start () {
			if (isServer) {
				idCount++;
				id = idCount;
			}
			if (isPlayer) {
				id = 0;
			}
			objSync = new ObjectSync (id, prefabId);
			position = new PositionSync (id, transform.position);
			rotation = new RotationSync (id, transform.rotation);
			if (!isServer) {
				ViewerManager.instance.synchronizables.Add (this);
			}
			if (isServer) {
				StartCoroutine (SyncPos ());
				StartCoroutine (SyncRot ());
				NetworkManager.instance.SendJSON ("newenemy", JsonUtility.ToJson (objSync));
			}
		}


		IEnumerator SyncPos () {
			while (syncPosition) {
				yield return new WaitForSeconds (syncTime);
				UpdatePosition ();
				// Send info
				NetworkManager.instance.SendJSON ("updateposition", JsonUtility.ToJson (position));
			}
		}

		IEnumerator SyncRot () {
			while (syncRotation) {
				yield return new WaitForSeconds (syncTime);
				UpdateRotation ();
				// Send info
				NetworkManager.instance.SendJSON ("updaterotation", JsonUtility.ToJson (rotation));
			}
		}


		void UpdatePosition () {
			position.SetPosition (transform.position);
		}

		void UpdateRotation () {
			rotation.SetRotation (transform.rotation);
		}


		void OnDestroy () {
			if (!isServer) {
				ViewerManager.instance.synchronizables.Remove (this);
			} else {
				NetworkManager.instance.SendJSON ("enemydie", JsonUtility.ToJson (objSync));
			}
		}

	}

	[Serializable]
	public class ObjectSync {
		public int id;
		public int prefabId;

		public ObjectSync (int id, int prefabId) {
			this.id = id;
			this.prefabId = prefabId;
		}
	}

	[Serializable]
	public class PositionSync {
		public int id;
		public float posX;
		public float posY;
		public float posZ;

		public PositionSync (int id, Vector3 position) {
			this.id = id;
			SetPosition (position);
		}

		public void SetPosition (Vector3 position) {
			posX = position.x;
			posY = position.y;
			posZ = position.z;
		}
	}

	[Serializable]
	public class RotationSync {
		public int id;
		public float rotX;
		public float rotY;
		public float rotZ;

		public RotationSync (int id, Quaternion rotation) {
			this.id = id;
			SetRotation (rotation);
		}

		public void SetRotation (Quaternion rotation) {
			Vector3 rot = rotation.eulerAngles;
			rotX = rot.x;
			rotY = rot.y;
			rotZ = rot.z;
		}
	}

}
