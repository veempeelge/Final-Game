using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEditor.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public NavMeshSurface nav;

    int playersLeft;
    bool player1alive, player2alive, player3alive;

    [SerializeField]  Button twoPlayers, threePlayers;

    [SerializeField] GameObject UIPlayerSelect, UIPlayerHP, UIGameOver;

    [SerializeField] GameObject player1obj, player2obj, player3obj;
    [SerializeField] GameObject player1Won, player2Won, player3Won;

    [SerializeField] GameObject[] player3UIs;

    [SerializeField] AudioClip gameplayMusic;

    [SerializeField] Transform hpBar2, weaponDurability2, item2;


    public float
        defPlayerAtk,
        defPlayerAtkSpd,
        defPlayerRange,
        defPlayerAtkWidth,
        defPlayerKnockback,
        defSpeed,
        defAcc;
    private bool gameStart;
    private bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        gameStart = true;
        SoundManager.Instance.PlayMusic(gameplayMusic); 
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
       if (gameStart)
       {
          if (Input.GetKeyDown(KeyCode.Alpha2))
          {
                _2Players();
          }

          if (Input.GetKeyDown(KeyCode.Alpha3))
          {
                _3Players();
          }
        }

       if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
    }

    void _2Players()
    {
        player1alive = true;
        player2alive = true;
        player3alive = false;

        playersLeft = 2;
        StartGame();
        player3obj.SetActive(false);
        for (int i = 0; i < player3UIs.Length; i++)
        {
            player3UIs[i].SetActive(false);
        }

        weaponDurability2.position = new Vector3(1684.65f, weaponDurability2.position.y, weaponDurability2.position.z);
        hpBar2.position = new Vector3(1648.833f, hpBar2.position.y, hpBar2.position.z);
        item2.position = new Vector3(1591.637f, item2.position.y, item2.position.z);
        gameStart = false;
    }

    void _3Players()
    {
        player1alive = true;
        player2alive = true;
        player3alive = true;
        playersLeft = 3;
        StartGame();

        gameStart = false;

    }

    public void Player1Dead()
    {
        playersLeft--;
        player1alive = false;
        Debug.Log("Player1Dead");
        if (playersLeft == 1)

        {
            GameOver();
            ScoreManager.Instance.Player1RunnerUp();

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
            ScoreManager.Instance.Player2RunnerUp();
        }

    }

    public void Player3Dead()
    {
        playersLeft--;
        player3alive = false;
        Debug.Log("Player1Dead");

        if (playersLeft == 1)
        {
            ScoreManager.Instance.Player3RunnerUp();
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
        MovementPlayer1 mov;
        CameraZoom cam;
        gameOver = true;
        Debug.Log("GameOver");
        UIGameOver.SetActive(true);
        Time.timeScale = 0;

        if (player1alive)
        {
            //Player 1 Won
            ScoreManager.Instance.Player1Won();
            player1Won.SetActive(true);
            mov = player1obj.gameObject.GetComponent<MovementPlayer1>();
            mov.Victory();
            cam = mov.gameObject.GetComponent<CameraZoom>();
            StartCoroutine(CamToWinner(cam, mov));

        }

        if (player2alive)
        {
            //Player 2 Won
            ScoreManager.Instance.Player2Won();
            player2Won.SetActive(true);
            mov = player2obj.gameObject.GetComponent<MovementPlayer1>();
            mov.Victory();
            cam = mov.gameObject.GetComponent<CameraZoom>();
            StartCoroutine(CamToWinner(cam, mov));


        }

        if (player3alive)
        {
            //Player 3 Won
            ScoreManager.Instance.Player3Won();
            player3Won.SetActive(true);
            mov = player3obj.gameObject.GetComponent<MovementPlayer1>();
            mov.Victory();
            cam = mov.gameObject.GetComponent<CameraZoom>();
            StartCoroutine(CamToWinner(cam, mov));
        }
    }

    IEnumerator CamToWinner(CameraZoom cam, MovementPlayer1 mov)
    {
        yield return new WaitForSeconds(.3f);
        cam.PlayerDied(mov.gameObject.transform.position);
    }
}
