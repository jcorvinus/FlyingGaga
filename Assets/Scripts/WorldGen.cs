using UnityEngine;
using System.Collections;

public class WorldGen : MonoBehaviour {
	//Possible object spawn closest to player on z axis
	private float near;
	//Possible object spawn furthest from player on z axis
	private float far;
	//Initial distance loaded before player even fires
	private float startSpawnDist = 200f;
	//Object spawning in
	public GameObject prefab;
	//Player reference. NOTE:Player needs player tag
	private GameObject player; 
	//Reference to the furthest object made
	private float furthestMade;
	//Distance from furthest to player where you should start spawning more
	private float threshold;

	// Use this for initialization
	void Start () {
		furthestMade = 0;
		player = GameObject.FindWithTag("Player");
		near = -7.3f;
		far = -3f;
		//Starter threshold. Update with player speed
		threshold = 100f;
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

	//Lets me select a random location in a range
	System.Random r = new System.Random();
	float rnd(float a, float b){
		return (float)(a + r.NextDouble()*(b-a));
	}

	//Method responsible for instantiating objects
	private void levelGen(int lane){
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

			Instantiate (prefab, new Vector3 (x, y, z), Quaternion.identity);
			furthestMade = z;
		}
}
