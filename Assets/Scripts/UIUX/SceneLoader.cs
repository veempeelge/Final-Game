using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    private void Start()
    {

    }

    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
