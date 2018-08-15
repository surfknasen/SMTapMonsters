using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour {

	public bool canAttack, autoAttack;

	public float hitShakeAmount;
	public float hitShakeSpeed;

	private GameObject myMonster;
	private Animation myMonsterAnim;

	private Transform otherMonster;
	private Vector3 otherMonsterOriginalPos;
	private float hitShakeDuration;
	private float timeSinceLastHit;

	private MonsterStats myStats;
	private LevelSystem level;
	private float yPos;
	public Vector3 myOriginalPos;

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

	IEnumerator AutoAttack()
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
		
		myMonsterAnim.Stop();
		myMonsterAnim.Play();

		hitShakeDuration = .1f;
		otherMonster.transform.GetChild(0).GetComponent<Health>().TakeDamage(Random.Range(myStats.atkMin, myStats.atkMax + 1));

		// for the future: add random misses. if the attack is successful
		level.AddExp(myStats.atkMax * 2);
	}

	IEnumerator AttackDelay()
	{
		canAttack = false;
		yield return new WaitForSeconds(myMonsterAnim.clip.length * 0.75f);
		canAttack = true;
	}
}
