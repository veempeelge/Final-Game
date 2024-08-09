using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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

    [SerializeField] GameObject firstPlayer1, firstPlayer2, firstPlayer3;
    [SerializeField] GameObject secondPlayer1, secondPlayer2, secondPlayer3;
    [SerializeField] GameObject thirdPlayer1, thirdPlayer2, thirdPlayer3;

    [SerializeField] String[] randomScenes;

    [SerializeField] int currentLevel;
    private void Start()
    {
        currentLevel = ScoreManager.Instance.lastLevel;

        Time.timeScale = 1f;
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
            if (round == 1)
            {
                if (currentLevel == 1)
                {
                    SceneManager.LoadSceneAsync(6);
                }
                if (currentLevel == 2)
                {
                    SceneManager.LoadSceneAsync(0);
                }

                if (currentLevel == 3)
                {
                    ResetScores();
                    SceneManager.LoadSceneAsync(0);
                }

            }
            else if (round == 2)
            {
                if (currentLevel == 2)
                {
                    SceneManager.LoadSceneAsync(7);
                }

                if (currentLevel == 3)
                {
                    ResetScores();
                    SceneManager.LoadSceneAsync(0);
                }
            }
        }
        else if (round == 3)
        {
            if (scoresTotal[0] == scoresTotal[1] || scoresTotal[0] == scoresTotal[2] || scoresTotal[1] == scoresTotal[2])
            {
                SceneManager.LoadSceneAsync(UnityEngine.Random.Range(5,7));
            }
            else
            {
                ResetScores();
                SceneManager.LoadSceneAsync(0);
            }
        }

        else if (round == 4)
        {
            ResetScores();
            SceneManager.LoadSceneAsync(0);
        }
    }

    private void MainMenuButtonClick()
    {
        round = 0;
        scoreManager.roundCount = 1;
        ResetScores();
        SceneManager.LoadSceneAsync(0);
    }

    private void ResetScores()
    {
        round = 0;
        for (int i = 0; i < scoresRound1.Length; i++)
        {
            scoresRound1[i].text = "0";
            scoresRound2[i].text = "0";
            scoresRound3[i].text = "0";
            scoresRound4[i].text = "0";
        }

        scoreManager.ResetScores();
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

        SetActiveRankingObjects(playerIndices);
    }

    private void SetActiveRankingObjects(int[] playerIndices)
    {
        // Disable all ranking objects first
        DisableAllRankingObjects();

        // Activate the appropriate ranking objects
        if (playerIndices[0] == 0) firstPlayer1.SetActive(true);
        else if (playerIndices[0] == 1) firstPlayer2.SetActive(true);
        else if (playerIndices[0] == 2) firstPlayer3.SetActive(true);

        if (playerIndices[1] == 0) secondPlayer1.SetActive(true);
        else if (playerIndices[1] == 1) secondPlayer2.SetActive(true);
        else if (playerIndices[1] == 2) secondPlayer3.SetActive(true);

        if (playerIndices[2] == 0) thirdPlayer1.SetActive(true);
        else if (playerIndices[2] == 1) thirdPlayer2.SetActive(true);
        else if (playerIndices[2] == 2) thirdPlayer3.SetActive(true);
    }

    private void DisableAllRankingObjects()
    {
        firstPlayer1.SetActive(false);
        firstPlayer2.SetActive(false);
        firstPlayer3.SetActive(false);
        secondPlayer1.SetActive(false);
        secondPlayer2.SetActive(false);
        secondPlayer3.SetActive(false);
        thirdPlayer1.SetActive(false);
        thirdPlayer2.SetActive(false);
        thirdPlayer3.SetActive(false);
    }
}
