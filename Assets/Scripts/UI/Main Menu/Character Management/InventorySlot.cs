using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class InventorySlot : MonoBehaviour, IDropHandler {

    ItemBase _item;

    public RectTransform rectTransform;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject item = InventoryDrag.itemBeingDragged;

        if (gameObject.GetComponent<ItemBase>())
        {
            Destroy(gameObject.GetComponent<ItemBase>());
        }

        if (gameObject.GetComponent<InventoryDrag>())
        {
            Destroy(gameObject.GetComponent<InventoryDrag>());
        }

        if (item.GetComponent<ItemBase>())
        {
            _item = item.GetComponent<ItemBase>();

            //item.transform.SetParent(transform);
            gameObject.GetComponent<Image>().sprite = item.GetComponent<ItemBase>().itemIcon;
            //item.SetActive(false);
            ItemBase itemBase = gameObject.AddComponent<ItemBase>();
            itemBase.itemID = _item.itemID;
            itemBase.itemName = _item.itemName;
            itemBase.itemIcon = _item.itemIcon;

            gameObject.AddComponent<InventoryDrag>().draggable = false;
        }
    }
}
