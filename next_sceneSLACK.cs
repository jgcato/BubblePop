using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class nextScene : MonoBehaviour {

    //logging variables
    public List<string> writeOut;
    private string dateTimeForm;
    private string logFilePath;

    private GameObject music;
    Logging log;
    List<string> listB;

    

    private void Start()
    {

        //logging
        List<string> writeOut = new List<string>();
        dateTimeForm = "yyMMdd_HHmmss";
        logFilePath = "Fitts";


        PlayerPrefs.GetInt("next level");
    }


    public void exitScene()
    {
        SceneManager.LoadScene("exit");
        //adding to list
        writeOut.Add("Game Completion: " + Time.unscaledTime.ToString());
        writeOut.Add("Date: " + DateTime.Now);
        MakeLogger(); //making list 
        Debug.Log("last log created");
    }

    public void expirement()
    {
        SceneManager.LoadScene("expirement");
    }

    //scenes
    public void nextLevel()
    {
        PlayerPrefs.GetInt("next level");


        if (PlayerPrefs.GetInt("next level") == 0)
        {
            SceneManager.LoadScene("Level 1(cali)");
          
        }

        if (PlayerPrefs.GetInt("next level") == 1)
        {
            SceneManager.LoadScene("level2(training)");
           
        }

        if (PlayerPrefs.GetInt("next level") == 2)
        {
            SceneManager.LoadScene("level3");
            
        }

        if (PlayerPrefs.GetInt("next level") == 3)
        {
            SceneManager.LoadScene("level4");
           
        }

        if (PlayerPrefs.GetInt("next level") == 4)
        {
            SceneManager.LoadScene("level5");
            
        }

        if (PlayerPrefs.GetInt("next level") == 5)
        {
            SceneManager.LoadScene("level6");

        }
        if (PlayerPrefs.GetInt("next level") == 6)
        {
            SceneManager.LoadScene("level7");

        }

        if (PlayerPrefs.GetInt("next level") == 7)
        {
            SceneManager.LoadScene("exit");
         
        }
    }

   public void stopPlay()
    {

        music = GameObject.Find("Bouncy_Fun_Song");

        Destroy(music);

    }


    //Methods for logging data
    private void OnDestroy()
    {
        MakeLogger();
    }

    /// <summary>
    /// This method allows us to write logs that we've continously stashed
    /// to the logger file that you will create
    /// </summary>
    public void MakeLogger()
    {
        // What this statement does:
        //  1) creates a streaming path 
        using (StreamWriter writer = new StreamWriter(logFilePath + DateTime.Now.ToString(dateTimeForm) + ".csv"))
        {
            foreach (string log in writeOut)
            {
                writer.WriteLine(log);
            }
        }
    }
}

