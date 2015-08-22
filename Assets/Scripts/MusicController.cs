using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour 
{
	public AudioClip waitingMusic;
	public AudioClip flyingMusic;
	public AudioClip windClip;

	public float windEnableHeight;

	private AudioSource musicSource;
	private AudioSource windSource;

	private PlayerController playerController;
	private AudioClip targetClip;
	private float fadeTimer = -1;
	private float fadeTimeInterval = 0.45f;

	// Use this for initialization
	void Start () 
	{
		playerController = GetComponent<PlayerController>();
		musicSource = gameObject.AddComponent<AudioSource>();
		musicSource.clip = waitingMusic;
		musicSource.loop = true;
		musicSource.volume = 0.5f;
		musicSource.Play();

		windSource = gameObject.AddComponent<AudioSource>();
		windSource.clip = windClip;
		windSource.loop = true;
		windSource.playOnAwake = true;
		windSource.Play();
		windSource.volume = 0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(fadeTimer > 0)
		{
			musicSource.volume = Mathf.InverseLerp(fadeTimeInterval, 0, fadeTimer);
			fadeTimer -= Time.deltaTime;

			if(fadeTimer <= 0)
			{
				musicSource.Stop();
				musicSource.clip = targetClip;
				musicSource.volume = 0.5f;
				musicSource.Play();
			}
		}

		if(playerController.transform.position.y > windEnableHeight)
		{
			windSource.volume = Mathf.Clamp01(windSource.volume + Time.deltaTime);
		}
		else
		{
			windSource.volume = Mathf.Clamp01(windSource.volume - Time.deltaTime);
		}
	}

	void OnPlayerLaunch()
	{
		targetClip = flyingMusic;
		fadeTimer = fadeTimeInterval;
	}

	void OnPlayerReset()
	{
		targetClip = waitingMusic;
		fadeTimer = fadeTimeInterval;
	}
}
