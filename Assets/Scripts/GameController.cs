using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	public delegate void GameEventHandler();
	public event GameEventHandler GameReset;

	public GameObject FurthestDistanceMarker;
	public TextMesh NewRecordText;
	public TextMesh RestartText;
	private PlayerController playerController;
	private float distance=0;
	private bool setDistance = false;

	public GameObject[] WorldManagers;

	// Use this for initialization
	void Start () 
	{
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		playerController.PlayerStopped += playerController_PlayerStopped; // HACK: this is what happens when you build a game in a day.
		playerController.PlayerReset += playerController_PlayerReset;
	}

	void playerController_PlayerReset()
	{
		OnPlayerReset();
		GameReset();
	}

	void playerController_PlayerStopped()
	{
		OnPlayerStopped();
	}
	
	void OnPlayerStopped()
	{
		if (playerController.DistanceTraveled > distance)
		{
			distance = playerController.DistanceTraveled;
			setDistance = true;
		}
		else setDistance = false;

		NewRecordText.gameObject.SetActive(setDistance);
		RestartText.gameObject.SetActive(true);
	}

	void OnPlayerReset()
	{
		if (setDistance)
		{
			FurthestDistanceMarker.transform.position = new Vector3(0, 6.54f, distance);
			FurthestDistanceMarker.SetActive(true);
			NewRecordText.gameObject.SetActive(false);
			Debug.Log(string.Format("SetDistance, {0}", FurthestDistanceMarker.transform.position));

			// clearing world managers
			foreach (GameObject worldMan in WorldManagers) worldMan.SendMessage("Clear");
		}

		playerController.DistanceTraveled = 0;
		RestartText.gameObject.SetActive(false);
	}
}
