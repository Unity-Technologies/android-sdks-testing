//This script handles lowering the wall. Additionally, the functionality exists the also raise the
//wall (even though we don't use this in the game). The wall will be lowered over time instead of 
//instantly "teleporting" which makes it more cinematic

using UnityEngine;
using System.Collections;

public class WallMover : MonoBehaviour 
{
	public float moveDistance = 3f;	//Distance the wall moves up or down
	public float moveSpeed = 2f;	//Speed the wall moves up or down
	public bool raised = true;		//Is the wall currently in the "raised" position?

	AudioSource audioSource;	//Reference to the wall's audio source
	Vector3 targetPosition;		//Position the wall wants to move to
	bool moving = false;		//Is the wall currently moving?


	void Start()
	{
		//Get a reference to the local audio source component
		audioSource = GetComponent<AudioSource> ();
	}

	void Update()
	{
		//If the wall isn't move, leave
		if (!moving)
			return;

		//Gradually move the wall from its current position to its new desired position
		transform.position = Vector3.Lerp (transform.position, targetPosition, moveSpeed * Time.deltaTime);

		//If the wall is very close (less than .001 units away) from its desired position, consider it already there
		//and set moving to false (this prevents micro-movements and jittering)
		if (Vector3.Distance (transform.position, targetPosition) <= .001f)
			moving = false;
	}

	//This method isn't currently used but could be called to raise a wall that is in the "lowered" position
	public void RaiseWall()
	{
		//If the wall is already raised OR it is moving, leave
		if (raised || moving)
			return;

		//Calculate the new position by adding the move distance to the current position's y value
		targetPosition = transform.position + new Vector3 (0f, moveDistance, 0f);

		//The wall is now moving
		moving = true;

		//If there is an audio source, play it
		if(audioSource != null)
			audioSource.Play ();
	}

	//This method is called by the game manager to lower the wall
	public void LowerWall()
	{
		//If the wall is already lowered OR it is moving, leave
		if (!raised || moving)
			return;

		//Calculate the new position by subtracting the move distance from the current position's y value
		targetPosition = transform.position - new Vector3 (0f, moveDistance, 0f);

		//The wall is now moving
		moving = true;

		//If there is an audio source, play it
		if(audioSource != null)
			audioSource.Play ();
	}
}
