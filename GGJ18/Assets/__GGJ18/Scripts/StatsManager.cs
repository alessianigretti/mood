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

    int followersEndOfMatch = 0;
    int newViewers = 0;
    public int totalViewers;
    
    void Start()
    {
        uiInteract.totalFollowers = 0;
        totalViewers = PlayerPrefs.GetInt("viewers");
        uiInteract.totalViewers = 0;
        InnerGameController.Instance.OnPlayerKillsEnemy +=	InnerGameController_OnPlayerKillsEnemy;
    }

    void InnerGameController_OnPlayerKillsEnemy(int points)
    {
        newViewers += points;
        totalViewers = newViewers;
        PlayerPrefs.SetInt("viewers",totalViewers);
    }

    void Update()
    {
        followerText.SetText(followersEndOfMatch.ToString());
        viewerText.SetText(totalViewers.ToString());
    }

    public void UpdateFollowersEndOfMatch()
    {
        followersEndOfMatch += (int) (newViewers * (uiInteract.chatScore/chatMessages.counter) * variable);
    }
}
