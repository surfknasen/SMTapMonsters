using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public GameObject damageText;
	public Transform damageCanvas;
	public Slider healthSlider;
	public Text healthText;

	private float maxHealth; // set to -1 if this is a training golem
	private bool isTrainingGolem;
	private float currentHealth;
	private GameObject gameManager;
	private bool endedFight;

	void Start () 
	{
		gameManager = GameObject.FindGameObjectWithTag("GameManager");
		maxHealth = GetComponent<MonsterStats>().hp;
		currentHealth = maxHealth;
		damageCanvas = GameObject.FindGameObjectWithTag("DamageCanvas").transform;
		if(healthText != null) healthText.text = "HP: " + currentHealth;
		if(maxHealth == -1) isTrainingGolem = true;
	}

	void Update()
	{
		if(healthSlider != null) LerpHealthSlider();
		if(currentHealth <= 0 && !isTrainingGolem && !endedFight)
		{
			endedFight = true;
			if(healthText != null) healthText.text = "HP: " + 0;
			gameManager.GetComponent<PlayerAttack>().canAttack = false;
			gameManager.GetComponent<PlayerAttack>().StopAllCoroutines();
			if(GameObject.FindGameObjectWithTag("MyMonster") != gameObject)
			{
				gameManager.GetComponent<BossFightHandler>().EndFight(true);
			} else
			{
				gameManager.GetComponent<BossFightHandler>().EndFight(false);
			}
		}
	}

	public void ResetScript()
	{
		currentHealth = maxHealth;
		if(healthText != null) healthText.text = "HP: " + currentHealth;
	}

	void LerpHealthSlider()
	{
		healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth / maxHealth, Time.deltaTime * 5);
	}

	public void TakeDamage(float dmg)
	{
		if(!isTrainingGolem)
		{
			currentHealth -= dmg;
			// set health text
			if(healthText != null) healthText.text = "HP: " + currentHealth;
		}
		CreateDamageText(dmg);
	}

	void CreateDamageText(float dmg)
	{
		GameObject newDamageText = GameObject.Instantiate(damageText);
		newDamageText.transform.SetParent(damageCanvas);
		newDamageText.transform.localScale = new Vector3(1, 1, 1);
		newDamageText.transform.position = transform.position + Random.insideUnitSphere * 1;
		newDamageText.GetComponent<Text>().text = "-" + dmg.ToString() + " HP";
	}
}
