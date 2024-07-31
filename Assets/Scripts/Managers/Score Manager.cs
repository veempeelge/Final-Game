using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int roundCount = 1;

    public int[] scoresRound1Manager;
    public int[] scoresRound2Manager;
    public int[] scoresRound3Manager;
    public int[] scoresRound4Manager;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
           Reset();
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < scoresRound1Manager.Length; i++)
        {
            scoresRound1Manager[i] = 0;
            scoresRound1Manager[i] = 0;
            scoresRound3Manager[i] = 0;
            scoresRound4Manager[i] = 0;
        }
    }
    public void Player1Won()
    {
        if (roundCount == 1)
        {
            scoresRound1Manager[0] += 3;
        }

        if (roundCount == 2)
        {
            scoresRound2Manager[0] += 3;
        }

        if (roundCount == 3)
        {
            scoresRound3Manager[0] += 3;
        }

        if (roundCount == 4)
        {
            scoresRound4Manager[0] += 3;
        }
    }

    public void Player1RunnerUp()
    {
        if (roundCount == 1)
        {
            scoresRound1Manager[0] += 1;
        }

        if (roundCount == 2)
        {
            scoresRound2Manager[0] += 1;
        }

        if (roundCount == 3)
        {
            scoresRound3Manager[0] += 1;
        }

        if (roundCount == 4)
        {
            scoresRound4Manager[0] += 1;
        }
    }

    public void Player2Won()
    {
        if (roundCount == 1)
        {
            scoresRound1Manager[1] += 3;
        }

        if (roundCount == 2)
        {
            scoresRound2Manager[1] += 3;
        }

        if (roundCount == 3)
        {
            scoresRound3Manager[1] += 3;
        }

        if (roundCount == 4)
        {
            scoresRound4Manager[1] += 3;
        }
    }

    public void Player2RunnerUp()
    {
        if (roundCount == 1)
        {
            scoresRound1Manager[1] += 1;
        }

        if (roundCount == 2)
        {
            scoresRound2Manager[1] += 1;
        }

        if (roundCount == 3)
        {
            scoresRound3Manager[1] += 1;
        }

        if (roundCount == 4)
        {
            scoresRound4Manager[1] += 1;
        }
    }

    public void Player3Won()
    {
        if (roundCount == 1)
        {
            scoresRound1Manager[2] += 3;
        }

        if (roundCount == 2)
        {
            scoresRound2Manager[2] += 3;
        }

        if (roundCount == 3)
        {
            scoresRound3Manager[2] += 3;
        }

        if (roundCount == 4)
        {
            scoresRound4Manager[2] += 3;
        }
    }

    public void Player3RunnerUp()
    {
        if (roundCount == 1)
        {
            scoresRound1Manager[2] += 1;
        }

        if (roundCount == 2)
        {
            scoresRound2Manager[2] += 1;
        }

        if (roundCount == 3)
        {
            scoresRound3Manager[2] += 1;
        }

        if (roundCount == 4)
        {
            scoresRound4Manager[2] += 1;
        }
    }   
}
