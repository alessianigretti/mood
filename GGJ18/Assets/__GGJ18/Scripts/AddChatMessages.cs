using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddChatMessages : MonoBehaviour
{
	public GameObject messagePrefab, parentObj;
	Button latestBtn1, latestBtn2;
    public Image avatar;
	public float wait = 10f;
	public StatsManager stats;

	List<GameObject> interventionPanels = new List<GameObject>();
	List<string> names, interventions, answers0, answers1;
	TextMeshProUGUI textAnswers0, textAnswers1;
	public int counter = 0; // tot chat messages

	void Awake()
	{
		names = I2.Loc.LocalizationManager.GetTermsList("Names");
        interventions = I2.Loc.LocalizationManager.GetTermsList("Interventions");
        answers0 = I2.Loc.LocalizationManager.GetTermsList("Answers0");
		answers1 = I2.Loc.LocalizationManager.GetTermsList("Answers1");
	}
	
	void Start()
	{
		InnerGameController.Instance.OnPlayerDie +=	InnerGameController_OnPlayerDie;
		StartCoroutine(Spawn(wait));
	}

	void InnerGameController_OnPlayerDie()
	{
		latestBtn1.interactable = false;
		latestBtn2.interactable = false;
		stats.UpdateFollowersEndOfMatch();
	}

	IEnumerator Spawn(float amount)
	{
		yield return new WaitForSeconds(amount);

        /*)if (stats.totalViewers >= 0 && stats.totalViewers < 10)
        {
            amount = 10;
            Debug.Log("Tengo " + stats.totalViewers.ToString() + " viewers y el tiempo es " + amount);
        }
        else if (stats.totalViewers > 11 && stats.totalViewers < 100)
        {
            amount = 7;
            Debug.Log("Tengo " + stats.totalViewers.ToString() + " viewers y el tiempo es " + amount);
        }
        else if (stats.totalViewers > 101)
        {
            amount = 3;
            Debug.Log("Tengo " + stats.totalViewers.ToString() + " viewers y el tiempo es " + amount);
        }*/

		GameObject newMessage = Instantiate(messagePrefab);
		newMessage.transform.SetParent(parentObj.transform);
		newMessage.transform.localScale = new Vector2(1f, 1f);
		
		if (counter >= names.Count)
		{
			counter = 0;	
		}

		TextMeshProUGUI[] nameT = newMessage.GetComponentsInChildren<TextMeshProUGUI>();
		
		for (int i = 0; i < nameT.Length; i++)
		{
			if (nameT[i].name == "Name")
			{
				nameT[i].SetText(I2.Loc.LocalizationManager.GetTranslation(names[counter]));
			}
			else if (nameT[i].name == "Text")
			{
				nameT[i].SetText(I2.Loc.LocalizationManager.GetTranslation(interventions[counter]));
			}
		}

		Button[] answerBtns = newMessage.GetComponentsInChildren<Button>();

		for (int i = 0; i < answerBtns.Length; i++)
		{
			if (answerBtns[i].name == "Answer 1")
			{
				textAnswers0 = answerBtns[i].GetComponentInChildren<TextMeshProUGUI>();
				textAnswers0.SetText(I2.Loc.LocalizationManager.GetTranslation(answers0[counter]));
				latestBtn1 = answerBtns[i];
			}
			else if (answerBtns[i].name == "Answer 2")
			{
				textAnswers1 = answerBtns[i].GetComponentInChildren<TextMeshProUGUI>();
				textAnswers1.SetText(I2.Loc.LocalizationManager.GetTranslation(answers1[counter]));
				latestBtn2 = answerBtns[i];
			}
		}

		counter++;

            
		StartCoroutine(Timer(amount));
		StartCoroutine(DestroyMessage(newMessage, amount * 8));
	}

	IEnumerator Timer(float amount)
	{
		yield return new WaitForSeconds(amount);

		latestBtn1.interactable = false;
		latestBtn2.interactable = false;

		StartCoroutine(Spawn(amount));
	}

	IEnumerator DestroyMessage(GameObject message, float amount)
	{
		yield return new WaitForSeconds(amount);

		Destroy(message);
	}
}
