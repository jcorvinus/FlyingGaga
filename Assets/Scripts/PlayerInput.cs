using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour 
{
	private PlayerController playerController;
	private KeyCode launchKey = KeyCode.Space;
	private KeyCode cameraResetKey = KeyCode.R;
	private KeyCode gameResetKey = KeyCode.F;
	private KeyCode timeIncrease = KeyCode.Plus;
	private KeyCode timeDecrease = KeyCode.Minus;
	private KeyCode timeLow = KeyCode.Alpha0;
	private OVRDisplay cameraDisplay;

	// Use this for initialization
	void Start () 
	{
		playerController = GetComponent<PlayerController>();
		cameraDisplay = new OVRDisplay();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!playerController.HasLaunched)
		{
			if(Input.GetKeyDown(cameraResetKey)) // reset camera
			{
				cameraDisplay.RecenterPose();
			}
			else if (Input.GetKeyDown(launchKey)) // try launching
			{
				playerController.TryLaunch();
			}
		}
		else
		{
			if(Input.GetKeyDown(gameResetKey))
			{
				playerController.ResetPlayer();
			}
		}

		// time scaling
		if (Input.GetKeyDown(timeDecrease)) Time.timeScale -= 0.2f;
		else if (Input.GetKeyDown(timeIncrease)) Time.timeScale += 0.2f;
		else if (Input.GetKeyDown(timeLow)) Time.timeScale = 0.1f;
	}
}
 