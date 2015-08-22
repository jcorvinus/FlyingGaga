using UnityEngine;
using System.Collections;

public class PlayerRaycastTrigger : MonoBehaviour 
{
	public string TriggerTag;
	public Vector3 RaycastDirection = Vector3.down;

	void FixedUpdate()
	{
		RaycastHit hitInfo;

		if (Physics.Raycast(transform.position, RaycastDirection, out hitInfo, float.PositiveInfinity))
		{
			if (hitInfo.collider.gameObject.tag == TriggerTag) hitInfo.collider.gameObject.SendMessage("OnRaycastTrigger", SendMessageOptions.DontRequireReceiver);
		}
	}
}
