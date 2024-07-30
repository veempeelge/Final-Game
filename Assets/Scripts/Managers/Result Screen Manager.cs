using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultScreenManager : MonoBehaviour
{
    public TMP_Text[,] scoreTable; // A 2D array or grid for the table

    private void Start()
    {
        DisplayScores();
    }

    private void DisplayScores()
    {
        if (ScoreManager.Instance != null)
        {
            int rounds = ScoreManager.Instance.GetRoundCount();

            for (int round = 0; round < rounds; round++)
            {
                int[] scores = ScoreManager.Instance.GetRoundScores(round);

                if (scores != null)
                {
                    for (int player = 0; player < scores.Length; player++)
                    {
                        scoreTable[round, player].text = scores[player].ToString();
                    }
                }
            }
        }
    }
}
