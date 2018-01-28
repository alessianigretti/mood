using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Information : MonoBehaviour {

    [SerializeField]
    Image _avatarImage;

    [SerializeField]
    Sprite[] _avatarsList;

    public int index;

    private void OnEnable()
    {
        _avatarImage.sprite = _avatarsList[Random.Range(0, _avatarsList.Length)];
    }
}
