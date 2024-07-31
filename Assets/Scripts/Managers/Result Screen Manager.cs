    using System;
    using System.Collections;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SocialPlatforms.Impl;
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
    
    
        private void Start()
        {
            confirmButton.onClick.AddListener(ButtonClick);
            mainMenuButton.onClick.AddListener(MainMenuButtonClick);

            scoreManager = ScoreManager.Instance;
            round = scoreManager.roundCount;

            if (scoreManager != null)
            {
                if (round == 1)
                {
                    //round2Column.SetActive(false);
                    //round3Column.SetActive(false);
                    //tieBreakerColumn.SetActive(false);

                    scoresRound1Manager = scoreManager.scoresRound1Manager;
                    CountTotal();


                InitizalizeScoreRound1();
                }

                if (round == 2)
                {
                    round2Column.SetActive(true);
                    round3Column.SetActive(false);
                    tieBreakerColumn.SetActive(false);

                    scoresRound1Manager = scoreManager.scoresRound1Manager;
                    scoresRound2Manager = scoreManager.scoresRound2Manager;
                    InitizalizeScoreRound2();
                    CountTotal();


                }

                if (round == 3)
                {
                    round2Column.SetActive(true);
                    round3Column.SetActive(true);
                    tieBreakerColumn.SetActive(false);

                    scoresRound1Manager = scoreManager.scoresRound1Manager;
                    scoresRound2Manager = scoreManager.scoresRound2Manager;
                    scoresRound3Manager = scoreManager.scoresRound3Manager;

                    InitizalizeScoreRound3();
                    CountTotal();


                }

                if (round == 4)
                {
                    round2Column.SetActive(true);
                    round3Column.SetActive(true);
                    tieBreakerColumn.SetActive(true);

                    scoresRound1Manager = scoreManager.scoresRound1Manager;
                    scoresRound2Manager = scoreManager.scoresRound2Manager;
                    scoresRound3Manager = scoreManager.scoresRound3Manager;
                    scoresRound4Manager = scoreManager.scoresRound4Manager;

                    InitizalizeScoreRound4();

                    CountTotal();

                }
            }
       
        }

        void MainMenuButtonClick()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }

        private void ButtonClick()
        {
            if (round < 3)
            {
                scoreManager.roundCount ++;
                // SceneManager.LoadSceneAsync("Level" + round);
                SceneManager.LoadSceneAsync(3);
            }
            else if ( round == 3)
            {
                if (scoresTotal[0] == scoresTotal[1] || scoresTotal[0] == scoresTotal[2] || scoresTotal[1] == scoresTotal[2])
                {
                    //SceneManager.LoadSceneAsync("Level" + UnityEngine.Random.Range(1, 3));
                    SceneManager.LoadSceneAsync(3);
                }
                else
                {
                    Debug.Log("Should be going to leaderboard Scene");
                }
            }
        }

        void InitizalizeScoreRound1()
        {
            for (int i = 0; i < scoresRound1.Length; i++)
            {
                scoresRound1[i].text = scoresRound1Manager[i].ToString();
            }
        }

        void InitizalizeScoreRound2()
        {
            for (int i = 0; i < scoresRound1.Length; i++)
            {
                scoresRound1[i].text = scoresRound1Manager[i].ToString();
            }

            for (int i = 0; i < scoresRound2.Length; i++)
            {
                scoresRound2[i].text = scoresRound2Manager[i].ToString();
            }
        }

        void InitizalizeScoreRound3()
        {
            for (int i = 0; i < scoresRound1.Length; i++)
            {
                scoresRound1[i].text = scoresRound1Manager[i].ToString();
            }

            for (int i = 0; i < scoresRound2.Length; i++)
            {
                scoresRound2[i].text = scoresRound2Manager[i].ToString();
            }

            for (int i = 0; i < scoresRound3.Length; i++)
            {
                scoresRound3[i].text = scoresRound3Manager[i].ToString();
            }
        }

        void InitizalizeScoreRound4()
        {
            for (int i = 0; i < scoresRound1.Length; i++)
            {
                scoresRound1[i].text = scoresRound1Manager[i].ToString();
            }

            for (int i = 0; i < scoresRound2.Length; i++)
            {
                scoresRound2[i].text = scoresRound2Manager[i].ToString();
            }

            for (int i = 0; i < scoresRound3.Length; i++)
            {
                scoresRound3[i].text = scoresRound3Manager[i].ToString();
            }

            for (int i = 0; i < scoresRound4.Length; i++)
            {
                scoresRound4[i].text = scoresRound4Manager[i].ToString();
            }
        }

        void CountTotal()
        {
            for (int i = 0;i < scoresTotal.Length;i++)
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
            Array.Sort(scoresTotal);

            for (int i = 0; i < scoresTotal.Length; i++)
            {
                Debug.Log("Number " + i+1 + "=" + scoresTotal[i]);
            }
        }
    }
