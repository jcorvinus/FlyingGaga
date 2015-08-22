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
			Vector3 v31 = new Vector3(0f,0.133f,1f);
			OperantForce = 3+(0.50f*PlayerRigidBody.velocity.magnitude*10);
			PlayerRigidBody.AddForce(v31 * OperantForce, ForceMode.VelocityChange);
		}
		if(bounceStrength==2){
			Vector3 v32 = new Vector3(0f,1.7f,3f);
			OperantForce = 3+(1.25f*PlayerRigidBody.velocity.magnitude*10);
			PlayerRigidBody.AddForce(v32 * OperantForce, ForceMode.VelocityChange);
		}
		if(bounceStrength==3){
			Vector3 v33 = new Vector3(0f,0.7f,0.7f);
			OperantForce = 3+(1.5f*PlayerRigidBody.velocity.magnitude*10);
			PlayerRigidBody.AddForce(v33 * OperantForce, ForceMode.VelocityChange);
		}
		if(bounceStrength==4){
			Vector3 v34 = new Vector3(0f,1.7f,1f);
			OperantForce = 3+(2f*PlayerRigidBody.velocity.magnitude*10);
			PlayerRigidBody.AddForce(v34 * OperantForce, ForceMode.VelocityChange);
		
		}
	}	


	// Update is called once per frame
	void Update () {

	}
}
