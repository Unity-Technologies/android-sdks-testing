//This script handles showing advertisements and then rewarding the player.

#if UNITY_IOS || UNITY_ANDROID

using UnityEngine;
using UnityEngine.Advertisements; //Enable ads in script

public class AdsManager : MonoBehaviour 
{
	//This method will be called by the "Watch Ad" button on the Loss Screen
	public void ShowRewardedAd()
	{
		//Before trying to show an ad, make sure it is ready to be shown. If it is...
		if (Advertisement.IsReady("rewardedVideo"))
		{
			//...create a ShowOptions variable which we will use to tell us
			//when the player is done watching the ad...
			ShowOptions options = new ShowOptions ();

			//...then tell the options variable that we'd like the method HandleAdResults()
			//to be called when the ad is done playing...
			options.resultCallback = HandleAdResults;

			//...finally show the advertisement
			Advertisement.Show("rewardedVideo", options);
		}
	}

	//This method will be called when an ad finishes playing
	void HandleAdResults(ShowResult result)
	{
		//If the result is "Finished" then the player watched the full ad and we should...
		if (result == ShowResult.Finished) 
		{
			//...write about the success to the console...
			Debug.Log ("Ad finished successfully");
			//...and if the GameManager exists, add 30 more seconds to the player's 
			//time to play
			if (GameManager.instance != null)
				GameManager.instance.AddMoreGameTime (30f);
		} 
		else 
		{
			//...If the ad didn't finish, then either they skipped the ad
			//or there was a technical issue that prevented it from working.
			//Either way, we log the problem and don't reward the extra time
			Debug.Log ("Something went wrong. Ad not viewed");
		}
	}
}

#else

//Since the ad test image will only show if the Unity editor is set to build for
//iOS or Android, we need to simulate the ad being shown on any other platform

using UnityEngine;

public class AdsManager : MonoBehaviour 
{
	//This method will be called by the "Watch Ad" button on the Loss Screen
	public void ShowRewardedAd()
	{

		//Write that we are simulating to the console
		Debug.Log("Build platform is not set to iOS or Android. Simulating Ad view");

		if (GameManager.instance != null)
			GameManager.instance.AddMoreGameTime (30f);
	}
}
#endif
