using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultScreenManager : MonoBehaviour
{
    [SerializeField] TMP_Text[] scoresRound1;
    [SerializeField] TMP_Text[] scoresRound2;
    [SerializeField] TMP_Text[] scoresRound3;
    [SerializeField] TMP_Text[] scoresRound4;
    [SerializeField] TMP_Text[] scoresTotalText;

    [SerializeField] int[] scoresTotal;

    [SerializeField] GameObject round1Column, round2Column, round3Column, tieBreakerColumn;

    int round;

    int[] scoresRound1Manager;
    int[] scoresRound2Manager;
    int[] scoresRound3Manager;
    int[] scoresRound4Manager;

    [SerializeField] Button confirmButton;
    [SerializeField] Button mainMenuButton;

    ScoreManager scoreManager;

    [SerializeField] TMP_Text[] rankings;

    private void Start()
    {
        confirmButton.onClick.AddListener(ButtonClick);
        mainMenuButton.onClick.AddListener(MainMenuButtonClick);

        scoreManager = ScoreManager.Instance;
        round = scoreManager.roundCount;

        if (scoreManager != null)
        {
            scoresRound1Manager = scoreManager.scoresRound1Manager;
            scoresRound2Manager = scoreManager.scoresRound2Manager ?? new int[scoresRound1Manager.Length];
            scoresRound3Manager = scoreManager.scoresRound3Manager ?? new int[scoresRound1Manager.Length];
            scoresRound4Manager = scoreManager.scoresRound4Manager ?? new int[scoresRound1Manager.Length];

            if (round == 1)
            {
                CountTotal();
                InitializeScoreRound1();
            }
            else if (round == 2)
            {
                InitializeScoreRound2();
                CountTotal();
            }
            else if (round == 3)
            {
                InitializeScoreRound3();
                CountTotal();
            }
            else if (round == 4)
            {
                InitializeScoreRound4();
                CountTotal();
            }
        }
    }

    private void ButtonClick()
    {
        if (round < 3)
        {
            scoreManager.roundCount++;
            //SceneManager.LoadSceneAsync(3);

            if (round == 1)
            {
                SceneManager.LoadSceneAsync("PRO_TestLvl.2");
            }
            else if (round == 2)
            {
                SceneManager.LoadSceneAsync("PRO_TestLvl.3");
            }
        }
        else if (round == 3)
        {
            if (scoresTotal[0] == scoresTotal[1] || scoresTotal[0] == scoresTotal[2] || scoresTotal[1] == scoresTotal[2])
            {
                SceneManager.LoadSceneAsync(3);
            }
            else
            {
                SceneManager.LoadSceneAsync(0);

            }
        }
    }

    private void MainMenuButtonClick()
    {
        scoreManager.roundCount = 1;
        ResetScores();
        SceneManager.LoadSceneAsync(0);
    }

    private void ResetScores()
    {
        if (scoresRound1Manager != null)
            Array.Clear(scoresRound1Manager, 0, scoresRound1Manager.Length);
        if (scoresRound2Manager != null)
            Array.Clear(scoresRound2Manager, 0, scoresRound2Manager.Length);
        if (scoresRound3Manager != null)
            Array.Clear(scoresRound3Manager, 0, scoresRound3Manager.Length);
        if (scoresRound4Manager != null)
            Array.Clear(scoresRound4Manager, 0, scoresRound4Manager.Length);
        if (scoresTotal != null)
            Array.Clear(scoresTotal, 0, scoresTotal.Length);
    }

    private void InitializeScoreRound1()
    {
        for (int i = 0; i < scoresRound1.Length; i++)
        {
            scoresRound1[i].text = scoresRound1Manager[i].ToString();
        }
    }

    private void InitializeScoreRound2()
    {
        InitializeScoreRound1();
        for (int i = 0; i < scoresRound2.Length; i++)
        {
            scoresRound2[i].text = scoresRound2Manager[i].ToString();
        }
    }

    private void InitializeScoreRound3()
    {
        InitializeScoreRound2();
        for (int i = 0; i < scoresRound3.Length; i++)
        {
            scoresRound3[i].text = scoresRound3Manager[i].ToString();
        }
    }

    private void InitializeScoreRound4()
    {
        InitializeScoreRound3();
        for (int i = 0; i < scoresRound4.Length; i++)
        {
            scoresRound4[i].text = scoresRound4Manager[i].ToString();
        }
    }

    private void CountTotal()
    {
        for (int i = 0; i < scoresTotal.Length; i++)
        {
            scoresTotal[i] = scoreManager.scoresRound1Manager[i] + scoreManager.scoresRound2Manager[i] + scoreManager.scoresRound3Manager[i] + scoreManager.scoresRound4Manager[i];
        }

        for (int i = 0; i < scoresTotalText.Length; i++)
        {
            scoresTotalText[i].text = scoresTotal[i].ToString();
        }

        Ranking();
    }

    private void Ranking()
    {
        int[] playerIndices = { 0, 1, 2 }; // assuming 3 players
        Array.Sort(scoresTotal, playerIndices);
        Array.Reverse(scoresTotal);
        Array.Reverse(playerIndices);

        for (int i = 0; i < scoresTotal.Length; i++)
        {
            Debug.Log($"Position {i + 1}: Player {playerIndices[i] + 1} with score {scoresTotal[i]}");
            rankings[i].SetText($"Position {i + 1}: Player {playerIndices[i] + 1} with score {scoresTotal[i]}");
        }
    }
}
