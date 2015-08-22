using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldStripSpawner : MonoBehaviour 
{
	public GameObject Strip;
	public List<GameObject> StripInstanceList;

	void Clear()
	{
		foreach (GameObject strip in StripInstanceList) Destroy(strip);

		StripInstanceList.Clear();
	}

	void Start()
	{
		StripInstanceList = new List<GameObject>();
	}

	public void Trigger()
	{
		// spawn a new strip and align it
		GameObject previousStrip;
		previousStrip = (StripInstanceList.Count > 0) ? StripInstanceList[StripInstanceList.Count - 1] : Strip;
		GameObject newStrip = (GameObject)GameObject.Instantiate(Strip, Strip.transform.position, Quaternion.identity);

		StripAligner[] previousAligners = previousStrip.GetComponentsInChildren<StripAligner>();
		Vector3 previousAlignerFront = Vector3.zero;
		Vector3 previousAlignerBack = Vector3.zero;

		foreach (StripAligner aligner in previousAligners)
		{
			if (aligner.Front)
			{
				previousAlignerFront = aligner.transform.position;
			}
			else
			{
				previousAlignerBack = aligner.transform.position;
			}
		}

		float distance = Vector3.Distance(previousAlignerFront, previousAlignerBack);
		newStrip.transform.Translate(Vector3.forward * distance);

		StripInstanceList.Add(newStrip);
	}
}
