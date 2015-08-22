using UnityEngine;
using System.Collections;

public enum BounceType { Stalling=1, Medium=2, Perfect=3, HighMedium=4 }

public class ObjBehavior : MonoBehaviour 
{
	//Player reference. NOTE:Player needs player tag
	public Rigidbody PlayerRigidBody;
	private float OperantForce;
	//Represents the type of object landed on
	//1 denotes a slowing object and launches at 15 deg
	//2 denotes a speeding up object and launches at 30 deg
	//3 denotes a speeding up object and launches at 45 deg
	//4 denotes a speeding up object and launches at 60 deg
	//NOTE: as these were made in ascending order 3 is actually more powerful than 4
	public BounceType bounceStrength;

	public Vector3[] AngleValues = new Vector3[] { new Vector3(0f, 0.133f, 1f),
		new Vector3(0f, 1.7f, 3f), new Vector3(0f, 0.7f, 0.7f), new Vector3(0f, 1.7f, 1f) };

	public GameObject[] ModelList;
	// Use this for initialization
	void Start () 
	{
		PlayerRigidBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
		OnEnable();
	}

	void OnEnable()
	{
		foreach (GameObject model in ModelList) model.SetActive(false);
		ModelList[(int)bounceStrength - 1].SetActive(true);
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Player")
		{
			//Additional force I used to simualte my objects moving faster and keep pacing
			//May ultimately be unnecessary.
			OperantForce = 10;
			//bounceStrength is something I was planning on attributing to the individual assets themselves.
			if ((int)bounceStrength == 1)
			{
				PlayerRigidBody.AddForce(AngleValues[(int)bounceStrength - 1].normalized * (PlayerRigidBody.velocity.magnitude * 0.45f));
			}
			else
			{
				PlayerRigidBody.AddForce(AngleValues[(int)bounceStrength - 1].normalized * OperantForce);
			}
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, AngleValues[(int)bounceStrength - 1]);
	}
}
