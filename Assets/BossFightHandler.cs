using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightHandler : MonoBehaviour {

	public MenuHandler menuHandler;
	public GameObject[] bossCards;
	public GameObject[] bossPrefabs;
	public List<GameObject> fightUIElementsToShow; // health sliders, texts etc
	public List<GameObject> fightUIElementsToHide;
	public ScrollRectSnap scrollRectSnap;

	private GameObject trainingDummy;
	private GameObject currentBoss;

	void Start () 
	{
		trainingDummy = GameObject.FindGameObjectWithTag("OtherMonster");
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
		// hide training dummy
		trainingDummy.SetActive(false);		
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
		// update the player attack to the new boss
		// show the fight ui elements
		foreach(GameObject g in fightUIElementsToShow)
		{
			g.SetActive(true);
		}
		foreach(GameObject g in fightUIElementsToHide)
		{
			g.SetActive(false);
		}
	}

	public void EndFight(bool won)
	{
		if(won)
		{
			// give exp and stuff
		} 
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
	}
}
