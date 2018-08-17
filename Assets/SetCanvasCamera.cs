using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCanvasCamera : MonoBehaviour {

	void Start () 
	{
		GetComponent<Canvas>().worldCamera = Camera.main;
	}
	
}
