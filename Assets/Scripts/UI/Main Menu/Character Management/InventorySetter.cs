using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventorySetter : MonoBehaviour
{

    private string slotInventorySite = "/slots";
    private ClientData clientData;

    public enum SlotType
    {
        knight_slots,
        ability_slots,
        lord_slots,
    }
    public SlotType slotType;
    public InventoryManager inventoryManager;

    public static InventorySetter instanceAbility;
    public static InventorySetter instanceInventory;
    public static InventorySetter instanceDungeon;
    public static InventorySetter instance;

    public void Start()
    {
        if (gameObject.name == "Ability List") instanceAbility = this;
        if (gameObject.name == "Inventory List") instanceInventory = this;
        if (gameObject.name == "Dungeon List") instanceDungeon = this;

        instance = this;

        slotInventorySite = inventoryManager.server + slotInventorySite;
        clientData = FindObjectOfType<ClientData>() as ClientData;
    }

    public static void SetInventory()
    {
        // Updates both inventories for knights
        // instanceAbility returns an object containing any items stored in the array
        instanceAbility.SendAbilityInventory();
        instanceInventory.SendItemInventory();
        instanceDungeon.SendDungeonInventory();
    }

    public void SendDungeonInventory()
    {
        List<JSONObject> existingInventory = InventoryManager.instance.inventoryJSON["lord"].list;
        Image[] slots = GetComponentsInChildren<Image>();
        foreach (Transform slotObject in transform)
        {
            ItemBase item = slotObject.GetComponent<ItemBase>();
            int itemTag = (int)slotObject.GetComponent<InventorySlot>().slotTag;
            int itemID = 0;
            if (item)
            {
                itemID = item.itemID;
            }
            int slotID = slotObject.transform.GetSiblingIndex();

            bool foundSlot = false;
            for (int i = 0; i < existingInventory.Count; i++)
            {
                // Check if the same slot of existing inventory
                if (existingInventory[i].list[1].n == slotID)
                {
                    foundSlot = true;

                    // Check whether itemID or slot has changed
                    if (existingInventory[i].list[0].n == itemID && existingInventory[i].list[2].n == itemTag)
                    {
                        break;
                    }
                    else
                    {
                        WWWForm www = new WWWForm();
                        www.AddField("uuid", clientData.GetSession());
                        www.AddField("slotType", "lord_slots");
                        www.AddField("dungeonID", (InventoryManager.instance.selectedDungeon).ToString());
                        www.AddField("itemSlot", slotID);
                        www.AddField("itemID", itemID);
                        www.AddField("itemTag", itemTag);
                        WWW w = new WWW(slotInventorySite, www.data);
                        StartCoroutine(SetInventorySlot(w));

                        existingInventory[i].list[0].n = itemID;
                        existingInventory[i].list[2].n = itemTag;

                        break;
                    }
                }
            }

            if (!foundSlot && itemID != 0)
            {
                WWWForm www = new WWWForm();
                www.AddField("uuid", clientData.GetSession());
                www.AddField("slotType", "lord_slots");
                www.AddField("dungeonID", (InventoryManager.instance.selectedDungeon).ToString());
                www.AddField("itemSlot", slotID);
                www.AddField("itemID", itemID);
                WWW w = new WWW(slotInventorySite, www.data);
                StartCoroutine(SetInventorySlot(w));

                JSONObject itemInfo = new JSONObject(JSONObject.Type.ARRAY);
                itemInfo.Add(itemID);
                itemInfo.Add(slotID);
                itemInfo.Add(itemTag);
                existingInventory.Add(itemInfo);
            }
        }
    }

    public void SendAbilityInventory()
    {
        List<JSONObject> existingInventory = null;
        Image[] slots = GetComponentsInChildren<Image>();
        foreach (Transform slotObject in transform)
        {
            ItemBase item = slotObject.GetComponent<ItemBase>();
            int itemID = 0;
            if (item)
            {
                itemID = item.itemID;
            }
            int slotID = slotObject.transform.GetSiblingIndex();

            if (slotID <= 10)
            {
                bool foundSlot = false;
                existingInventory = InventoryManager.instance.inventoryJSON["ability"].list;
                for (int i = 0; i < existingInventory.Count; i++)
                {
                    // Check if the same slot of existing inventory
                    if (existingInventory[i].list[1].n == slotID)
                    {
                        foundSlot = true;

                        // Check whether itemID has changed
                        if (existingInventory[i].list[0].n == itemID)
                        {
                            break;
                        }
                        else
                        {
                            WWWForm www = new WWWForm();
                            www.AddField("uuid", clientData.GetSession());
                            www.AddField("slotType", "ability_slots");
                            www.AddField("itemSlot", slotID);
                            www.AddField("itemID", itemID);
                            WWW w = new WWW(slotInventorySite, www.data);
                            StartCoroutine(SetInventorySlot(w));

                            existingInventory[i].list[0].n = itemID;

                            break;
                        }
                    }
                }

                if (!foundSlot && itemID != 0)
                {
                    WWWForm www = new WWWForm();
                    www.AddField("uuid", clientData.GetSession());
                    www.AddField("slotType", "ability_slots");
                    www.AddField("itemSlot", slotID);
                    www.AddField("itemID", itemID);
                    WWW w = new WWW(slotInventorySite, www.data);
                    StartCoroutine(SetInventorySlot(w));

                    JSONObject itemInfo = new JSONObject(JSONObject.Type.ARRAY);
                    itemInfo.Add(itemID);
                    itemInfo.Add(slotID);
                    existingInventory.Add(itemInfo);
                }
            }
            else
            {
                existingInventory = InventoryManager.instance.inventoryJSON["knight"].list;
                int itemTag = (int)slotObject.GetComponent<InventorySlot>().slotTag;
                slotID = slotID + 9;

                bool foundSlot = false;
                for (int i = 0; i < existingInventory.Count; i++)
                {
                    // Check if the same slot of existing inventory
                    if (existingInventory[i].list[1].n == slotID)
                    {
                        foundSlot = true;

                        // Check whether itemID or slot has changed
                        if (existingInventory[i].list[0].n == itemID && existingInventory[i].list[2].n == itemTag)
                        {
                            break;
                        }
                        else
                        {
                            WWWForm www = new WWWForm();
                            www.AddField("uuid", clientData.GetSession());
                            www.AddField("slotType", "knight_slots");
                            www.AddField("itemSlot", slotID);
                            www.AddField("itemID", itemID);
                            www.AddField("itemTag", itemTag);
                            WWW w = new WWW(slotInventorySite, www.data);
                            StartCoroutine(SetInventorySlot(w));

                            existingInventory[i].list[0].n = itemID;
                            existingInventory[i].list[2].n = itemTag;

                            break;
                        }
                    }
                }

                if (!foundSlot && itemID != 0)
                {
                    WWWForm www = new WWWForm();
                    www.AddField("uuid", clientData.GetSession());
                    www.AddField("slotType", "knight_slots");
                    www.AddField("itemSlot", slotID);
                    www.AddField("itemID", itemID);
                    www.AddField("itemTag", itemTag);
                    WWW w = new WWW(slotInventorySite, www.data);
                    StartCoroutine(SetInventorySlot(w));

                    JSONObject itemInfo = new JSONObject(JSONObject.Type.ARRAY);
                    itemInfo.Add(itemID);
                    itemInfo.Add(slotID);
                    itemInfo.Add(itemTag);
                    existingInventory.Add(itemInfo);
                }
            }
        }
    }

    public void SendItemInventory()
    {
        List<JSONObject> existingInventory = InventoryManager.instance.inventoryJSON["knight"].list;
        Image[] slots = GetComponentsInChildren<Image>();
        foreach (Transform slotObject in transform)
        {
            ItemBase item = slotObject.GetComponent<ItemBase>();
            int itemTag = (int)slotObject.GetComponent<InventorySlot>().slotTag;
            int itemID = 0;
            if (item)
            {
                itemID = item.itemID;
            }
            int slotID = slotObject.transform.GetSiblingIndex();

            bool foundSlot = false;
            for (int i = 0; i < existingInventory.Count; i++)
            {
                // Check if the same slot of existing inventory
                if (existingInventory[i].list[1].n == slotID)
                {
                    foundSlot = true;

                    // Check whether itemID or slot has changed
                    if (existingInventory[i].list[0].n == itemID && existingInventory[i].list[2].n == itemTag)
                    {
                        break;
                    }
                    else
                    {
                        WWWForm www = new WWWForm();
                        www.AddField("uuid", clientData.GetSession());
                        www.AddField("slotType", "knight_slots");
                        www.AddField("itemSlot", slotID);
                        www.AddField("itemID", itemID);
                        www.AddField("itemTag", itemTag);
                        WWW w = new WWW(slotInventorySite, www.data);
                        StartCoroutine(SetInventorySlot(w));

                        existingInventory[i].list[0].n = itemID;
                        existingInventory[i].list[2].n = itemTag;

                        break;
                    }
                }
            }

            if (!foundSlot && itemID != 0)
            {
                WWWForm www = new WWWForm();
                www.AddField("uuid", clientData.GetSession());
                www.AddField("slotType", "knight_slots");
                www.AddField("itemSlot", slotID);
                www.AddField("itemID", itemID);
                www.AddField("itemTag", itemTag);
                WWW w = new WWW(slotInventorySite, www.data);
                StartCoroutine(SetInventorySlot(w));

                JSONObject itemInfo = new JSONObject(JSONObject.Type.ARRAY);
                itemInfo.Add(itemID);
                itemInfo.Add(slotID);
                itemInfo.Add(itemTag);
                existingInventory.Add(itemInfo);
            }
        }
    }

    IEnumerator SetInventorySlot(WWW w)
    {
        yield return w;

        if (w.text != "Successfully Updated.")
        {
            inventoryManager.notificationRect.transform.gameObject.SetActive(true);
            inventoryManager.notificationRect.SetAsLastSibling();
            inventoryManager.notificationText.text = "An error occurred";
            inventoryManager.notificationButton.onClick.RemoveAllListeners();
            inventoryManager.notificationButton.onClick.AddListener(() => { inventoryManager.notificationRect.transform.gameObject.SetActive(false); }); ;
        }

        Debug.Log(w.text);
    }

    void ResetInventory()
    {
        Image[] inventoryIcons = GetComponentsInChildren<Image>();

        for (int i = 0; i < inventoryIcons.Length; i++)
        {
            inventoryIcons[i].sprite = inventoryIcons[i].GetComponent<InventorySlot>().defaultSlotImage;

            if (inventoryIcons[i].GetComponent<ItemBase>())
            {
                Destroy(inventoryIcons[i].GetComponent<ItemBase>());
            }
            else if (inventoryIcons[i].GetComponent<Ability>())
            {
                Destroy(inventoryIcons[i].GetComponent<Ability>());
            }
        }
    }
}