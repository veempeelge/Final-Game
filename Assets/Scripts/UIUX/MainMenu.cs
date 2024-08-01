using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
   
public class MainMenu : MonoBehaviour
{

    [SerializeField] AudioClip mainMenuMusic;

    private void Start()
    {
         //SoundManager.Instance.PlayMusic(mainMenuMusic);
    }

    public void Level1()
    {
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Lvl1()
    {
        SceneManager.LoadScene("PRO1.5_TestLevelPopulate");
    }

    public void Lvl2()
    {
        SceneManager.LoadScene("PRO_TestLvl.2");
    }

    public void Lvl3()
    {
        SceneManager.LoadScene("PRO_TestLvl.3");
    }

    public void LvlPicker()
    {
        SceneManager.LoadScene("LevelPicker");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
