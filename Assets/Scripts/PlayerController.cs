using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	// gameplay variables
	public bool HasLaunched = false;
	public float DistanceTraveled = 0;
	public Vector3 LaunchConstraintVector; // we'll do a dot product against this to see if we can launch properly.
	public float MinConstraint = 0.47f;
	public float MaxConstraint = 1.167f;

	public float LaunchCountDown=1;
	public float DisplayCountDown = 5;
	public int MaxCountDown=8;
	public int MinCountDown=2;
	private int countDownVariance=3;
	public float LaunchForce = 1f;
	private Vector3 launchDirection = Vector3.zero;
	
	public int SelectedLane = 1; // valid values are 0 for left, 1 for mid, 2 for right

	// references to other objects
	public GameObject CannonContainer;
	public GameObject CameraLooker; // our HMD is here.
	public Rigidbody PlayerRigidBody;

	// Audio variables
	public AudioClip LaunchFailClip;
	public AudioClip CannonLaunchClip;

	// debug variables
	private float debugPrintTimer;
	private float debugPrintTimeInterval = 0.25f;
	private Vector3 startPosition;
	private Quaternion startRotation;


	void Awake()
	{
		PlayerRigidBody = GetComponent<Rigidbody>();
		debugPrintTimer = debugPrintTimeInterval;
	}

	// Use this for initialization
	void Start () 
	{
		startPosition = transform.position;
		startRotation = CameraLooker.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!HasLaunched)
		{
			// align our cannon and HMD
			CannonContainer.transform.rotation = CameraLooker.transform.rotation;

			debugPrintTimer -= Time.deltaTime;
			if (debugPrintTimer < 0)
			{
				// debug print the dot product of our current look vs the launch constraint
				Debug.Log(string.Format("LLock DotProd: {0}", Vector3.Dot(CameraLooker.transform.forward, LaunchConstraintVector)));
				debugPrintTimer = debugPrintTimeInterval;
			}
		}
		else
		{
			if(LaunchCountDown > 0)
			{
				LaunchCountDown -= Time.deltaTime;
				DisplayCountDown -= Time.deltaTime;

				if(LaunchCountDown < 0)
				{
					PlayerRigidBody.AddForce(launchDirection, ForceMode.VelocityChange); // actually process force later.
				}
			}
		}
	}

	public void ResetPlayer()
	{

	}

	void OnDrawGizmosSelected()
	{
		Debug.DrawRay(transform.position, LaunchConstraintVector, Color.red);
	}

	public void TryLaunch()
	{
		float constraintDot = Vector3.Dot(CameraLooker.transform.forward, LaunchConstraintVector);
		if((constraintDot >= MinConstraint ) && (constraintDot <= MaxConstraint))
		{
			launchDirection = CameraLooker.transform.forward;
			LaunchCountDown = Random.Range(MinCountDown, MaxCountDown);
			DisplayCountDown = LaunchCountDown + Random.Range(0, countDownVariance);
		}
	}
}
