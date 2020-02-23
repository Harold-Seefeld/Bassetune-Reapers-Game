﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var item = InventoryDrag.itemBeingDragged;

        if (item.GetComponent<ItemBase>())
        {
            item.transform.SetParent(transform);
            gameObject.GetComponent<Image>().sprite = item.GetComponent<ItemBase>().itemIcon;
            item.SetActive(false);
        }
        else if (item.GetComponent<WeaponBase>())
        {
            item.transform.SetParent(transform);
            gameObject.GetComponent<Image>().sprite = item.GetComponent<WeaponBase>().weaponIcon;
            item.SetActive(true);
        }
    }
}