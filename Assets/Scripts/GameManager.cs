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

    [SerializeField] GameObject UIPlayerSelect;
    [SerializeField] GameObject UIPlayerHP;

    [SerializeField] GameObject player1obj, player2obj, player3obj;

    // Start is called before the first frame update
    void Start()
    {
        UIPlayerHP.SetActive(false);
        UIPlayerSelect.SetActive(true);
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
        playersLeft = 2;
        StartGame();
        player3obj.SetActive(false);
    }

    void _3Players()
    {
        playersLeft = 3;
        StartGame();

    }
        
    public void Player1Dead()
    {
        playersLeft--;
        player1alive = false;
        if (playersLeft < 0)
        {
            GameOver();
        }
    }

    public void Player2Dead()
    {
        playersLeft--;
        player2alive = false;
        if (playersLeft < 0)
        {
            GameOver();
        }

    }

    public void Player3Dead()
    {
        playersLeft--;
        player3alive = false;
        if (playersLeft < 0)
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
       if (player1alive)
        {
            //Player 1 Won
        }

        if (player2alive)
        {
            //Player 2 Won
        }

        if (player2alive)
        {
            //Player 2 Won
        }
    }
}
