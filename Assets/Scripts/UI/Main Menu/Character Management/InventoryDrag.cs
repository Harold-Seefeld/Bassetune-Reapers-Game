using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class InventoryDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler{
	
	public static GameObject itemBeingDragged;
	
	private Vector3 startPosition;
	private Transform startParent;
	[SerializeField] RectTransform rectTransform;
	
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

	public void OnPointerEnter(PointerEventData eventData)
	{
		if(GetComponent<ItemBase>())
		{
			ItemBase _item = GetComponent<ItemBase>();

			if(!Popup.instance)
				Debug.LogError("Please make sure you have Popup Gameobject on your scene");

			
			Popup.instance.gameObject.SetActive (true);
			Popup.instance.Display (rectTransform.position + new Vector3 (Screen.width / 21, -Screen.width / 85),
			                        _item.itemName, "type needs to be edited in script inventory drag", _item.itemDescription);
		}
		else
			return;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if(!Popup.instance)
			Debug.LogError("Please make sure you have Popup Gameobject on your scene");
		
		Popup.instance.gameObject.SetActive (false);
	}
}
