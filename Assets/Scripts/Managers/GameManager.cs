using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEditor.Rendering;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public NavMeshSurface nav;

    int playersLeft;
    bool player1alive, player2alive, player3alive;

    [SerializeField] UnityEngine.UI.Button twoPlayers, threePlayers;

    [SerializeField] GameObject UIPlayerSelect, UIPlayerHP, UIGameOver;

    [SerializeField] GameObject player1obj, player2obj, player3obj;
    [SerializeField] GameObject player1Won, player2Won, player3Won;

    [SerializeField] GameObject[] player3UIs;

    [SerializeField] AudioClip gameplayMusic, buttonClick;

    [SerializeField] Transform hpBar2, weaponDurability2, item2;

    [SerializeField] GameObject player1deadText, player2deadText, player3deadText;

    [SerializeField] GameObject tutorialPage;


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
        if (tutorialPage != null)
        {
            tutorialPage.SetActive(false);
        }
        gameStart = true;
        SoundManager.Instance.PlayMusic(gameplayMusic);
        UIPlayerHP.SetActive(false);
        UIPlayerSelect.SetActive(true);
        UIGameOver.SetActive(false);
        Time.timeScale = 0;
        twoPlayers.onClick.AddListener(_2Players);
        threePlayers.onClick.AddListener(_3Players);
        player1deadText.SetActive(false);
        player2deadText.SetActive(false);
        player3deadText.SetActive(false);

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
        if (tutorialPage != null)
        {
            tutorialPage.SetActive(true);
        }
        SoundManager.Instance.Play(buttonClick);

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

        //weaponDurability2.position = new Vector3(1684.65f, weaponDurability2.position.y, weaponDurability2.position.z);
        //hpBar2.position = new Vector3(1648.833f, hpBar2.position.y, hpBar2.position.z);
        //item2.position = new Vector3(1591.637f, item2.position.y, item2.position.z);
        gameStart = false;
    }

    void _3Players()

    {

        if (tutorialPage != null)
        {
            tutorialPage.SetActive(true);
        }
        SoundManager.Instance.Play(buttonClick);

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
            StartCoroutine(GameOver());

            ScoreManager.Instance.Player1RunnerUp();

        }

        player1deadText.SetActive(true);
    }

    public void Player2Dead()
    {
        playersLeft--;
        player2alive = false;
        Debug.Log("Player1Dead");

        if (playersLeft == 1)
        {
            StartCoroutine(GameOver());

            ScoreManager.Instance.Player2RunnerUp();
        }

        player2deadText.SetActive(true);
    }

    public void Player3Dead()
    {
        playersLeft--;
        player3alive = false;
        Debug.Log("Player1Dead");
        player3deadText.SetActive(true);

        if (playersLeft == 1)
        {
            ScoreManager.Instance.Player3RunnerUp();
            StartCoroutine(GameOver());
        }
    }

    void StartGame()
    {
        UIPlayerHP.SetActive(true);
        UIPlayerSelect.SetActive(false);
        Time.timeScale = 1;
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1);
        gameOver = true;
        Debug.Log("GameOver");
        UIGameOver.SetActive(true);
        Time.timeScale = 0;

        if (player1alive)
        {
            //Player 1 Won
            ScoreManager.Instance.Player1Won();
            player1Won.SetActive(true);
            

        }

        if (player2alive)
        {
            //Player 2 Won
            ScoreManager.Instance.Player2Won();
            player2Won.SetActive(true);
           


        }

        if (player3alive)
        {
            //Player 3 Won
            ScoreManager.Instance.Player3Won();
            player3Won.SetActive(true);
            
        }
    }

    IEnumerator CamToWinner(CameraZoom cam, MovementPlayer1 mov)
    {
        yield return new WaitForSeconds(.3f);
        cam.PlayerDied(mov.gameObject.transform.position);
    }
}
