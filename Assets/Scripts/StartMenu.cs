using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    
 

    public void loadMainScene()
    {
        SceneManager.LoadScene("main_scene");

    }

    public void loadSettingsScene()
    {
        SceneManager.LoadScene("settings_scene");

    }

    public void quitGame()
    {
        Application.Quit();
    }



}
