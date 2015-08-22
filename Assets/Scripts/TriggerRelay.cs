using UnityEngine;
using System.Collections;

public class TriggerRelay : MonoBehaviour 
{
	public GameObject[] Targets;
	public bool TriggerOnce = true;
	private bool hasTriggered = false;

	private GameController gameController;

	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		gameController.GameReset += gameController_GameReset;
	}

	void gameController_GameReset()
	{
		Reset();
	}

	void SendMessageToTargets()
	{
		if(TriggerOnce)
		{
			if (hasTriggered) return;
		}

		foreach (GameObject target in Targets) target.SendMessage("Trigger");

		hasTriggered = true;
	}

	void OnRaycastTrigger()
	{
		SendMessageToTargets();
	}

	void OnTrigger()
	{
		SendMessageToTargets();
	}

	public void Reset()
	{
		hasTriggered = false;
	}
}
