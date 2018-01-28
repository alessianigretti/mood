using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MyTestSocketIO))]
public class MyTestSocketIOEditor : Editor {

	MyTestSocketIO myScript;


	void OnEnable () {
		myScript = (MyTestSocketIO)target;
	}

	public override void OnInspectorGUI () {
		DrawDefaultInspector ();

		if (Application.isPlaying) {
			if (GUILayout.Button ("Connect")) {
				myScript.Connect ();
			}

			if (myScript.IsConnected ()) {
				if (GUILayout.Button ("Beep!")) {
					myScript.Beep ();
				}

				if (GUILayout.Button ("JSON!")) {
					myScript.SendJSON ();
				}

				if (GUILayout.Button ("Disconnect")) {
					myScript.Disconnect ();
				}
			}
		}
	}

}
