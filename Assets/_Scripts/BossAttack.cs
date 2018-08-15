using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour {

	public float hitShakeAmount, hitShakeSpeed;
	public bool canAttack = true;

	BossFightHandler bossFightHandler;
	Animation bossAnim;
	float hitShakeDuration;
	Transform otherMonster;
	Vector3 otherMonsterOriginalPos;
	Vector3 myOriginalPos;
	MonsterStats stats;

	void Start () 
	{
		bossFightHandler = GetComponent<BossFightHandler>();	
		otherMonster = GameObject.FindGameObjectWithTag("MyMonster").transform;
		otherMonsterOriginalPos = otherMonster.transform.parent.position;
	}

	void Update()
	{
		if(hitShakeDuration > 0) // if it's hit
		{
			if (otherMonster != null && BossFightHandler.bossFightActive) otherMonster.transform.parent.position = Vector3.Lerp(otherMonster.parent.position, new Vector3(otherMonsterOriginalPos.x, 0, otherMonsterOriginalPos.z) + Random.insideUnitSphere * hitShakeAmount, Time.deltaTime * hitShakeSpeed);
			hitShakeDuration -= Time.deltaTime;
		} else
		{
			if (otherMonster != null && BossFightHandler.bossFightActive) otherMonster.transform.parent.position = Vector3.Lerp(otherMonster.parent.position, new Vector3(otherMonsterOriginalPos.x, 0, otherMonsterOriginalPos.z), Time.deltaTime * hitShakeSpeed);
		}
	}
	
	public void StartBossFight(GameObject boss, GameObject myMonster)
	{
		myOriginalPos = boss.transform.position;
		bossAnim = boss.transform.GetChild(0).GetComponent<Animation>();
		otherMonster = myMonster.transform;
		otherMonsterOriginalPos = myMonster.transform.parent.position;
		stats = boss.transform.GetChild(0).GetComponent<MonsterStats>();
		StartCoroutine(Attack(boss));
	}

	IEnumerator Attack(GameObject boss)
	{
		yield return new WaitForSeconds(3); // animation delay
		while(true && canAttack)
		{
			if(bossAnim == null) yield break;
			//yield return new WaitForSeconds(bossAnim.clip.length * 0.2f);
	
			bossAnim.Stop();
			bossAnim.Play();

			hitShakeDuration = .1f;
			otherMonster.GetComponent<Health>().TakeDamage(Random.Range(stats.atkMin, stats.atkMax + 1));

			// for the future: add random misses.
			yield return new WaitForSeconds(Random.Range(1f, 3f));
		}
	}


}
