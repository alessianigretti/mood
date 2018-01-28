using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallForEventManager : MonoBehaviour
{
	GameObject eventManager;

	void Start()
	{
		eventManager = GameObject.Find("Event Manager");
	}

	public void CallToEventManager(int answer)
	{
		eventManager.GetComponent<UIInteractions>().ChooseAnswer(answer);
	}
}
