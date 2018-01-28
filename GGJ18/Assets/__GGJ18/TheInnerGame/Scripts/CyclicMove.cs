using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CyclicMove : MonoBehaviour {

    [SerializeField]
    Vector3 _move;

	// Use this for initialization
	void Start () {
        Vector3 move = transform.localPosition + _move;
        transform.DOLocalMove(move, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
	}
}
