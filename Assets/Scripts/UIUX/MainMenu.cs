using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
   
public class MainMenu : MonoBehaviour
{

    [SerializeField] AudioClip mainMenuMusic;

    private void Start()
    {
         SoundManager.Instance.PlayMusic(mainMenuMusic);
    }

    public void Level1()
    {
        SceneManager.LoadScene(2);
    }
}
