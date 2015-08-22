using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
}
/**
 * 
using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour 
{
	private PlayerController playerController;
	private KeyCode launchKey = KeyCode.Space;
	private KeyCode cameraResetKey = KeyCode.R;
	private KeyCode gameResetKey = KeyCode.F;
	private OVRDisplay cameraDisplay;

	// Use this for initialization
	void Start () 
	{
		playerController = GetComponent<PlayerController>();
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

			}
		}
	}
}
 **/