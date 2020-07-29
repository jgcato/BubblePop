using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class playerInfoSceneLog : MonoBehaviour {

    //logging variables
    public List<string> writeOut;
    private string dateTimeForm;
    private string logFilePath;
    public Text age;
    public Toggle female;
    Toggle male;
    string sex = "male";
    Button button;
 

    // Use this for initialization
    void Start () { 

        List<string> writeOut = new List<string>();
        dateTimeForm = "yyMMdd_HHmmss";
        logFilePath = "Fitts";
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void femaleToggle()
    {
        sex = "female";
    }


    /// <summary>
    /// This method allows us to write logs that we've continously stashed
    /// to the logger file that you will create
    /// </summary>
    public void MakeLogger()
    {
       
        writeOut.Add("Player Info: " + age.text + "    " + sex);
        writeOut.Add("Date/Time: " + DateTime.Now); 

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
