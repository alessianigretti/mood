using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Information : MonoBehaviour {

    [SerializeField]
    Image _avatarImage;

    [SerializeField]
    TMP_Text _avatarName;

    [SerializeField]
    Sprite[] _avatarsList;

    [SerializeField]
    Color [] _colorList;

    public int index;

    private void OnEnable()
    {
        _avatarImage.sprite = _avatarsList[Random.Range(0, _avatarsList.Length)];
        _avatarName.color = _colorList[Random.Range(0, _colorList.Length)];
    }
}
