using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSelector : MonoBehaviour {

	public GameObject[] monsterCards;
	public ScrollRectSnap scrollRectSnap;
	public bool[] bossesBeaten;

	private LevelSystem levelSystem;
	[SerializeField]
	private Color lockedColor;

	void Start () 
	{
		bossesBeaten = new bool[monsterCards.Length];
		for(int i = 0; i < bossesBeaten.Length; i++)
		{
			if(i < PlayerPrefs.GetInt("BossesBeaten"))
			{
				bossesBeaten[i] = true;
			} else
			{
				break;
			}
		}

		UpdateCardStates();
	}

	public void SetPlayerPrefs()
	{
		for(int i = 0; i < bossesBeaten.Length; i++)
		{
			if(!bossesBeaten[i])
			{
				PlayerPrefs.SetInt("BossesBeaten", i);
				break;
			}
		}
	}

	void UpdateCardStates()
	{
		for(int i = 1; i < monsterCards.Length; i++)
		{
			if(bossesBeaten[i-1])
			{
				monsterCards[i].GetComponent<Image>().color = Color.white;
			} 
			else
			{
				monsterCards[i].GetComponent<Image>().color = lockedColor;
			} 
		}
	}
	
	public void CheckIfNewBossUnlocked()
	{
		UpdateCardStates();
	}

	public void OnFightBossButtonPress()
	{
		for(int i = 0; i < monsterCards.Length; i++)
		{
			if(scrollRectSnap.cardToSnapTo.gameObject == monsterCards[0])
			{
				GetComponent<BossFightHandler>().StartFight();
				break;
			}
			else if(scrollRectSnap.cardToSnapTo.gameObject == monsterCards[i]) // find the index of the current card
			{
				if(bossesBeaten[i-1]) // are you the right level for it?
				{
					// spawn that monster
					GetComponent<BossFightHandler>().StartFight();
				} else
				{
					// display some message that you're not the right lvl
				}
			}  
		}
	}

}
