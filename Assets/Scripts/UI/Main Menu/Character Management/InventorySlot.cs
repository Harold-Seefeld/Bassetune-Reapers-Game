using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class InventorySlot : MonoBehaviour, IDropHandler {

	public void OnDrop(PointerEventData eventData)
	{
		InventoryDrag.itemBeingDragged.transform.SetParent(transform);
	}

}
