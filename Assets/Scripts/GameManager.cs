//This script handles the logic and UI for our game. It controls how much time the player has,
//how many points they have scored, and it detects when the player has won or lost the game

using UnityEngine;
using UnityEngine.UI;				//Enable UI items in script
using UnityEngine.SceneManagement;	//Enable scene management in script

public class GameManager : MonoBehaviour 
{
	//This class contains a public static reference to itself. This means that it 
	//will be accessible to other classes globally, even if they don't have a 
	//reference or link to it. 
	public static GameManager instance;

	[Header("Game Properties")]
	public int scoreToWin = 4;		//Amount of points the player needs to lower the wall
	public float timeAmount = 60f;	//How long the player has to reach the goal
	public WallMover wall;			//A reference to the wall script that lowers it

	[Header("UI Elements")]
	public Text timeText;			//The UI element that shows the amount of time
	public Text collectText;		//The UI element that shows the player's score
	public GameObject winPanel;		//The panel that will pop up when the player wins	
	public GameObject lossPanel;	//The panel that will pop up when the player loses

	int score;		//Player's current score
	bool gameover;	//Is the game over?


	void Awake()
	{
		//If there currently isn't a GameManager, make this the game manager. Otherwise,
		//destroy this object. We only want one GameManager
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	void Start () 
	{
		//Set our initial UI text for score and the amount of time
		collectText.text = score + " / " + scoreToWin;
		timeText.text = ((int)timeAmount).ToString();
	}

	void Update () 
	{
		//Always look to see if the player is pressing the "Cancel" button (escape). If so,
		//quit the game
		if (Input.GetButtonDown ("Cancel"))
			ExitGame ();

		//If the game is already over, don't do anything (just leave)
		if (gameover)
			return;

		//Reduce the player's time
		timeAmount -= Time.deltaTime;

		//If the player's time is now at or below zero...
		if (timeAmount <= 0f) 
		{
			//...set the time to zero...
			timeAmount = 0f;
			//...record that the game is now over...
			gameover = true;
			//...and show the Loss Panel
			lossPanel.SetActive (true);
		}

		//Update the UI to show the remaining time
		timeText.text = ((int)timeAmount).ToString();
	}
		
	//This method is called from the CollectableSpawner when the player picks up a shield
	public void PlayerScored()
	{
		//If the player already has enough points, leave. This prevents the score from
		//becoming larger than the needed score in the UI (for instance, 5 out of 4), and also
		//prevents us from trying to lower the wall multiple times
		if (score >= scoreToWin)
			return;

		//Increase the player's score and update the UI
		score++;
		collectText.text = score + " / " + scoreToWin;

		//If the player hasn't scored eough points yet, leave
		if (score < scoreToWin)
			return;

		//Lower the wall
		wall.LowerWall ();
	}

	//This method is called when the player enters the goal area
	public void PlayerEnteredGoalZone()
	{
		//The game is now over
		gameover = true;
		//Show the Win Panel
		winPanel.SetActive (true);
	}

	//This method is called from the Player's script. We only want the player to be
	//able to move if the game isn't over
	public bool IsGameOver()
	{
		//Return whether or not the game is over
		return gameover;
	}

	//This methid is called from the AdManager when the player watches a rewarded ad
	public void AddMoreGameTime(float amount)
	{
		//Add the given amount of time to the player's time
		timeAmount += amount;

		//If the game is already over, it is no longer over, so that the player
		//can keep trying
		if (gameover)
			gameover = false;
	}

	//This method is called from the "Play Again" buttons in the UI
	public void ReloadScene()
	{
		//Reload the current scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	//This method allows the player to exit the game either by pressing the correct key or
	//selecting to exit the game from the UI
	public void ExitGame()
	{
		//Check to see if we are in the editor, and if we are simply stop playing
		//If we are not in the editor, then we are in a build and we need to tell the application
		//to Quit. NOTE: This does not work on all platforms. Some platforms only allow the OS to
		//terminate applications and in those cases Application.Quit() won't do anything
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false; 
		#else
			Application.Quit();
		#endif
	}
}
