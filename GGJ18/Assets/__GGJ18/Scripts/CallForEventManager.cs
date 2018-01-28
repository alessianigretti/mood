using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallForEventManager : MonoBehaviour
{
	GameObject eventManager;

	void Start()
	{
		eventManager = GameObject.Find("Event Manager");
	}

	public void CallToEventManager(int answer)
	{
        if (InnerGameController.Instance.IsActive) return;

        eventManager.GetComponent<UIInteractions>().ChooseAnswer(answer);
        GetComponent<Image>().color = new Color(0.36f, 0.6f, 0.36f);
	}
}