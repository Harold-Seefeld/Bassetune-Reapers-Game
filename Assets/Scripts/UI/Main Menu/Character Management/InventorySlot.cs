using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler {

    ItemBase _item;

    public RectTransform rectTransform;

    public bool swappable = true;
    public bool clearOnDrop = false;

    public enum SlotType
    {
        Item,
        Offensive_Ability,
        Defensive_Ability,
        Weapon,
        Armor,
        Consumable,
        Ammo,
        Lord,
        Lesser_Lord,
        Minion,
        Trap
    }
    public SlotType slotType;

    public enum SlotTag
    {
        Inventory,
        Ability,
        Mainhand,
        Offhand,
        Armor,
        Ammo
    }
    public List<SlotTag> slotTags = new List<SlotTag>(new SlotTag[] { SlotTag.Inventory });

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject item = InventoryDrag.itemBeingDragged;
        ItemBase thisItemBase = gameObject.GetComponent<ItemBase>();
        InventorySlot thisItemSlot = gameObject.GetComponent<InventorySlot>();
        InventorySlot otherItemSlot = item.GetComponent<InventorySlot>();
        InventoryDrag otherDrag = item.GetComponent<InventoryDrag>();
        ItemBase otherItem = item.GetComponent<ItemBase>();


        // Check if item matches slot type
        if ((slotType == SlotType.Item && !otherItem.isItem()) ||
            (slotType == SlotType.Consumable && !otherItem.isConsumable()) ||
            (slotType == SlotType.Ammo && !otherItem.isAmmo()) ||
            (slotType == SlotType.Weapon && !otherItem.isWeapon()) ||
            (slotType == SlotType.Armor && !otherItem.isArmor()) ||
            (slotType == SlotType.Offensive_Ability && !otherItem.isOffensiveAbility()) ||
            (slotType == SlotType.Defensive_Ability && !otherItem.isDefensiveAbility()))
        { 
            return;
        }

        if (otherItemSlot && thisItemSlot)
        {
            List<SlotTag> tempTags = otherItemSlot.slotTags.GetRange(0, otherItemSlot.slotTags.Count);
            otherItemSlot.slotTags = thisItemSlot.slotTags.GetRange(0, thisItemSlot.slotTags.Count);
            thisItemSlot.slotTags = tempTags;
        }
        
        // Update slots on server
        if (Server.instance)
        {
            JSONObject itemSwap = new JSONObject(JSONObject.Type.OBJECT);
            itemSwap.AddField("characterID", Server.instance.currentDefaultCharacter.CharacterID);
            itemSwap.AddField("target", 0);
            // Slot 1 index
            if (thisItemSlot.transform.parent.name == "Defensive Skills / Items")
            {
                itemSwap.AddField("slot1", 17 + thisItemSlot.transform.GetSiblingIndex());
            }
            else
            {
                itemSwap.AddField("slot1", thisItemSlot.transform.GetSiblingIndex());
            }
            // Slot 2 index
            if (otherItemSlot.transform.name == "Defensive Skills / Items")
            {
                itemSwap.AddField("slot2", 17 + otherItemSlot.transform.GetSiblingIndex());
            }
            else
            {
                itemSwap.AddField("slot2", otherItemSlot.transform.GetSiblingIndex());
            }
            CharacterManager.instance.socket.Emit(SocketIOEvents.Output.Knight.CHANGE_EQUIPPED, itemSwap);
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
        else if (otherDrag && otherItemSlot && swappable && !thisItemBase)
        {
            UpdateInventorySlot(item);

            Destroy(otherDrag.gameObject.GetComponent<ItemBase>());
            otherDrag.gameObject.GetComponent<Image>().sprite = null;
        }
        else
        {
            UpdateInventorySlot(item);
        }
    }

    float doubleClickThreshold = 0.5f;
    float clickTime = 0;
    PointerEventData.InputButton clickType = PointerEventData.InputButton.Left;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != clickType)
        {
            // Reset button and time since last click
            clickType = eventData.button;
            clickTime = 0;
        }

        if (Time.timeSinceLevelLoad - clickTime <= doubleClickThreshold)
        {
            ItemBase itemBase = gameObject.GetComponent<ItemBase>();
            // Double clicked
            if (clickType == PointerEventData.InputButton.Left)
            {
                if (itemBase.isWeapon())
                {
                    bool twoHanded = false;
                    foreach (Weapon.TwoHanded weapon in Enum.GetValues(typeof(Weapon.TwoHanded)))
                    {
                        if (FindWeapon(itemBase.itemID, InventoryManager.instance.items).weaponType.ToString() == weapon.ToString())
                        {
                            twoHanded = true;
                        }
                    }

                    if (twoHanded)
                    {
                        // Equip two-handed
                        SetTag(InventorySlot.SlotTag.Mainhand, true);
                        SetTag(InventorySlot.SlotTag.Offhand, false);
                    }
                    else
                    {
                        // Equip weapon as mainhand
                        SetTag(InventorySlot.SlotTag.Mainhand, true);
                    }
                }
            } 
            else if (clickType == PointerEventData.InputButton.Right)
            {
                if (itemBase.isWeapon())
                {
                    bool twoHanded = false;
                    foreach (Weapon.TwoHanded weapon in Enum.GetValues(typeof(Weapon.TwoHanded)))
                    {
                        if (FindWeapon(itemBase.itemID, InventoryManager.instance.items).weaponType.ToString() == weapon.ToString())
                        {
                            twoHanded = true;
                        }
                    }

                    if (twoHanded)
                    {
                        // Equip two-handed
                        SetTag(InventorySlot.SlotTag.Mainhand, true);
                        SetTag(InventorySlot.SlotTag.Offhand, false);
                    }
                    else
                    {
                        // Equip weapon as mainhand
                        SetTag(InventorySlot.SlotTag.Offhand, true);
                    }
                }
            }
        }
    }

    public void UpdateInventorySlot(GameObject item)
    {
        // not deleting existing itembase for some reason
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

    public Weapon FindWeapon(int itemID, PrefabStore[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].prefabs.Length > 0 && !items[i].prefabs[0].GetComponent<Weapon>())
            {
                continue;
            }
            for (int n = 0; n < items[i].prefabs.Length; n++)
            {
                if (itemID == items[i].prefabs[n].GetComponent<Weapon>().itemID)
                {
                    return items[i].prefabs[n].GetComponent<Weapon>();
                }
            }
        }
        return new Weapon();
    }

    public void SetTag(SlotTag tag, bool overwrite)
    {
        // Linear search array for any existing tags and overwrite them
        foreach (InventorySlot slot in FindObjectsOfType<InventorySlot>())
        {
            if (slot.slotTags.Contains(tag))
            {
                slot.slotTags = new List<SlotTag>(new SlotTag[] { SlotTag.Inventory });
            }
        }

        if (overwrite)
        {
            slotTags = new List<SlotTag>(new SlotTag[] { tag });
        }
        else
        {
            slotTags.Add(tag);
        }
    }
}
