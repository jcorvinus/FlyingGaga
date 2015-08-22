using UnityEngine;
using System.Collections;

public class PlayerFollow : MonoBehaviour 
{
	public float FollowDistance = 100;
	private GameObject playerGameObject;

	void Start()
	{
		playerGameObject = GameObject.FindGameObjectWithTag("Player");
	}

	void FixedUpdate()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, playerGameObject.transform.position.z - FollowDistance);
	}	
}
