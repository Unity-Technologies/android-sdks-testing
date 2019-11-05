//This script controls the game's collectable pickups. It handles a collectable's visual effects
//as well as tell a controlling object when it is "picked up"

using UnityEngine;

public class Collectable : MonoBehaviour 
{
	public CollectableSpawner spawner;	//A reference to whatever script "spawned" this collectable
	public float disapearDuration = 3f;	//The amount of time it take for the visual effects to fade away once collected
	public float lightFadeAmount = 2f;	//The amount (or how quickly) the spotlight should fade away once collected

	MeshRenderer shield;		//Reference to the collectable's mesh renderer
	ParticleSystem particles;	//Reference to the collectable's particle system
	Light skyLight;				//Reference to the collectable's light component
	AudioSource audioSource;	//Reference to the collectable's audio source
	bool isActive = true;		//Is the collectable currently alive (has it not been collected)?


	void Awake()
	{
		//Find references to all of the components this script needs to change
		shield = GetComponentInChildren<MeshRenderer> ();
		particles = GetComponentInChildren<ParticleSystem> ();
		skyLight = GetComponentInChildren<Light> ();
		audioSource = GetComponent<AudioSource> ();
	}

	void Update()
	{
		//If this collectable has been picked up...
		if (!isActive) 
		{
			//Reduce the intensity of its light so it fades away
			skyLight.intensity -= lightFadeAmount * Time.deltaTime;
		}
	}

	//This is called whenever an object runs into this object
	void OnTriggerEnter(Collider other)
	{
		//If this collectable has already been picked up OR of the object trying to
		//pick this up is not the player, leave
		if (!isActive || !other.CompareTag("Player"))
			return;

		//This has been picked up now, so it is no longer active
		isActive = false;
		//Hide the shield
		shield.enabled = false;
		//Stop emitting particles. The existing particles will still remain and
		//fade away in time
		particles.Stop ();

		//If there is an audio source, play it
		if(audioSource != null)
			audioSource.Play ();

		//If there is a spawner, tell it that this was collected
		if (spawner != null)
			spawner.CollectableTaken ();

		//Destroy this game object after a delay. That way, we can see the 
		//visual effects fade out instead of abruptly disappearing
		Destroy (gameObject, disapearDuration);
	}
}
