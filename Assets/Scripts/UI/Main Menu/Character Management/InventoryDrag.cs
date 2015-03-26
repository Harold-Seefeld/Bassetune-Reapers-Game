using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class InventoryDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	
	public static GameObject itemBeingDragged;
	
	private Vector3 startPosition;
	private Transform startParent;
	
	public void OnBeginDrag(PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	
	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}
	
	public void OnEndDrag (PointerEventData eventData)
	{
		GetComponent<CanvasGroup>().blocksRaycasts = true;

		if (transform.parent == startParent)
		{
			transform.position = startPosition;
		}
	}
}
