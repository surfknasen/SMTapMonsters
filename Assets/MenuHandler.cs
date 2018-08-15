using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour {

	public GameObject menu0, menu1;

	void Start () {
		
	}
	
	void Update () {
		
	}

	public void OnSwitchMenu()
	{
		if(menu0.activeInHierarchy) 
		{
			menu1.SetActive(true);
			menu0.SetActive(false);
		} else if(menu1.activeInHierarchy)
		{
			menu0.SetActive(true);
			menu1.SetActive(false);
		}
	}
}
