﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour {

	public Slider expSlider;
	public Text expText, lvlText;

	[HideInInspector]
	public int currentLevel = 1;
	private float nextLevelExp = 20;
	private float currentExp;

	private MonsterSelector selector;

	void Start()
	{
		currentLevel = PlayerPrefs.GetInt("Level");
		currentExp = PlayerPrefs.GetInt("CurrentExp");
		lvlText.text = "LVL " + currentLevel;
		nextLevelExp = 20 * currentLevel * (currentLevel + 1) / 2;
		expText.text = currentExp + " / " + nextLevelExp;
		selector = GetComponent<MonsterSelector>();
		//selector.UpdateCardStates();
	}

	void Update()
	{
		UpdateSlider(currentExp / nextLevelExp);
	}
	

	public void AddExp(int amount)
	{
		currentExp += amount;
		lvlText.text = "LVL " + currentLevel;
		expText.text = currentExp + " / " + nextLevelExp;
		if(currentExp >= nextLevelExp)
		{
			LevelUp(currentExp-nextLevelExp);
		}
		PlayerPrefs.SetInt("CurrentExp", (int)currentExp);
	}


	void LevelUp(float spareExp)
	{
		//nextLevelExp = 20 * (int)Mathf.Pow(2, currentLevel - 1);
		nextLevelExp = 20 * currentLevel * (currentLevel + 1) / 2;
		currentLevel++;
		currentExp = 0;
		PlayerPrefs.SetInt("Level", currentLevel);
		AddExp((int)spareExp);
		selector.CheckIfNewCardUnlocked();
		StartCoroutine(LvlTextAnimation());
		if(spareExp > 0) return;
		UpdateSlider(0);
		lvlText.text = "LVL " + currentLevel;
		expText.text = "0 / " + nextLevelExp;
		
	}

	IEnumerator LvlTextAnimation()
	{
		float time = 0.25f;
		float elapsedTime = 0;
		while(elapsedTime < time)
		{
			lvlText.transform.localScale = Vector3.Lerp(lvlText.transform.localScale, new Vector3(1.2f, 1.2f, 1.2f), (elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		elapsedTime = 0;
		while(elapsedTime < time)
		{
			lvlText.transform.localScale = Vector3.Lerp(lvlText.transform.localScale, new Vector3(1, 1, 1), (elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}

	void UpdateSlider(float newValue)
	{
		expSlider.value = Mathf.MoveTowards(expSlider.value, newValue, Time.deltaTime );
	}
}
