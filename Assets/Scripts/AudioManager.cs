using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public Sound[] sounds;

	private Sound sound;

	void Start()
    {
		Play("MainTheme");
    }

    void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = s.mixerGroup;
		}
	}

	public void Play(string name)
	{
		if (sound != null)
        {
			if (sound.name == name)
            {
				return;
            }
            else
            {
				sound.source.Stop();
			}
		}

		sound = Array.Find(sounds, s => s.name == name);
		if (sound == null)
		{
			Debug.LogWarning("Music " + name + " not found.");
			return;
		}
		sound.source.Play();
	}

}
