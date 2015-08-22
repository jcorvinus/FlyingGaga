using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public delegate void PlayerEventHandler();
	public event PlayerEventHandler PlayerStopped;
	public event PlayerEventHandler PlayerReset;

	#region Gameplay Variables
	public bool HasLaunched = false;
	public bool HasStopped = false;
	public float DistanceTraveled = 0;
	public TextMesh DistanceText;
	public Vector3 LaunchConstraintVector; // we'll do a dot product against this to see if we can launch properly.
	public float MinConstraint = 0.47f;
	public float MaxConstraint = 1.167f;
	public float WidthConstraint = 10f;
	private Vector3 oldPosition;

	public float LaunchCountDown=1;
	public float DisplayCountDown = 5;
	public int MaxCountDown=8;
	public int MinCountDown=2;
	private int countDownVariance=4;
	public float LaunchForce = 1f;
	private Vector3 launchDirection = Vector3.zero;
	private Vector3 lookForceAdjust = Vector3.zero;
	private Vector3 motionNuller = new Vector3(1,0,0);
	
	public int SelectedLane = 1; // valid values are 0 for left, 1 for mid, 2 for right
	#endregion

	// references to other objects
	public GameObject CannonContainer;
	public GameObject CameraLooker; // our HMD is here.
	public Rigidbody PlayerRigidBody;

	// Audio variables
	public AudioClip LaunchFailClip;
	private float launchFailClashtimer = 0.25f;
	public AudioClip CannonLaunchClip;
	public AudioClip ImpactSound;

	// visual stuff
	public GameObject LaunchErrorObject;

	#region Debug Variables
	private float debugPrintTimer;
	private float debugPrintTimeInterval = 0.25f;
	private Vector3 startPosition;
	private Quaternion startRotation;
	public bool DrawWidthBox = false;
	#endregion

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
		oldPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		DistanceText.text = string.Format("Total Distance: {0}", ((int)DistanceTraveled).ToString());
		launchFailClashtimer -= Time.deltaTime;

		if (!HasStopped)
		{
			if (!HasLaunched)
			{
				// align our cannon and HMD
				CannonContainer.transform.rotation = CameraLooker.transform.rotation;

				debugPrintTimer -= Time.deltaTime;
				if (debugPrintTimer < 0)
				{
					// debug print the dot product of our current look vs the launch constraint
					//Debug.Log(string.Format("LLock DotProd: {0}", Vector3.Dot(CameraLooker.transform.forward, LaunchConstraintVector)));
					debugPrintTimer = debugPrintTimeInterval;
				}

				float constraintDot = Vector3.Dot(CameraLooker.transform.forward, LaunchConstraintVector);
				if ((constraintDot >= MinConstraint) && (constraintDot <= MaxConstraint))
				{
					LaunchErrorObject.SetActive(false);
				}
				else
				{
					LaunchErrorObject.SetActive(true);
				}
			}
			else
			{
				if (LaunchCountDown > 0)
				{
					LaunchCountDown -= Time.deltaTime;
					DisplayCountDown -= Time.deltaTime;

					if (LaunchCountDown < 0)
					{
						PlayerRigidBody.AddForce(launchDirection, ForceMode.Impulse); // actually process force later.
						PlayerRigidBody.useGravity = true;
						AudioSource.PlayClipAtPoint(CannonLaunchClip, CameraLooker.transform.position);
						Debug.Log("Launched!");
						SendMessage("OnPlayerLaunch", SendMessageOptions.DontRequireReceiver);
					}
				}
				else // we've launched and are flying, not just sitting in the barrel waiting for countdown.
				{
					if (PlayerRigidBody.velocity.magnitude < 0.125f) // we've come to a stop.
					{
						HasStopped = true;
						Debug.Log("Stopped!");
						SendMessage("OnPlayerStopped", SendMessageOptions.DontRequireReceiver);
						if (PlayerStopped != null) PlayerStopped();
					}
				}
			}
		}

		// calculate distance
		DistanceTraveled += Vector3.Distance(transform.position, oldPosition);
		oldPosition = transform.position;
	}

	void FixedUpdate()
	{
		if (!HasStopped)
		{
			// width constraints
			if ((PlayerRigidBody.transform.position.x > (WidthConstraint / 2)) || PlayerRigidBody.transform.position.x < -(WidthConstraint / 2))
			{
				PlayerRigidBody.velocity = Vector3.Scale(PlayerRigidBody.velocity, new Vector3(0, 1, 1));
			}

			lookForceAdjust = transform.position + Vector3.Scale(CameraLooker.transform.forward, motionNuller);

			// handle motion adjusting
			if (HasLaunched && (LaunchCountDown <= 0))
			{
				PlayerRigidBody.AddForce(Vector3.Scale(CameraLooker.transform.forward, motionNuller));
			}
		}
	}

	public void ResetPlayer()
	{
		if (HasLaunched && !HasStopped) return;

		HasLaunched = false;
		LaunchCountDown = 1;
		DisplayCountDown = 5;
		PlayerRigidBody.velocity = Vector3.zero;
		PlayerRigidBody.MovePosition(startPosition);
		PlayerRigidBody.useGravity = false;
		HasStopped = false;
		CannonContainer.transform.rotation = startRotation;
		DistanceTraveled = 0f;
		Debug.Log("Resetting player!");
		SendMessage("OnPlayerReset", SendMessageOptions.DontRequireReceiver);
		PlayerReset();
	}

	void OnCollisionEnter()
	{
		PlayerRigidBody.velocity = PlayerRigidBody.velocity * 0.45f;
	}

	void OnCollisionExit()
	{
		if (!HasStopped) AudioSource.PlayClipAtPoint(ImpactSound, transform.position);
	}

	void OnDrawGizmosSelected()
	{
		Debug.DrawRay(transform.position, LaunchConstraintVector, Color.red);

		if (DrawWidthBox)
		{
			Gizmos.color = Color.grey;
			Gizmos.DrawCube(transform.position, new Vector3(WidthConstraint, 10, 1000));
		}

		if (Application.isPlaying)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawCube(lookForceAdjust, Vector3.one);
		}
	}

	public void TryLaunch()
	{
		float constraintDot = Vector3.Dot(CameraLooker.transform.forward, LaunchConstraintVector);
		if((constraintDot >= MinConstraint ) && (constraintDot <= MaxConstraint))
		{
			launchDirection = CameraLooker.transform.forward;
			LaunchCountDown = Random.Range(MinCountDown, MaxCountDown);
			DisplayCountDown = LaunchCountDown + Random.Range(0, countDownVariance);

			HasLaunched = true;
			Debug.Log("Starting Countdown!");
			SendMessage("OnPlayerCountdown", SendMessageOptions.DontRequireReceiver);
			DistanceTraveled = 0;
		}
		else
		{
			if (launchFailClashtimer <= 0)
			{
				AudioSource.PlayClipAtPoint(LaunchFailClip, CameraLooker.transform.position);
				launchFailClashtimer = 0.25f;
			}
		}
	}
}
