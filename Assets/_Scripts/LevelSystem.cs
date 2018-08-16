using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour {

	public Slider expSlider;
	public Text expText;

	[HideInInspector]
	public int currentLevel = 1;
	private float nextLevelExp = 20;
	private float currentExp;

	private MonsterSelector selector;

	void Start()
	{
		selector = GetComponent<MonsterSelector>();
	}

	void Update()
	{
		UpdateSlider(currentExp / nextLevelExp);
	}
	

	public void AddExp(int amount)
	{
		currentExp += amount;
		expText.text = "LVL " + currentLevel + "  -  " + currentExp + " / " + nextLevelExp;
		if(currentExp >= nextLevelExp)
		{
			LevelUp(currentExp-nextLevelExp);
		}
	}


	void LevelUp(float spareExp)
	{
		currentLevel++;
		nextLevelExp = 20 * (int)Mathf.Pow(2, currentLevel - 1);
		currentExp = 0;
		AddExp((int)spareExp);
		selector.CheckIfNewCardUnlocked();
		if(spareExp > 0) return;
		UpdateSlider(0);
		expText.text = "LVL " + currentLevel + "  -  " + "0 / " + nextLevelExp;
	}

	void UpdateSlider(float newValue)
	{
		expSlider.value = Mathf.Lerp(expSlider.value, newValue, Time.deltaTime * 5);
	}
}
