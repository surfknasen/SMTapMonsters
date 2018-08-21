﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSelector : MonoBehaviour {

	public GameObject[] cards;
	public GameObject[] monsters;
	public ScrollRectSnap scrollRectSnap;

	private LevelSystem levelSystem;
	private PlayerAttack playerAttack;
	private int[] unlockableLvl;
	[SerializeField]
	private Color lockedColor;

	void Start () 
	{
		
		levelSystem = GetComponent<LevelSystem>();
		playerAttack = GetComponent<PlayerAttack>();
		
		unlockableLvl = new int[100];

		UpdateCardStates();
	}

	void UpdateCardStates()
	{
		for(int i = 0; i < cards.Length; i++)
		{
			//unlockableLvl[i] = 1 + (i * 8 * i / 2); // what
			unlockableLvl[i] = i * 5;
			cards[i].transform.GetChild(0).GetComponent<Text>().text = "LVL " + unlockableLvl[i];
			if(levelSystem.currentLevel >= unlockableLvl[i])
			{
				cards[i].transform.GetChild(0).gameObject.SetActive(false);
				cards[i].GetComponent<Image>().color = Color.white;
			} 
			else if(i > 0)
			{
				cards[i].GetComponent<Image>().color = lockedColor;
			} 
		}
	}
	
	public void CheckIfNewCardUnlocked()
	{
		// display message when a new card is unlocked
		// run this from LevelSystem
		UpdateCardStates();
	}

	public void OnUpgradeButtonPress()
	{
		for(int i = 0; i < cards.Length; i++)
		{
			if(scrollRectSnap.cardToSnapTo.gameObject == cards[i]) // find the index of the current card
			{
				if(levelSystem.currentLevel >= unlockableLvl[i]) // are you the right level for it?
				{
					// spawn that monster
					StartCoroutine(SpawnNewMonster(i));
				} else
				{
					// display some message that you're not the right lvl
				}
			}  
		}
	}

	IEnumerator SpawnNewMonster(int index)
	{
		// check if my monster is currently in an animation
		GameObject currentMonster = GameObject.FindGameObjectWithTag("MyMonster");
		while(currentMonster.GetComponent<Animation>().isPlaying)
		{
			yield return new WaitForSeconds(0.05f);
		}

		Destroy(currentMonster.transform.parent.gameObject);
		GameObject newMonster = GameObject.Instantiate(monsters[index]);
		newMonster = newMonster.transform.GetChild(0).gameObject;
		playerAttack.SetMyMonsterVariables(newMonster);
		GetComponent<BossFightHandler>().SetHealthCanvas(newMonster);
	}

}
