using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {

	public bool canAttack, autoAttack;
	public Text ultimateAttackText;
	public GameObject fightCanvas;

	public float hitShakeAmount;
	public float hitShakeSpeed;
	public Vector3 myOriginalPos;
	public AudioSource hitSound;

	private GameObject myMonster;
	private Animation myMonsterAnim;

	private Transform otherMonster;
	private Vector3 otherMonsterOriginalPos;
	private float hitShakeDuration;
	private float timeSinceLastHit;

	private MonsterStats myStats;
	private LevelSystem level;
	private float yPos;
	private int hitsSinceLastUltimate;
	private float mouseDownTime;

	void Start () 
	{
		SetMyMonsterVariables(null);

		otherMonster = GameObject.FindGameObjectWithTag("OtherMonster").transform.parent;
		otherMonsterOriginalPos = otherMonster.position;

		level = GetComponent<LevelSystem>();

		canAttack = autoAttack = true;
		myOriginalPos = myMonster.transform.position;

		StartCoroutine(AutoAttack());
	}

	public void SetMyMonsterVariables(GameObject monster)
	{
		if(monster == null)
		{
			myMonster = GameObject.FindGameObjectWithTag("MyMonster");
			myMonsterAnim = myMonster.GetComponent<Animation>();
			myStats = myMonster.GetComponent<MonsterStats>();
		} else
		{
			myMonster = monster;
			myMonsterAnim = myMonster.GetComponent<Animation>();
			myStats = myMonster.GetComponent<MonsterStats>();
		}
		
	}

	public void SetOtherMonsterVariables(GameObject monster)
	{
		otherMonster = monster.transform;
		otherMonsterOriginalPos = otherMonster.position;
		yPos = otherMonsterOriginalPos.y - myOriginalPos.y;
	}

	void Update () 
	{
		timeSinceLastHit += Time.deltaTime;
		if(Input.GetMouseButtonDown(0) && canAttack && !EventSystem.current.IsPointerOverGameObject()) // IF THE PLAYER DOES NOT CLICK, ATTACK AUTOMATICALLY
		{
			timeSinceLastHit = 0;
			StartCoroutine(Attack());
		}

		if(Input.GetMouseButton(0) && BossFightHandler.bossFightActive)
		{
			mouseDownTime += Time.deltaTime;
			if(mouseDownTime > 5 && hitsSinceLastUltimate > 20 && canAttack)
			{
				hitsSinceLastUltimate = 0;
				mouseDownTime = 0;
				if(!ultimateAttackText.gameObject.activeInHierarchy) ShowUltimateAttackText();
				UltimateAttack();
			}
		} else if(hitsSinceLastUltimate > 20)
		{
			if(!ultimateAttackText.gameObject.activeInHierarchy) ShowUltimateAttackText();
		}

		if(hitShakeDuration > 0) // if it's hit
		{
			if(!BossFightHandler.bossFightActive) otherMonster.transform.position = Vector3.Lerp(otherMonster.position, otherMonsterOriginalPos + Random.insideUnitSphere * hitShakeAmount, Time.deltaTime * hitShakeSpeed);
			else otherMonster.transform.position = Vector3.Lerp(otherMonster.position, new Vector3(otherMonsterOriginalPos.x, yPos, otherMonsterOriginalPos.y) + Random.insideUnitSphere * hitShakeAmount, Time.deltaTime * hitShakeSpeed);
			hitShakeDuration -= Time.deltaTime;
		} else
		{
			if(BossFightHandler.bossFightActive) otherMonster.transform.position = Vector3.Lerp(otherMonster.position, new Vector3(otherMonsterOriginalPos.x, yPos, otherMonsterOriginalPos.y), Time.deltaTime * hitShakeSpeed);
			else otherMonster.transform.position = Vector3.Lerp(otherMonster.position, otherMonsterOriginalPos, Time.deltaTime * hitShakeSpeed);
		}
	}

	public IEnumerator AutoAttack()
	{
		while(true)
		{
			if(timeSinceLastHit > 1 && autoAttack)
			{
				// attack
				StartCoroutine(Attack());
				timeSinceLastHit = 0;
			}
			yield return new WaitForSeconds(.5f);
		}
	}

	IEnumerator Attack() // hit other monster 
	{
		if(myMonsterAnim == null) yield break;
		StartCoroutine(AttackDelay());
		yield return new WaitForSeconds(myMonsterAnim.clip.length * 0.2f);
		//PlayHitAudio();
		myMonsterAnim.Stop();
		myMonsterAnim.Play();
		if(BossFightHandler.bossFightActive) hitsSinceLastUltimate++;
		hitShakeDuration = .1f;
		otherMonster.transform.GetChild(0).GetComponent<Health>().TakeDamage(Random.Range(myStats.atkMin, myStats.atkMax + 1));

		// for the future: add random misses. if the attack is successful
		level.AddExp(Random.Range(myStats.atkMin, myStats.atkMax) * level.currentLevel * 4);
	}

	IEnumerator AttackDelay()
	{
		canAttack = false;
		yield return new WaitForSeconds(myMonsterAnim.clip.length * 0.75f);
		hitShakeAmount = 0.3f;
		canAttack = true;
	}

	void PlayHitAudio()
	{
		hitSound.time = 0.5f;
		hitSound.pitch = Random.Range(1f, 1.1f);
		hitSound.Play();
	}

	void ShowUltimateAttackText()
	{
		if(ultimateAttackText.gameObject.activeInHierarchy) return;
		fightCanvas.SetActive(true);
		ultimateAttackText.gameObject.SetActive(true);
		ultimateAttackText.gameObject.GetComponent<Animation>().Play();
	}

	public void HideUltimateAttackText()
	{
		ultimateAttackText.gameObject.GetComponent<Animation>().Stop();
		ultimateAttackText.gameObject.SetActive(false);
		fightCanvas.SetActive(false);
	}

	public void UltimateAttack()
	{
		hitShakeAmount = 2;
		StartCoroutine(Attack());
		hitsSinceLastUltimate = 0;
		for(int i = 0; i < 10; i++)
		{
			otherMonster.transform.GetChild(0).GetComponent<Health>().TakeDamage(Random.Range(myStats.atkMin, myStats.atkMax + 1)); // extra damage too
		}
		ultimateAttackText.gameObject.SetActive(false);
	}

	/*public IEnumerator UltimateAttackDelay()
	{
		while(true)
		{
			ultimateAttackAlreadyPlayed = false;
			yield return new WaitForSeconds(0.5f);
			if(BossFightHandler.bossFightActive && hitsSinceLastUltimate >= 15) ShowUltimateAttackButton();
		}
	}
	*/
}
