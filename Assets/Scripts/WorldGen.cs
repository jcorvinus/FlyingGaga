using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGen : MonoBehaviour {
	//Possible object spawn closest to player on z axis
	public float near = -7.3f;
	//Possible object spawn furthest from player on z axis
	public float far = -3f;
	//Initial distance loaded before player even fires
	public float startSpawnDist = 200f;
	//Object spawning in
	public GameObject groundObject;
	public GameObject airObject;
	//Player reference. NOTE:Player needs player tag
	private GameObject player; 
	//Reference to the furthest object made
	private float furthestMade;
	//Distance from furthest to player where you should start spawning more
	public float threshold = 100f;
	private List<GameObject> spawnedObjects;

	// Use this for initialization
	void Start () {
		spawnedObjects = new List<GameObject>();
		furthestMade = 0;
		player = GameObject.FindWithTag("Player");
		//near = -7.3f;
		//far = -3f;
		////Starter threshold. Update with player speed
		//threshold = 100f;
		//generating the initial part of the level
		while (furthestMade < startSpawnDist) {
			levelGen(1);
			levelGen(2);
			levelGen(3);
			}
	}

	// Update is called once per frame
	void Update () {
		//if distance from player to furthest is less than threshold OR player dist is >than furthest
		if((furthestMade-player.transform.position.z)<threshold){
		levelGen(1);
		levelGen(2);
		levelGen(3);
		}
	}

	void Clear() {
		foreach(GameObject clearObject in spawnedObjects)
		{
			if (clearObject != null) Destroy(clearObject);
		}

		spawnedObjects.Clear();
	}

	//Lets me select a random location in a range
	System.Random r = new System.Random();
	float rnd(float a, float b){
		return (float)(a + r.NextDouble()*(b-a));
	}

	//Method responsible for instantiating objects
	private void levelGen(int lane) {
			//Test to see if I can catch when player hits edge of generated content
			//Locations generated randomly within desired spawn area
			//For test purposes, x is defined between -5.18 & 5.37. Note this is only 1 lane ATM. 
			//3 lanes planned through copy and paste upon completion of generating code.

			float leftBound = 0;
			float rightBound = 0;
			if (lane == 1) {
				leftBound = -5.18f;
				rightBound = 5.37f;
			}
			if (lane == 2) {
				leftBound = 15.18f;
				rightBound = 20.37f;
			}
			if (lane == 3) {
				leftBound = -20.18f;
				rightBound = -15.37f;
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
