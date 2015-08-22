using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour 
{
	public Animator[] Animators;

	void OnPlayerReset()
	{
		foreach (Animator anim in Animators)
		{
			anim.SetTrigger("Stop");
		}
	}

	void OnPlayerLaunch()
	{
		foreach (Animator anim in Animators)
		{
			anim.SetTrigger("Activate");
		}
	}	
}
