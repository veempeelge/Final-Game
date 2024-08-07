using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class TutorialSceneManager : MonoBehaviour
{
    [SerializeField] GameObject player1,player2,player3;
    bool player1moved,player2moved,player3moved = false;
    bool player1threw, player2threw, player3threw = false;

    [SerializeField] AudioClip buttonClick;

    bool waterCollected = false;
    bool phase1, phase2, phase3, phase4 = false;
    bool zombieSpawned, waterSpawned = false;

    Rigidbody player1movedrb, player2movedrb, player3movedrb;

    MovementPlayer1 player1Code,player2Code,player3Code;
    [SerializeField] GameObject waterGen;
    [SerializeField] GameObject zombie;
    [SerializeField] Transform[] zombieSpawnPoint, waterSpawnPoint;
    [SerializeField] GameObject tutorialScreen, tutorialScreen2, tutorialScreen3, tutorialScreen4;

    List<GameObject> waters = new List<GameObject>();
    List<GameObject> zombieList = new List<GameObject>();

    [SerializeField] Button next;
    [SerializeField] GameObject tutorialImage;
    [SerializeField] GameObject TutorialOverMarrrr;

    // Start is called before the first frame update
    void Start()
    {
        tutorialScreen.SetActive(false);

        next.onClick.AddListener(Next);
        tutorialScreen2.SetActive(false);
        tutorialScreen3.SetActive(false);
        tutorialScreen4.SetActive(false);

        player1movedrb = player1.GetComponent<Rigidbody>();
        player2movedrb = player2.GetComponent<Rigidbody>();
        player3movedrb = player3.GetComponent<Rigidbody>();

        player1Code = player1.GetComponent<MovementPlayer1>();
        player2Code = player2.GetComponent<MovementPlayer1>();
        player3Code = player3.GetComponent<MovementPlayer1>();
    }

    public void StartTutorialImage()
    {
        tutorialScreen.SetActive(true);
    }

    public void Next()
    {
        tutorialScreen.SetActive(true);
        tutorialImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()   
    {
        if (player1movedrb && player2Code!= null)
        {

            if (player1movedrb.velocity.magnitude > 4f)
            {
                player1moved = true;
            }
        }

        if (player2movedrb && player2Code != null)
        {
            if (player2movedrb.velocity.magnitude > 4f)
            {
                player2moved = true;
            }
        }


        if (player3movedrb && player3Code != null)
        {
            if (player3movedrb.velocity.magnitude > 4f)
            {
                player3moved = true;
            }
        }

        if (player3.activeSelf)
        {
            if (player1moved && player2moved && player3moved)
            {
                StartCoroutine(SpawnWater());
            }
        }
        else if (!player3.activeSelf)
        {
            if (player1moved && player2moved)
            {
                StartCoroutine(SpawnWater());
            }
        }
        

        if (phase2)
        {
            Invoke(nameof(CheckWater),3f);
        }

        if (phase3)
        {

            Invoke(nameof(WaterCheck),3f);
        }

        if (player1threw && player1threw && player1threw)
        {
            tutorialScreen.SetActive(false);
            tutorialScreen2.SetActive(false);
            tutorialScreen3.SetActive(false);
            tutorialScreen4.SetActive(true);
        }

    }

    private void WaterCheck()
    {
        if (player1Code.waterCharge < 3)
        {
            player1threw = true;
        }

        if (player2Code.waterCharge < 3)
        {
            player2threw = true;
        }

        if (player2Code.waterCharge < 3)
        {
            player2threw = true;
        }

    }

    IEnumerator SpawnWater()
    {
        if (waterSpawned == false)
        {
            Debug.Log("WaterSpawn");
            waterSpawned = true;
            tutorialScreen.SetActive(false);
            tutorialScreen2.SetActive(true);
            tutorialScreen3.SetActive(false);
            tutorialScreen4.SetActive(false);

            phase2 = true;
            yield return new WaitForSeconds(1f);

            // Clear the list to avoid adding more waterGen objects if SpawnWater is called again
            waters.Clear();

            for (int i = 0; i < waterSpawnPoint.Length; i++)
            {
                // Instantiate the waterGen prefab
                GameObject water = Instantiate(waterGen, waterSpawnPoint[i].position, waterSpawnPoint[i].rotation);

                // Add the instantiated waterGen to the waters list
                waters.Add(water);
            }

            yield break;
        }
       
    }

    void CheckWater()
    {
        for (int i = 0; i < waters.Count; i++)
        {
            if (waters[i] == null)
            {
                waters.Remove(waters[i]);
            }
        }

        if (waters.Count == 0)
        {
            Debug.Log(waters.Count);
            StartCoroutine(SpawnZombies());
            phase3 = true;
        }
    }

    IEnumerator SpawnZombies()
    {
        Debug.Log("ZombieSpawn");

        if (zombieSpawned == false)
        {
            zombieSpawned = true;
            tutorialScreen.SetActive(false);
            tutorialScreen2.SetActive(false);
            tutorialScreen3.SetActive(true);
            tutorialScreen4.SetActive(false);

            
            yield return new WaitForSeconds(5f);

            for (int i = 0; i < zombieSpawnPoint.Length; i++)
            {
                GameObject zombiesObject = Instantiate(zombie, zombieSpawnPoint[i].position, waterSpawnPoint[i].rotation);

                zombieList.Add(zombiesObject);
            }

            yield return new WaitForSeconds(30f);

            Time.timeScale = 0;
            TutorialOverMarrrr.SetActive(true);

            yield break;
        }
    }

    public void MainMenuFromTutorial()
    {
        Time.timeScale = 1;
        SoundManager.Instance.Play(buttonClick);
        SceneManager.LoadScene("Main Menu");
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        SoundManager.Instance.Play(buttonClick);
        SceneManager.LoadScene(5);
    }
}
