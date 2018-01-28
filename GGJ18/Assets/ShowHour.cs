using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ShowHour : MonoBehaviour {

    TMP_Text _timerText;

    int _minutes;
    int _seconds;

    // Use this for initialization
    void Awake () {
        _timerText = GetComponent<TMP_Text>();
    }
	
	// Update is called once per frame
	void Update () {
        _minutes = System.DateTime.Now.Hour;
        _seconds = System.DateTime.Now.Minute;

        SetTime();
    }

    void SetTime(float time)
    {
        _minutes = Mathf.FloorToInt(time / 60f);
        _seconds = Mathf.FloorToInt(time % 60);
        SetTime();
    }

    void SetTime()
    {
        _timerText.text = string.Format("{0:00}:{1:00}", _minutes, _seconds);
    }
}
