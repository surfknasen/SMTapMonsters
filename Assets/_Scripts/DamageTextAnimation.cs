using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextAnimation : MonoBehaviour {

	private float riseSpeed;

	void Start () {
		StartCoroutine(DestroyObject());		
	}

	void Update()
	{
		transform.position += new Vector3(0, .02f, 0);
	}

	IEnumerator DestroyObject()	
	{
		yield return new WaitForSeconds(Random.Range(1f, 1.5f));
		
		Text thisText = GetComponent<Text>();
		Color transparent = thisText.color;
		transparent.a = 0;

		float elapsedTime = 0;
		float duration = 1f;
		while(elapsedTime < duration)
		{
			thisText.color = Color.Lerp(thisText.color, transparent, (elapsedTime / duration));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		Destroy(gameObject);
	}
}
