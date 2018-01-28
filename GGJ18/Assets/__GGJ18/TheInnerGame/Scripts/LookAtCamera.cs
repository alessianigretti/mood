using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    Transform _cameraTransform;

	// Use this for initialization
	void Start () {
        _cameraTransform = Camera.main.transform;
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(_cameraTransform, Vector3.up);
	}
}
