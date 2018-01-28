using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public static PlayerController instance;

	[SerializeField]
	private float _sensitivity = 2f;

	[SerializeField]
	private float _shotRate = 0.5f;

	[SerializeField]
	RectTransform _gun;

	[SerializeField]
	Transform _cameraParent;

	[SerializeField]
	RectTransform _fire;

    [SerializeField]
    RectTransform _crossHair;

    private Transform _cameraTransform;
	private Vector2 _referenceMousePosition;

	private float _nextShot = 0;

	public bool inputEnabled = true;

	//[SerializeField]
	//private Vector2 _speed;

	private void Awake () {
		instance = this;
		_cameraTransform = Camera.main.transform;
	}

	private void Update () {
		if (inputEnabled) {
			if (Input.GetKeyDown (KeyCode.Tab)) {
				InnerGameController.Instance.SwitchInnerGameActive ();
			}

			if (InnerGameController.Instance.IsActive) {
				_cameraTransform.Rotate (
					-Input.GetAxis ("Mouse Y") * _sensitivity,
					0,
					0);

				transform.Rotate (
					0,
					Input.GetAxis ("Mouse X") * _sensitivity,
					0);

				_cameraTransform.eulerAngles = new Vector3 (
					_cameraTransform.eulerAngles.x, //Mathf.Clamp(_cameraTransform.eulerAngles.x, _minCameraRotation.x, _maxCameraRotation.x),
					_cameraTransform.eulerAngles.y, //Mathf.Clamp(_cameraTransform.eulerAngles.y, _minCameraRotation.y, _maxCameraRotation.y),
					0); //Mathf.Clamp(_cameraTransform.eulerAngles.z, _minCameraRotation.z, _maxCameraRotation.z));

				if (Input.GetMouseButtonDown (0)) {
					Shot ();
				}

				//Vector2 _move = Vector2.zero;
				//if (Input.GetKey(KeyCode.A))
				//{
				//    _move.x -= _speed.x;
				//}
				//if (Input.GetKey(KeyCode.D))
				//{
				//    _move.x += _speed.x;
				//}
				//if (Input.GetKey(KeyCode.W))
				//{
				//    _move.y += _speed.y;
				//}
				//if (Input.GetKey(KeyCode.S))
				//{
				//    _move.y -= _speed.y;
				//}

				//transform.position += transform.forward * _move.y * Time.deltaTime;
				//transform.position += transform.right * _move.x * Time.deltaTime;
			}

			_referenceMousePosition = Input.mousePosition;
		}
	}

	public void Shot () {
		if (Time.time >= _nextShot) {

            ShotEffects();

            if (inputEnabled) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (
					         new Vector2 (Camera.main.scaledPixelWidth / 2, Camera.main.scaledPixelHeight / 2));

				if (Physics.Raycast (ray, out hit, 50f, ~0)) {

					if (hit.collider.tag == "Enemy") {
						hit.collider.GetComponent<Enemy> ().TakeDamage ();
					}
				}
			}

			_nextShot = Time.time + _shotRate;
		}
	}

    private void ShotEffects()
    {
        _fire.gameObject.SetActive(true);
        Timing.CallDelayed(0.1f, () => {
            _fire.gameObject.SetActive(false);
        });

        if (TeamTheDream.StreamService.NetworkManager.instance != null)
        {

        }

        _cameraParent.DOLocalRotate(new Vector3(0f, 0f, -15f), 0.1f).SetEase(Ease.Flash).OnComplete(() =>
        {
            _cameraParent.DOLocalRotate(Vector3.zero, 0.35f);
        });

        _cameraParent.DOLocalMoveZ(-0.001f, 0.1f).SetEase(Ease.Flash).OnComplete(() =>
        {
            _cameraParent.DOLocalMoveZ(0, 0.3f);
        });

        _gun.DOAnchorPos3D(new Vector3(-250f, 0f, -190f), 0.15f).SetEase(Ease.Flash).OnComplete(() => {
            _gun.DOAnchorPos3D(new Vector3(-250f, 0f, 0f), 3f).SetEase(Ease.InQuart);
        });

        //transform.DOAnchorPos3D(new Vector3(-250f, 0f, -190f), 0.15f).SetEase(Ease.Flash).OnComplete(() => {
        //    transform.DOAnchorPos3D(new Vector3(-250f, 0f, 0f), 3f).SetEase(Ease.InQuart);
        //});


        _crossHair.gameObject.SetActive(false);
        Timing.CallDelayed(_shotRate, () =>
        {
            _crossHair.gameObject.SetActive(true);
        });
    }
}
