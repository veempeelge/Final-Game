using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    int playersLeft;
    bool player1alive, player2alive, player3alive;

    [SerializeField]  Button twoPlayers, threePlayers;

    [SerializeField] GameObject UIPlayerSelect, UIPlayerHP, UIGameOver;

    [SerializeField] GameObject player1obj, player2obj, player3obj;
    [SerializeField] GameObject player1Won, player2Won, player3Won;

    [SerializeField] GameObject player3HPBar;

    public float
        defPlayerAtk,
        defPlayerAtkSpd,
        defPlayerRange,
        defPlayerAtkWidth,
        defPlayerKnockback,
        defSpeed,
        defAcc;

    // Start is called before the first frame update
    void Start()
    {
        UIPlayerHP.SetActive(false);
        UIPlayerSelect.SetActive(true);
        UIGameOver.SetActive(false);
        Time.timeScale = 0;
        twoPlayers.onClick.AddListener(_2Players);
        threePlayers.onClick.AddListener(_3Players);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void _2Players()
    {
        player1alive = true;
        player2alive = true;
        player3alive = false;

        playersLeft = 2;
        StartGame();
        player3obj.SetActive(false);
        player3HPBar.SetActive(false);
    }

    void _3Players()
    {
        player1alive = true;
        player2alive = true;
        player3alive = true;
        playersLeft = 3;
        StartGame();

    }
        
    public void Player1Dead()
    {
        playersLeft--;
        player1alive = false;
        Debug.Log("Player1Dead");
        if (playersLeft == 1)

        {
            GameOver();
        }
    }

    public void Player2Dead()
    {
        playersLeft--;
        player2alive = false;
        Debug.Log("Player1Dead");

        if (playersLeft == 1)

        {
            GameOver();
        }

    }

    public void Player3Dead()
    {
        playersLeft--;
        player3alive = false;
        Debug.Log("Player1Dead");

        if (playersLeft == 1)
        {
            GameOver();
        }
    }

    void StartGame()
    {
        UIPlayerHP.SetActive(true);
        UIPlayerSelect.SetActive(false);
        Time.timeScale = 1;
    }
    void GameOver()
    {
        Debug.Log("GameOver");
        UIGameOver.SetActive(true);
        Time.timeScale = 0;
        if (player1alive)
        {
            //Player 1 Won
            player1Won.SetActive(true);
        }

        if (player2alive)
        {
            //Player 2 Won
            player2Won.SetActive(true);

        }

        if (player3alive)
        {
            //Player 3 Won
            player3Won.SetActive(true);

        }
    }
}
