using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Sprites;

public class level3 : MonoBehaviour {
    
    public GameObject sqaure;

    //sound variables
    public AudioSource error;
    public AudioSource pop;

    private int hitNum = 0; //keeps track of # of hits for score

    public touchInput load; //will be accessing load scene methods from "touchInput" class with this object

    private int scoreNum = 26;
    public Text score;

    //these array values will hold the values for Vector2 positoining
    public float[] Xposition = new float[10];
    public float[] Yposition = new float[10];
    private int positionOfSquare = 0; //variable to hold index of array



    //holds different sizes for the targets
    public float[] size = new float[10];
    Transform parentOfSqaure;
    private int sizeOfSqaure;

    //this coroutine creates a delay
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        //congrats screen loads after player scored 10 points 
        load.loadCongrats();
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if statement checks if player has touched the screen
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {  

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
            //if statement uses raycasting to check if player touched to object
            if (hit.collider != null)
            {
                changeSize();
                changePosition();
                Debug.Log(Camera.main.ScreenPointToRay(Input.GetTouch(0).position));
                pop.Play();
                hitNum++;                             
                scoreNum++;
                score.text = "Score: " + scoreNum.ToString();
                PlayerPrefs.SetInt("player score", scoreNum);
                PlayerPrefs.SetInt("next level", 3);


                if (hitNum == 10)
                {
                    StartCoroutine(ExecuteAfterTime(1)); //calling IEnumerator method that creates a delay
                }



            }

            if (hit.collider == null)
                error.Play();
            Debug.Log(Camera.main.ScreenPointToRay(Input.GetTouch(0).position));
            

        }


        
    }

    public void changePosition()
    {

        sqaure.transform.position = new Vector3(Xposition[positionOfSquare], Yposition[positionOfSquare], 0);
        positionOfSquare++;

    }

    public void changeSize()
    {
        int random = (UnityEngine.Random.Range(0, 3));

        //changes size of sqaure
        sqaure.transform.SetParent(parentOfSqaure);
        sqaure.transform.localScale = new Vector3(size[random], size[random], 0);

    }



    

   

}


