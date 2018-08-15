using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

	public RectTransform contentParent;
	public RectTransform[] content;
	public float snapSpeed;
	[HideInInspector]
	public RectTransform cardToSnapTo;
	public float cardsDistance;

	private bool snap, delayActive;
	public bool isScrolling;
	private float[] distance;
	private float shortestDistance;
	public Vector3 lerpTo;

	void Start () 
	{
		distance = new float[content.Length];
	}

	void Update () 
	{
		if(!isScrolling && !delayActive && !snap) // if it's not scrolling
		{
			StartCoroutine(DelayUntilSnap());
		} else if(isScrolling && snap)
		{
			snap = false;
			GetComponent<ScrollRect>().decelerationRate = .75f;
		}
		if(snap)
		{
			SnapContent();
		}
	}

	public void OnBeginDrag(PointerEventData data)
	{
		isScrolling = true;
	}

	public void OnEndDrag(PointerEventData data)
	{
		isScrolling = false;
	}

	IEnumerator DelayUntilSnap()
	{
		delayActive = true;
		yield return new WaitForSeconds(0.15f);
		if(!isScrolling)
		{
			snap = true; // after x sec, if it's not scrolling, start the snap
			GetComponent<ScrollRect>().decelerationRate = 0.005f;
		} 
		delayActive = false;
	}
	
	void SnapContent()
	{
		// get all the distance of all content to the center
		for(int i = 0; i < content.Length; i++)
		{
			distance[i] = Mathf.Abs(transform.position.x - content[i].position.x);
		}
		// get the shortest distance from the array
		shortestDistance = Mathf.Min(distance);
		// find the index, to then find the card
		for(int i = 0; i < distance.Length; i++)
		{
			if(distance[i] == shortestDistance)
			{
				cardToSnapTo = content[i];
				lerpTo = new Vector3(i * -cardsDistance, 0, 0); // 210 is very hardcoded, but MEH... it's the distance between the cards
				break;
			} 
		}

		// lerp
		contentParent.localPosition = Vector3.Lerp(contentParent.localPosition, lerpTo, Time.deltaTime * snapSpeed);
	}

}
