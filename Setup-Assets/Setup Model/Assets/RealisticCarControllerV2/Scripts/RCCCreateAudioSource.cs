//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCCCreateAudioSource : MonoBehaviour {

	public static AudioSource NewAudioSource(GameObject go, string audioName, float minDistance, float maxDistance, float volume, AudioClip audioClip, bool loop, bool playNow, bool destroyAfterFinished){
		
		GameObject audioSource = new GameObject(audioName);
		audioSource.transform.position = go.transform.position;
		audioSource.transform.rotation = go.transform.rotation;
		audioSource.transform.parent = go.transform;
		audioSource.AddComponent<AudioSource>();
		audioSource.GetComponent<AudioSource>().priority = 128;
		audioSource.GetComponent<AudioSource>().minDistance = minDistance;
		audioSource.GetComponent<AudioSource>().maxDistance = maxDistance;
		audioSource.GetComponent<AudioSource>().volume = volume;
		audioSource.GetComponent<AudioSource>().clip = audioClip;
		audioSource.GetComponent<AudioSource>().loop = loop;
		audioSource.GetComponent<AudioSource>().spatialBlend = 1f;
		
		if(playNow)
			audioSource.GetComponent<AudioSource>().Play();
		
		if(destroyAfterFinished){
			if(audioClip)
				Destroy(audioSource, audioClip.length);
			else
				Destroy(audioSource);
		}

		if(go.transform.Find("All Audio Sources"))
			audioSource.transform.SetParent(go.transform.Find("All Audio Sources"));
		else
			audioSource.transform.SetParent(go.transform);
		
		return audioSource.GetComponent<AudioSource>();
		
	}

}
