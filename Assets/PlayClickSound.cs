using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayClickSound : MonoBehaviour {

	public AudioSource audioSource;


	void Update () 
	{
		if(Input.GetMouseButtonDown(0)) audioSource.Play();
	}
}
