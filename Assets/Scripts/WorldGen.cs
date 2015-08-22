using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Spawns interactive entities in the world. The spawning routine starts from the player's z location and walks outward into the distance.
/// </summary>
public class WorldGen : MonoBehaviour
{
	/// <summary>Near bound of spawning region.</summary>
	public float near = -7.3f;
	/// <summary>Far bound of spawning region.</summary>
	public float far = -3f;
	//Initial distance loaded before player even fires
	public float startSpawnDist = 200f;
	/// <summary>Prefab for spawning ground entities.</summary>
	public GameObject groundObject;
	/// <summary>Prefab for aerial objects. Currently unused.</summary>
	public GameObject airObject;
	/// <summary>Player Object.</summary>
	private GameObject player; 
	/// <summary>Distance to current farthest object. Used for knowing when to quit generating objects.</summary>
	private float furthestMade;
	//Distance from furthest to player where you should start spawning more. Scale this to player speed
	public float threshold = 100f;
	/// <summary>A list of currently spawned objects and references in the world. May contain null references, so check before looping.</summary>
	private List<GameObject> spawnedObjects;

	private float startNear;
	private float startFar;
	private float initStartSpawnDist;

	// Use this for initialization
	void Start () 
	{
		spawnedObjects = new List<GameObject>();
		furthestMade = 0;
		player = GameObject.FindWithTag("Player");
		GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		gameController.GameReset += gameController_GameReset;

		//generating the initial part of the level
		startNear = near;
		startFar = far;
		initStartSpawnDist = startSpawnDist;
		Init();
	}

	void gameController_GameReset()
	{
		Init();
	}

	private void Init()
	{
		near = startNear;
		far = startFar;
		startSpawnDist = initStartSpawnDist;

		furthestMade = 0;
		while (furthestMade < startSpawnDist)
		{
			levelGen(1);
			levelGen(2);
			levelGen(3);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		//if distance from player to furthest is less than threshold OR player dist is >than furthest
		if((furthestMade-player.transform.position.z)<threshold)
		{
			levelGen(1);
			levelGen(2);
			levelGen(3);
		}
	}

	void Clear() 
	{
		foreach(GameObject clearObject in spawnedObjects)
		{
			if (clearObject != null) Destroy(clearObject);
		}

		spawnedObjects.Clear();
	}

	//Lets me select a random location in a range
	System.Random r = new System.Random();

	float rnd(float a, float b)
	{
		return (float)(a + r.NextDouble()*(b-a));
	}

	//Method responsible for instantiating objects
	private void levelGen(int lane) 
	{
		float leftBound = 0;
		float rightBound = 0;

		if (lane == 1)
		{
			leftBound = -5.18f;
			rightBound = 5.37f;
		}
		else if (lane == 2) 
		{
			leftBound = 10.18f;
			rightBound = 15.37f;
		}
		else if (lane == 3)
		{
			leftBound = -15.18f;
			rightBound = -10.37f;
		}

		float x = rnd (leftBound,rightBound);

		//Y should be the desired spawn height of the object. 
		float spawnHeight = 0.39f;
		float y = spawnHeight;

		//near and far are meant to create a range more immediately in front of the player
		//Currently they are concretely defined. Need to be player linked though
		float z;
		z = rnd (near, far);
			near = far;
			far = far+2f;

		GameObject newObject = (GameObject)Instantiate (groundObject, new Vector3 (x, y, z), Quaternion.identity);
		ObjBehavior objectBehavior = newObject.GetComponent<ObjBehavior>();
		objectBehavior.bounceStrength = (BounceType)Random.Range(1, 4);
		spawnedObjects.Add(newObject);

		furthestMade = z;
	}
}
