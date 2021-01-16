using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public static AudioManager Instance;
	public GameObject audioSourcePrefab;

	private List<AudioSource> loopingSounds = new List<AudioSource>();

	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}

	public AudioSource PlaySoundSFX(AudioClip clip, GameObject objectToPlayOn, float volume = 1f, float pitch = 1f, float blend = 1f, bool loop = false) {
		AudioSource freshAudioSource = PlaySoundSFX(clip, objectToPlayOn.transform.position, volume, pitch, blend, loop);
		freshAudioSource.gameObject.transform.parent = objectToPlayOn.transform;

		freshAudioSource.GetComponent<VirtualAudioSource>().CalculateClosestListener(objectToPlayOn);

		return freshAudioSource;
	}

	public AudioSource PlaySoundSFX(AudioClip clip, Vector3 positionToPlayAt, float volume = 1f, float pitch = 1f, float blend = 1f, bool loop = false) {
		AudioSource freshAudioSource = Instantiate(audioSourcePrefab).GetComponent<AudioSource>();
		freshAudioSource.gameObject.transform.position = positionToPlayAt;
		freshAudioSource.pitch = pitch;
		freshAudioSource.volume = volume;
		freshAudioSource.spatialBlend = blend;
		freshAudioSource.clip = clip;
		freshAudioSource.loop = loop;
		freshAudioSource.Play();

		if (loop) loopingSounds.Add(freshAudioSource);
		else Destroy(freshAudioSource.gameObject, freshAudioSource.clip.length * pitch);

		freshAudioSource.GetComponent<VirtualAudioSource>().CalculateClosestListener(positionToPlayAt);

		return freshAudioSource;
	}

	public void StopSound(AudioSource source, float fadeTime) {
		StartCoroutine(FadeOut(source, fadeTime));
		if (loopingSounds.Contains(source)) loopingSounds.Remove(source);
	}

	IEnumerator FadeOut(AudioSource source, float fadeTime) {
		float startTime = Time.time;
		float currentTime = 0f;
		float startVolume = source.volume;

		while (startTime + fadeTime > Time.time) {
			currentTime = Time.time - startTime;

			source.volume = Mathf.Lerp(startVolume, 0f, currentTime / fadeTime);
			yield return null;
		}

		source.Stop();
		Destroy(source.gameObject);

	}
}