﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightHandler : MonoBehaviour {

	public static bool bossFightActive;
	public MenuHandler menuHandler;
	public Animation sceneSwitch;
	public GameObject[] bossCards;
	public GameObject[] bossPrefabs;
	public List<GameObject> fightUIElementsToShow; // health sliders, texts etc
	public List<GameObject> fightUIElementsToHide;
	public ScrollRectSnap scrollRectSnap;

	private GameObject trainingDummy;
	private GameObject currentBoss;
	private PlayerAttack playerAttack;
	private Health playerHealth;

	void Start () 
	{
		trainingDummy = GameObject.FindGameObjectWithTag("OtherMonster");
		playerHealth = GameObject.FindGameObjectWithTag("MyMonster").GetComponent<Health>();
		playerAttack = GetComponent<PlayerAttack>();
		foreach(GameObject g in fightUIElementsToShow)
		{
			g.SetActive(false);
		}
	}
	
	public void SetHealthCanvas(GameObject newMonster)
	{
		foreach(Transform t in newMonster.transform)
		{
			if(t.gameObject.name == "Canvas")
			{
				fightUIElementsToShow[0] = t.gameObject;
				t.gameObject.SetActive(false);
			}
		}
	}

	public void StartFight()
	{
		StartCoroutine(StartFightCoroutine());
	}

	IEnumerator StartFightCoroutine()
	{
		playerAttack.canAttack = false; 
		playerAttack.autoAttack = false;

		sceneSwitch.Play();
 		yield return new WaitForSeconds(1); // delay while the animation is taking place. the animation is 2 seconds. this is half of that time
		playerAttack.canAttack = true; 
		bossFightActive = true;

		// ** FIX UP THE MENU (REMOVE THE BOTTOM MENU) ** //
		foreach(GameObject g in fightUIElementsToShow)
		{
			g.SetActive(true);
		}
		foreach(GameObject g in fightUIElementsToHide)
		{
			g.SetActive(false);
		}

	
		// ** SPAWN BOSS ** //

		// hide training dummy
		trainingDummy.SetActive(false);		
		//find player
		// spawn the boss
		for(int i = 0; i < bossCards.Length; i++)
		{
			if(bossCards[i] == scrollRectSnap.cardToSnapTo.gameObject)
			{
				GameObject boss = Instantiate(bossPrefabs[i]);
				currentBoss = boss;
				GetComponent<PlayerAttack>().SetOtherMonsterVariables(boss);
				GetComponent<BossAttack>().StartBossFight(boss, GameObject.FindGameObjectWithTag("MyMonster"));
				break;
			}
		}

		// ** CENTER MY MONSTER ** //
		Transform monster = GameObject.FindGameObjectWithTag("MyMonster").transform.parent;
		monster.position = Vector3.zero;
	}
	

	public void EndFight(bool won)
	{
		StartCoroutine(EndFightCoroutine(won));
	}

	IEnumerator EndFightCoroutine(bool won)
	{
		playerAttack.canAttack = false; // turn this off for now so that it doesn't attack during the win/lose animation screen
		GetComponent<BossAttack>().canAttack = false;
		
		if(won)
		{
			// give exp and stuff
			print("won");
		} 

		sceneSwitch.Play();
		yield return new WaitForSeconds(1); // delay to let that stuff finish
		bossFightActive = false;

		// some animation sutff, win/lose screen
		
		playerHealth.ResetScript(); // reset the health script
		
		// return to the training grounds
		foreach(GameObject g in fightUIElementsToShow) // hide the health and stuff
		{
			g.SetActive(false);
		}
		foreach(GameObject g in fightUIElementsToHide) // show the right canvas
		{
			g.SetActive(true);
		}
		Destroy(currentBoss); // remove the boss
		GetComponent<PlayerAttack>().SetOtherMonsterVariables(trainingDummy.transform.parent.gameObject); // update the player to the new monster

		trainingDummy.SetActive(true); // put back the training dummy
		menuHandler.menu0.SetActive(true); // show the card selector menu
		menuHandler.menu1.SetActive(false);

		GameObject otherMonster = GameObject.FindGameObjectWithTag("MyMonster"); // reset the player monster position
		Vector3 ogPos = playerAttack.myOriginalPos;
		otherMonster.transform.parent.position = new Vector3(0, ogPos.y, 0);

		yield return new WaitForSeconds(1); // since the scene switch animation is 2 sec, we finish that here before letting the player attack again
		GetComponent<BossAttack>().canAttack = true;
		playerAttack.canAttack = true;
		playerAttack.autoAttack = true;
		
	}
}
