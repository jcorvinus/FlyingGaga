using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour 
{
	private PlayerController playerController;
	public TextMesh CountdownText;
	bool isCountDown = false;

	// Use this for initialization
	void Start () 
	{
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isCountDown)
		{
			CountdownText.text = ((int)playerController.DisplayCountDown).ToString();
		}
	}

	void OnPlayerLaunch()
	{
		isCountDown = false;
		CountdownText.text = "";
	}

	void OnPlayerCountdown()
	{
		isCountDown = true;
	}
}
