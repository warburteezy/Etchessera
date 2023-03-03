using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class SettingsManagerScript : MonoBehaviour
{

    public static string whiteArmyPreset = "________PPPPPPPPRNBQKBNR";
    public static string blackArmyPreset = "________PPPPPPPPRNBKQBNR";

    public void setWhiteArmyPreset()
    {
        GameObject whiteInputObject = GameObject.Find("WhiteArmyInput");
        TMP_InputField whiteTextField = whiteInputObject.GetComponent <TMP_InputField>();
        whiteArmyPreset = whiteTextField.text;

    }

    public void setBlackArmyPreset()
    {
        GameObject blackInputObject = GameObject.Find("BlackArmyInput");
        TMP_InputField blackTextField = blackInputObject.GetComponent<TMP_InputField>();
        blackArmyPreset = blackTextField.text;
    }

    public void randomizeWhiteArmyPreset()
    {
        GameObject whiteInputObject = GameObject.Find("WhiteArmyInput");
        TMP_InputField whiteTextField = whiteInputObject.GetComponent<TMP_InputField>();
        string temp = generateRandomPresetString();
        whiteArmyPreset = temp;
        whiteTextField.text = temp;

    }

    public void randomizeBlackArmyPreset()
    {
        GameObject blackInputObject = GameObject.Find("BlackArmyInput");
        TMP_InputField blackTextField = blackInputObject.GetComponent<TMP_InputField>();
        string temp = generateRandomPresetString();
        blackArmyPreset = temp;
        blackTextField.text = temp;
    }

    public string generateRandomPresetString()
    {
        string temp = "________"; 
        for(int i = 0; i < 8; i++)
        {
            if(Random.Range(1.0f, 3.0f) < 2.3f)
            {
                temp = temp + "P";
            }
            else
            {
                temp = temp + "A";
            }

        }
        if(Random.Range(1.0f, 3.0f) < 1.8f)
        {
            temp = temp + "B_UUKUU_";
        }
        else if (Random.Range(1.0f, 3.0f) < 1.8f){
            temp = temp + "ANNQKQNN";
        }else if (Random.Range(1.0f, 3.0f) < 1.8f){
            temp = temp + "_QKCCUR_";
        }else if (Random.Range(1.0f, 3.0f) < 1.8f){
            temp = temp + "RNBQKBNR";
        }else
        {
            temp = temp + "KQRRNNBB";
        }


        return temp;


    }


    public string getWhiteArmyPreset()
    {
        return whiteArmyPreset;  
    }

    public string getBlackArmyPreset()
    {
        return blackArmyPreset;
    }

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.IndexOf("settings") > -1)
        {
            GameObject whiteInputObject = GameObject.Find("WhiteArmyInput");
            // Get the component
            TMP_InputField whiteTextField = whiteInputObject.GetComponent<TMP_InputField>();
            // To get the text
            whiteTextField.text = whiteArmyPreset;


            GameObject blackInputObject = GameObject.Find("BlackArmyInput");
            // Get the component
            TMP_InputField blackTextField = blackInputObject.GetComponent<TMP_InputField>();
            // To get the text
            blackTextField.text = blackArmyPreset;

        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadStartMenuScene()
    {
        SceneManager.LoadScene("Start Menu Scene");

    }

    public void quitGame()
    {
        Application.Quit();
    }



}
