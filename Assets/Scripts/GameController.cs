using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	public GameObject FurthestDistanceMarker;
	public TextMesh NewRecordText;
	private PlayerController playerController;
	private float distance=0;
	private bool setDistance = false;

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
	}

	void OnPlayerReset()
	{
		if (setDistance)
		{
			FurthestDistanceMarker.transform.position = new Vector3(0, 0.55f, distance);
			FurthestDistanceMarker.SetActive(true);
			NewRecordText.gameObject.SetActive(false);
		}
	}
}
