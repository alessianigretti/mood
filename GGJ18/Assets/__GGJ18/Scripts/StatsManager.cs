using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public TextMeshProUGUI followerText;
    public TextMeshProUGUI viewerText;
    public UIInteractions uiInteract;
    public AddChatMessages chatMessages;
    public float variable = 0.5f;

    int followers = 0;
    int newViewers = 0;
    public int totalViewers;
    
    void Start()
    {
        //uiInteract.totalFollowers = PlayerPrefs.GetInt("Followers");
        followers = PlayerPrefs.GetInt("Followers", 0);
        totalViewers = followers;
        //uiInteract.totalViewers = totalViewers;
        InnerGameController.Instance.OnPlayerKillsEnemy +=	InnerGameController_OnPlayerKillsEnemy;
    }

    void InnerGameController_OnPlayerKillsEnemy(int points)
    {
        newViewers += points;
        totalViewers += newViewers;
    }

    void Update()
    {
        followerText.SetText(followers.ToString());
        viewerText.SetText(totalViewers.ToString());
    }

    public void UpdateFollowers()
    {

        followers += (int)((float)newViewers * ((float)uiInteract.chatScore / (float)chatMessages.counter) * variable);
        newViewers = 0;

        PlayerPrefs.SetInt("Followers", followers);
        PlayerPrefs.Save();
    }
}