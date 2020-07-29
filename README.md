# BubblePop
This repsitory contains the scripts written for a Bubble Pop game I created in UNITY 2D. I created this game during a REU summer internship in a Human Computer Interaction Lab at the University of Florida. This read me document is for the game setting in UNITY as well as the individual scripts, methods, and logging. 

Bubble Pop Game 

UNITY...
-	All canvases in the game should have a canvas scaler component set to 1440x2960, the canvases are named level_score
-	Levels 2 – 6 have a level_score prefab that contains the following UI: level text, score text and the start button. 
-	The child background game objects attached to the bubble/movingCircle game object and the startButton are there to make the objects more visible to the user
-	The touchInput prefab is used to load the congrats scene. It is used in the combination script as load.loadCongrats()
-	The GameObject prefab, is the bubble that users will see in the game. This prefab has all the necessary scripts attached to it as well as necessary sprites. 
-	The startScript prefab is used in the levels to set the bubble game object invisible on start of the game and visible after the start button is pressed 
-	The nextScene prefab is used in the congrats scene and attached to the button’s onClick component to load the next level 
-	The shadow and New Sprite gameobjects are used to measure the distance between the bubbles, they would not affect game play if deleted.
 
SCRIPTS...

Combination Script: 
-	The dist stack holds the values for the distance the bubble will move. 
-	The size stack holds the values for the size the bubble will change to throughout the game level
-	The code below updates the score text and keeps it updating throughout the levels. The score only increments if the user has touched the bubble.
score = PlayerPrefs.GetInt("player score");
scoreText.text = "score: " + score;

startingLevel and startButton Script:
-	The starting level script is what makes the bubble gameObject disappear at the beginning (bubble.setActive(false)) of the level. The startButton script which is passed into the onClick component inside the UNITY inspector is what makes the bubble appear after the start button is pressed. Inside the startButton script the bubble is represented with the movingCircle gameobject.  

Level 2 Script:
- The script for level 2 is almost identical to the combination script. The difference between the two is that the level 2 script keeps the bubble’s size at .48 will the combination script has the bubble alternating between sizes in the stack. It is like this because Level 2 is a finger calibration level and the size of the bubble will not change. 

nextScene Script:
-	This script contains a couple methods for loading different scenes but is main purpose is to set which level will load next after the congrats scene appears at the end of the previous level. The if statements inside the nextLevel() method is where this all happens. If the playerPrefs is set to 1, level 2 will load. If the playerPrefs is set to 2, level 3 will load. It will continue like this until level 6. After level 6 the exit scene will load.

-	START: Inside start the values for the dist stack and the size stack are being set with the Push() method. The score text is being set to the end score of the previous level,  (score = PlayerPrefs.GetInt(“player score”) … ). The list that is being used for logging is being initialized in START as well, the name of the list is called writeOut. 

-	UPDATE: If the start button is destroyed, the code will start detecting if the user has touched the bubble or not. hit.collider != null, means the player has touched the bubble. hit.collider == null, means the player did not touch the bubble. 
hit.collider != null: if this “if” statement is entered. A pop sound will play (pop.Play()), the score will increment, information will be added to the log (AddLog()) and the top items of the stack will be popped (dist.Pop(), sizes.Pop()). The the code will enter into the newCombination method. This will continue to happen inside the if statement until the stacks have a count of zero. When the stacks have a count of zero, the congrats scene will load and a log will be made with the MakeLogger() method. 
hit.collider == null: when this “if” statement is entered an error sound will play. A log will be added with AddLog() and the distance and size that was missed will be pushed back onto the stack ( dist.Push(dist.Peek()),sizes.Push(sizes.Peek())  ) for the user to touch again.

METHODS...

-	Enumerator ExecuteAfterTime(float time): this Coroutine creates a 1 second delay before the congrats scene of the game is loaded. Without this, the scene was loading too fast for the user.The method is called with StartCoroutine(ExecuteAfterTime(1)).
-	newCombination(float distance, float size): New combination takes a float value of distance from the dist stack and a size value from the sizes stack. Inside new combination the size of the bubble changes and a new (X,Y) coordinate is being found and the bubble will move to that new coordinate. To find a new (X,Y) coordinate a random X2 values is first found and that value is passed into the Y2, Y3 methods. If the random X2 value and Y2 or Y3 values create a coordinate that is in the bounds of the screen, the bubble will be moved to that point. If the new point is out of range the code will find a random Y2 value and pass that value into the X2 and X3 methods. If the random Y2 and X2 or X3 values gives us a point that is on screen, the bubble will be moved to that point. If not, the code will stay in the while loop until a point that is in range of the screen is found. The random X2 and Y2 values both have to be less than the distance when subtract from X1 and Y2 respectively. 
-	public float y2value(float distance, float x2): This method calculates a y2 value that will be used in the new X,Y coordinate.
-	public float y3Value(float distance, float x2): this method calculates a y3 value that will be used for the new X,Y coordinate in case Y2 gives a point that is out of range. 
-	public float x2Value(float distance, float newY): this points calculates a new X2 value that will be used in the new coordinate for the bubble.
-	public float x3Value(float distance, float newY): this method calculates a new X value that will be used in the new coordinate point in case the X2 value is out of range.
-	public bool inRange(Vector3 XYposition, float radius): a vector3 point and radius value (r) is passed into this method to check that the point is in range of the screen. This point checks that X,Y coordinate added and subtract by the given radius to insure that the whole sprite is on the screen.
-	public Vector2 convertToUnits(float x, float y): this method takes two float values that represent X and Y and converts those values into UNITY units. Up until the method is called the values for the new X,Y coordinate are in pixels. 
-	public void moveBubble(Vector2 position): this method is what moves the bubble to it’s new position. Also inside the method, the old X2, Y2 values are being recycled into X1, Y1. 

Logging... 

The data in this game is log by using a list. During the game information like the touch position, bubble position, distance, time, etc. will be added to a list called writeOut as a string. At the completion of the game the MakeLogger() method is called and exports everything from the list into a .csv file.  
 Methods:
 -	AddLog(): everytime the button is hit or missed, addLog is called and it log the information of the game. When the start button is pressed AddLog, excutes the first if   statement in the method which logs information of the touch of the start button, its position, etc. After the start button is gone and the bubble has appeared, AddLog() will only execute the second if statement which is logging information about the bubble.

PlayerPrefs...
-	Each level has a playerPrefs for score and for loading the next scene. Levels 3-6 have a separate script attached to them for the next scene playerPref. These scripts are named level 3, level 4, level 5 and level 6. The playerPref for score is inside each script and this allows the score to continue incrementing throughout the different scenes. 
Music Script:
-	The music script allows the song at the beginning of the game to keep playing in the start scene and instruction scene without having to reload. 
touchInput Script:
-	The touchInput script is a script full of methods that load different scenes using SceneManager.LoadScene().



