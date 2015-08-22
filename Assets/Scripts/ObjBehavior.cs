using UnityEngine;
using System.Collections;

public class ObjBehavior : MonoBehaviour {
	//Player reference. NOTE:Player needs player tag
	private GameObject player; 

	public Rigidbody PlayerRigidBody;
	private float OperantForce;

	//Represents the type of object landed on
	//1 denotes a slowing object and launches at 15 deg
	//2 denotes a slowing object and launches at 30 deg
	//3 denotes a slowing object and launches at 45 deg
	//4 denotes a slowing object and launches at 60 deg
	public int bounceStrength;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
	}


	//If player hits the objects then corresponding velocity multiplier
	//Launch from new angle


	void OnCollisionEnter (Collision col){
		OperantForce = 40;
		if(bounceStrength==1){
			OperantForce = 0.70f*OperantForce*(PlayerRigidBody.velocity.magnitude+1);
			PlayerRigidBody.AddForce(Vector3.up * OperantForce, ForceMode.VelocityChange);
		}
		if(bounceStrength==2){
			OperantForce = 1.25f*OperantForce*(PlayerRigidBody.velocity.magnitude+1);
			PlayerRigidBody.AddForce(Vector3.up * OperantForce, ForceMode.VelocityChange);
		}
		if(bounceStrength==3){
			OperantForce = 1.5f*OperantForce*(PlayerRigidBody.velocity.magnitude+1);
			PlayerRigidBody.AddForce(Vector3.up * OperantForce, ForceMode.VelocityChange);
		}
		if(bounceStrength==4){
			OperantForce = 2f*OperantForce*(PlayerRigidBody.velocity.magnitude+1);
			PlayerRigidBody.AddForce(Vector3.up * OperantForce, ForceMode.VelocityChange);
		
		}
	}	


	// Update is called once per frame
	void Update () {

	}
}
