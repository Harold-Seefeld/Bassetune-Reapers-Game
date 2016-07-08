using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class InventorySlot : MonoBehaviour, IDropHandler {

    ItemBase _item;

    public RectTransform rectTransform;

    public bool swappable = true;
    public bool clearOnDrop = false;

    public enum SlotType
    {
        Item,
        Offensive_Ability,
        Defensive_Ability,
    }
    public SlotType slotType;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject item = InventoryDrag.itemBeingDragged;
        ItemBase thisItemBase = gameObject.GetComponent<ItemBase>();
        InventorySlot otherItemSlot = item.GetComponent<InventorySlot>();
        InventoryDrag otherDrag = item.GetComponent<InventoryDrag>();
        ItemBase otherItem = item.GetComponent<ItemBase>();

        // Check if item matches slot type
        if ((slotType == SlotType.Item && (otherItem.itemID < 1000 || otherItem.itemID >= 2500)) ||
            (slotType == SlotType.Offensive_Ability && (otherItem.itemID < 2500 || otherItem.itemID >= 2750)) ||
            (slotType == SlotType.Defensive_Ability && (otherItem.itemID < 2750 || otherItem.itemID >= 3000)))
        { 
            return;
        }

        if (otherDrag && swappable && thisItemBase && otherItemSlot)
        {
            UpdateInventorySlot(item);

            ItemBase newItemBase = item.GetComponent<ItemBase>();
            newItemBase.itemID = thisItemBase.itemID;
            newItemBase.itemName = thisItemBase.itemName;
            newItemBase.itemIcon = thisItemBase.itemIcon;
            newItemBase.itemDescription = thisItemBase.itemDescription;
            newItemBase.itemBuyPrice = thisItemBase.itemBuyPrice;
            newItemBase.itemSellPrice = thisItemBase.itemSellPrice;

            item.GetComponent<Image>().sprite = newItemBase.itemIcon;

            InventoryDrag.swapped = true;
        }
        else if (otherDrag && swappable && !thisItemBase)
        {
            UpdateInventorySlot(item);
            //InventoryDrag.swapped = true;
        }
        else
        {
            UpdateInventorySlot(item);
        }
    }

    private void UpdateInventorySlot(GameObject item)
    {
        if (gameObject.GetComponent<ItemBase>())
        {
            Destroy(gameObject.GetComponent<ItemBase>());
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
            itemBase.itemDescription = _item.itemDescription;
            itemBase.itemBuyPrice = _item.itemBuyPrice;
            itemBase.itemSellPrice = _item.itemSellPrice;
        }
    }
}
