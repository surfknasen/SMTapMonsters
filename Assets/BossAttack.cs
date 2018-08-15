using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour {

	public float hitShakeAmount, hitShakeSpeed;

	bool bossFightActive;
	BossFightHandler bossFightHandler;
	Animation bossAnim;
	float hitShakeDuration;
	Transform otherMonster;
	Vector3 otherMonsterOriginalPos;
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
			otherMonster.transform.parent.position = Vector3.Lerp(otherMonster.parent.position, otherMonsterOriginalPos + Random.insideUnitSphere * hitShakeAmount, Time.deltaTime * hitShakeSpeed);
			hitShakeDuration -= Time.deltaTime;
		} else
		{
			otherMonster.transform.parent.position = Vector3.Lerp(otherMonster.parent.position, otherMonsterOriginalPos, Time.deltaTime * hitShakeSpeed);
		}
	}
	
	public void StartBossFight(GameObject boss, GameObject myMonster)
	{
		bossAnim = boss.transform.GetChild(0).GetComponent<Animation>();
		otherMonster = myMonster.transform;
		otherMonsterOriginalPos = myMonster.transform.parent.position;
		stats = boss.transform.GetChild(0).GetComponent<MonsterStats>();
		StartCoroutine(Attack(boss));
	}

	IEnumerator Attack(GameObject boss)
	{
		while(true)
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
