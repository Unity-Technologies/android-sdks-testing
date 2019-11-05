//This script detects when the player reaches the goal zone and then tells
//the Game Manager about it

using UnityEngine;

public class GoalZone : MonoBehaviour 
{
	bool isActive = true; //Is the goal currently active?


	void OnTriggerEnter(Collider other)
	{
		//If the goal isn't currently active OR the object entering 
		//the goal isn't the player, leave
		if (!isActive || !other.CompareTag("Player"))
			return;

		//Since the player entered the goal, it is no longer active. This
		//prevents us from trying to win multiple times
		isActive = false;

		//If the GameManager exists, tell it that the player entered the goal
		if(GameManager.instance != null)
			GameManager.instance.PlayerEnteredGoalZone();
	}
}
