using UnityEngine;
using System.Collections;

/// <summary>
/// Class in play the sound in the game
/// </summary>
public class SoundManager : MonoBehaviourHelper 
{
	AudioSource a;

	public AudioClip soundBeep;
	public AudioClip soundFail;
	public AudioClip soundSuccess;   

	bool canPlay
	{
		get
		{
			return Util.SoundIsOn();
		}
	}

	void Awake()
	{
		a = FindObjectOfType<AudioSource> ();
	}
	/// <summary>
	/// Play sound beep
	/// </summary>
	public void PlaySoundBeep()
	{
		if(canPlay)
			a.PlayOneShot (soundBeep);
	}
	/// <summary>
	/// Play sound fail
	/// </summary>
	public void PlaySoundFail()
	{
		if(canPlay)
			a.PlayOneShot (soundFail);
	}
	/// <summary>
	/// Play sound success
	/// </summary>
	public void PlaySoundSuccess()
	{
		if(canPlay)
			a.PlayOneShot (soundSuccess);
	}

	public void MuteAllMusic()
	{
		
	}

	public void UnmuteAllMusic()
	{
		
	}

    public void pauseAllMusic()
    {
        AudioSource music = GameObject.Find("Main Camera/Music").GetComponent<AudioSource>();
        music.Pause();
    }

    public void resumeAllMusic()
    {
        AudioSource music = GameObject.Find("Main Camera/Music").GetComponent<AudioSource>();
        music.Play();
    }
}
