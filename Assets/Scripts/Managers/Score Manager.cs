using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int[] playerScores;
    private List<int[]> roundScores;
    private int gameCounter;
    private const int totalGames = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeScores()
    {
        playerScores = new int[3];
        roundScores = new List<int[]>();
        for (int i = 0; i < playerScores.Length; i++)
        {
            playerScores[i] = 0;
        }
        gameCounter = 0;
    }

    public void UpdateScores(int winnerIndex, int runnerUpIndex)
    {
      
        playerScores[runnerUpIndex] += 1;

       
        roundScores.Add((int[])playerScores.Clone());

        gameCounter++;
        if (gameCounter < totalGames)
        {
            LoadNextLevel();
        }
        else
        {
            LoadResultScene();
        }
    }

    private void LoadNextLevel()
    {
     
        bool isTie = (playerScores[0] == playerScores[1] || playerScores[1] == playerScores[2] || playerScores[0] == playerScores[2]);
        string nextLevel = isTie ? GetRandomLevel() : GetNextLevel();

     
        SceneManager.LoadScene(nextLevel);
    }

    private string GetNextLevel()
    {
     
        int nextLevelIndex = gameCounter + 1;
        return "Level" + nextLevelIndex;
    }

    private string GetRandomLevel()
    {
       
        int randomLevelIndex = Random.Range(1, totalGames + 1);
        return "Level" + randomLevelIndex;
    }

    private void LoadResultScene()
    {
       
        SceneManager.LoadScene("ResultScene");
    }

    public int GetScore(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < playerScores.Length)
        {
            return playerScores[playerIndex];
        }
        return 0;
    }

    public int[] GetRoundScores(int roundIndex)
    {
        if (roundIndex >= 0 && roundIndex < roundScores.Count)
        {
            return roundScores[roundIndex];
        }
        return null;
    }

    public int GetRoundCount()
    {
        return roundScores.Count;
    }

    public void ResetScores()
    {
        InitializeScores();
    }
}