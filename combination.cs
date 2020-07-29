using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class combination : MonoBehaviour {

    //varaibles for logging the data
    public List<string> writeOut;
    private string dateTimeForm;
    private string logFilePath;
    Touch touch;
    float secondsBtwnTouch; //will store the seconds between touch
    Logging log; 
    Vector2 bubblePostion;
    Vector2 touchPosition;

    string targetHit;

    //variables to load congrats scene
    public touchInput load;
    public GameObject startText;
    public Text scoreText;
    public Text levelText;
    int score = 0; 

    //used to change sprite from start button to bubble 
    public SpriteRenderer spriteRenderer;
    public Sprite bubbleSprite;
    public Sprite startSprite;
    public GameObject shadow;

    public Camera mainCamera; 

    public GameObject movingCircle;

    //arrays that hold to circle sizes and two distances
    int counter = 0;
    float size;

    ////stacks that store the distance and circle size
    Stack<float> dist = new Stack<float>();
    Stack<float> sizes = new Stack<float>();

    //variables for x2value() and y2value() methods
    float d;
    float d2;
    float x1;
    float y1;
    float x2;
    float y2;
    float y3;
    float x3;

    Vector3 point;
    Vector3 point2;

    private Sprite lastSprite;
    float r;

    private void Start()
    {
        List<string> writeOut = new List<string>();

        dateTimeForm = "yyMMdd_HHmmss";
        logFilePath = "Fitts";

        writeOut.Add("Seconds between Touch, Levels, Bubble Size, Distance, Bubble Postion X, Bubble Position Y, Unit X, Unit Y, Touch Position X, Unit X, Unit Y, Touch Position Y, Hit/Missed");

        //setting distances values
        dist.Push(40f);   //1
        dist.Push(40f);   //2
        dist.Push(60f);   //3
        dist.Push(60f);   //4
        dist.Push(40f);   //5
        dist.Push(60f);   //6
        dist.Push(40f);   //7 
        dist.Push(60f);   //8 
        dist.Push(40f);   //9
        dist.Push(60f);   //10
        dist.Push(60f);   //11    
        dist.Push(40f);   //12 
        dist.Push(40f);   //13


        //setting size values
        sizes.Push(0.48f);   //1
        sizes.Push(1.44f);   //2
        sizes.Push(0.96f);   //3
        sizes.Push(1.44f);   //4
        sizes.Push(0.48f);   //5
        sizes.Push(0.96f);   //6
        sizes.Push(1.44f);   //7
        sizes.Push(0.48f);   //8
        sizes.Push(0.96f);   //9
        sizes.Push(0.48f);   //10
        sizes.Push(1.44f);   //11
        sizes.Push(0.96f);   //12
        sizes.Push(0.96f);   //13


        spriteRenderer.sprite = startSprite; //sets sprite to start button

        //getting (x,y) of start button for x1 and y1 in equation
        Vector3 s;
        s = mainCamera.WorldToScreenPoint(new Vector3(movingCircle.transform.position.x, movingCircle.transform.position.y, 0));
        x1 = s.x;
        y1 = s.y;
        Debug.Log("world to screen: " + x1 + "," + y1);

        //Vector3 s;
        //s = mainCamera.ScreenToWorldPoint(new Vector3(200, 20, 0));
        //x1 = s.x;
        //y1 = s.y;
        //print(s.x);
        //print(s.y);

    }

    // Update is called once per frame
    private void Update()
    {
       
     
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {

            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);

            //if statement uses raycasting to check if player touched to object
            if (hit.collider != null)
            {
                startText.SetActive(false); //makes start text go away when game starts
                spriteRenderer.sprite = bubbleSprite; //sets sprite to bubble

                //changing position and size w/ newCombination method
                newCombination(dist.Pop(),sizes.Pop());
                counter++;
                score++;
                scoreText.text = "score: " + score.ToString();
                targetHit = "hit";
            }

            //moves to next scene
            if (sizes.Count == 0 || dist.Count == 0) 
            {
                load.loadCongrats();
                writeOut.Add("Seconds between Touch, Levels, Bubble Size, Distance, Bubble Postion X, Bubble Position Y, Touch Position X, Touch Position Y, B Unit X, B Unit Y, T Unit X, T Unit Y, Hit/Missed");
                writeOut.Add("Completion time: " + Time.timeSinceLevelLoad.ToString());
                writeOut.Add("Date: " + DateTime.Now);
                MakeLogger();
                Debug.Log("log created");
            }

            //if circle is missed 
            if (hit.collider == null)
            {
                dist.Push(dist.Peek());
                sizes.Push(sizes.Peek());
                targetHit = "missed";
            }

            //continues to add game data while items are still in the stacks
            if(score > 1)                            //(sizes.Count != 0 && dist.Count != 0)
            {
                AddLog();
            }

        }
    }

    public void newCombination(float distance, float size)
    {
        //updates radius
        if (size == .48f)
        {
            r = .24f;
        }
        else if (size == .96f)
        {
            r = .48f;
        }
        else
            r = .72f;


        movingCircle.transform.localScale = new Vector3(size, size, 0); //changing size  

        //getting x,y coordinate
        x2 = x1 + (r*100); 
        y2 = y2value(distance, x2); 
        y3 = y3Value(distance,x2); //2nd y value if y2 is off screen

        //storing vector3 position into variables
        point = new Vector3(x2, y2, 0);
        point2 = new Vector3(x2, y3, 0);

        //finds y2 = y1 +r and new x2 value if points out of range
        if(inRange(point,r) == false && inRange(point2,r) == false)     
        { 
            y2 = y1 + r; //new y2 value
            x2Value(y2);
            x3Value(y2); //second x value if x2 is off screen

            //updating points with x2 and x3 values 
            point = new Vector3(x2, y2, 0);
            point2 = new Vector3(x3, y2, 0);

            //converting from pixel to units
            if (inRange(point,r) == true)
            {
                bubblePostion = point; //for logging data 
                //recycling x2,y2 values into x1,y1 
                x1 = x2;
                y1 = y2;
                point = convertToUnits(point.x, point.y);//converting to unity units
                movingCircle.transform.position = point; //moving bubble 
                
                Instantiate(shadow, point, Quaternion.identity);
                Debug.Log("x2, y2: " + point.x + " , " + point.y);
            }
            else if (inRange(point2, r) == true)
            {
                bubblePostion = point2;  
                x1 = x3;
                y1 = y2;
                point = convertToUnits(point2.x, point2.y);
                movingCircle.transform.position = point;
                Instantiate(shadow, point, Quaternion.identity);
                Debug.Log("x2, y2: " + point.x + " , " + point.y);
            }

        }
      
        
        //converting from pixel to units
        if (inRange(point,r) == true)
        {
            bubblePostion = point;
            x1 = x2;
            y1 = y2;
            point = convertToUnits(point.x, point.y);
            movingCircle.transform.position = point;
           
            Instantiate(shadow, point, Quaternion.identity);
            Debug.Log("x2, y2: " + point.x + " , " + point.y);
        }
        else if (inRange(point2,r) == true)
        {
            bubblePostion = point2;
            x1 = x2;
            y1 = y3;
            point = convertToUnits(point2.x, point2.y);
            movingCircle.transform.position = point;
            Instantiate(shadow, point, Quaternion.identity);
            Debug.Log("x2, y2: " + point.x + " , " + point.y);
        }


    }


    //method calculates y2 to plug into x2 equation
    public float y2value(float distance, float xTwo)
    {
                d = Screen.dpi / 25.4f * distance;
                d2 = (float)Math.Pow(d, 2);
                Debug.Log("distance: " + d);

        //calulating y2 
        float xT = xTwo;
        Debug.Log("x2 in y2 method: " + xT);
        y2 = y1 - ((float)Math.Sqrt(-Math.Pow(x1, 2) + (2 * (x1) * (xT)) - Math.Pow(xT, 2) + d2));
        
        return y2;
    }


    public float y3Value(float distance, float xTwo)
    {
        d = Screen.dpi / 25.4f * distance;
        d2 = (float)Math.Pow(d, 2);
        Debug.Log("distance: " + distance);

        float xT = xTwo;
        Debug.Log("x2 in Y3 method: " + xT);
        y3 = y1 + ((float)Math.Sqrt(-Math.Pow(x1, 2) + (2 * (x1) * (xT)) - Math.Pow(xT, 2) + d2));
        return y3;
    }

    //method calucates x2 using y2 value returned from y2value() method
    public float x2Value(float newY)
    {
        float yTwo;
        yTwo = newY;
        x2 = x1 - ((float)Math.Sqrt(-Math.Pow(y1, 2) + (2 * (y1) * (yTwo)) + d2 - Math.Pow(yTwo, 2)));

        return Math.Abs(x2);
    }

    public float x3Value(float newY)
    {
        float yTwo;
        yTwo = newY;
        x3 = x1 + ((float)Math.Sqrt(-Math.Pow(y1, 2) + (2 * (y1) * (yTwo)) + d2 - Math.Pow(yTwo, 2)));

        return Math.Abs(x3);
    }

    //value of true means point is in range,
    //false is out of range
    public bool inRange(Vector3 XYposition, float radius)
    {
        float R;
        R = radius*100;

        float X = XYposition.x;
        float Y = XYposition.y;

        Debug.Log("pixel x: " + X);
        Debug.Log("pixel y: " + Y);


        if ((X+R > 0) && (Y+R > 0) && (X-R > 0) && (Y-R > 0) && (X+R < Screen.width) && (Y+R < Screen.width) && (X-R < Screen.width) && (Y-R < Screen.height))
        {
            Debug.Log("in range");
            return true;
        }
        else
        {
            Debug.Log("out of range");
            return false;
        }
       
    }

    public Vector2 convertToUnits(float x, float y)
    {
        Vector3 p;
        p = mainCamera.ScreenToWorldPoint(new Vector3(x, y, 0));
        return new Vector2(p.x, p.y);
    }






    /// <summary>
    /// Methods for logging data
    /// </summary>


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

    public void AddLog()
    {
        touch = Input.GetTouch(0);
        writeOut.Add( Time.deltaTime + ", " + levelText.text + ", " + sizes.Peek() + ", " + dist.Peek() + ", " + bubblePostion + "," + touch.position + ", " + convertToUnits(bubblePostion.x, bubblePostion.y) + ", " + convertToUnits(touch.position.x, touch.position.y) + "," + targetHit);
    }

}




