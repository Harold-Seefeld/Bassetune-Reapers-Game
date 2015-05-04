using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class InventorySlot : MonoBehaviour, IDropHandler {

	ItemBase _item;
	WeaponBase _weapon;

	public RectTransform rectTransform;

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void OnDrop(PointerEventData eventData)
	{
		GameObject item = InventoryDrag.itemBeingDragged;

		if (item.GetComponent<ItemBase>())
		{
			_item = item.GetComponent<ItemBase>();

			item.transform.SetParent(transform);
			gameObject.GetComponent<Image>().sprite = item.GetComponent<ItemBase>().itemIcon;
			item.SetActive(false);
		}
		else if (item.GetComponent<WeaponBase>())
		{
			_weapon = item.GetComponent<WeaponBase>();

			item.transform.SetParent(transform);
			gameObject.GetComponent<Image>().sprite = item.GetComponent<WeaponBase>().weaponIcon;
			item.SetActive(false);
		}
	}

	public void OnPointerEnter()
	{
		if (_weapon)
		{
			if(!Popup.instance)
				Debug.LogError("Please make sure you have Popup Gameobject on your scene");

			Popup.instance.gameObject.SetActive (true);
			Popup.instance.Display (rectTransform.position + new Vector3 (0, 70),
		                        	_weapon.weaponName,
		                        	_weapon.weaponType.ToString(),
		                        	_weapon.weaponDescription);
		}
		else if(_item)
		{
			if(!Popup.instance)
				Debug.LogError("Please make sure you have Popup Gameobject on your scene");
			
			Popup.instance.gameObject.SetActive (true);
			Popup.instance.Display (rectTransform.position + new Vector3 (0, 70),
			                        _item.itemName,
			                        _item.itemType.ToString(),
			                        _item.itemDescription);
		}
		else
			return;
	}
}
