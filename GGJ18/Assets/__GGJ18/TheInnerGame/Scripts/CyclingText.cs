using MEC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class CyclingText : MonoBehaviour {

    TMP_Text _text;

    [SerializeField]
    private float _delay = 0.2f;

    private int _currentHide = 3;

	// Use this for initialization
	void Start () {
        _text = GetComponent<TMP_Text>();
        _text.maxVisibleCharacters = _text.textInfo.characterCount - _currentHide;
        Timing.CallDelayed(_delay, Next);
    }

    private void Next()
    {
        if (_currentHide == 0)
        {
            _currentHide = 3;
            _text.maxVisibleCharacters = _text.textInfo.characterCount - _currentHide;
        }
        else
        {
            --_currentHide;
            _text.maxVisibleCharacters = _text.textInfo.characterCount - _currentHide;
        }

        Timing.CallDelayed(_delay, Next);
    }
}
