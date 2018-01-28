using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIInteractions : MonoBehaviour
{
    //public int totalFollowers = 0;
    //public int totalViewers = 0;

    public int chatScore;

    public void ChooseAnswer(int answerNumber)
    {
        float randomNum = Random.Range(0.0f, 1.0f);

        if (answerNumber == 1)
        {
            chatScore++;
        }
        else if (answerNumber == 2)
        {
            chatScore--;
            if (chatScore < 0) chatScore = 0;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
