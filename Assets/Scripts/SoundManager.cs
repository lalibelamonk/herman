using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource fxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;
	
	// pitch variation for dynamacism
	public float lowPitchRange = .95f;
	public float highPitchRange = 1.05f;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		
		DontDestroyOnLoad(gameObject);
	}
	
	public void playSingle (AudioClip clip) {
		fxSource.clip = clip;
		fxSource.Play ();
	}
	
	public void randomizeFx (params AudioClip [] clips) {
		int randomIndex = Random.Range (0, clips.Length);
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);
		
		fxSource.pitch = randomPitch;
		fxSource.clip = clips[randomIndex];
		fxSource.Play();
	}
}
