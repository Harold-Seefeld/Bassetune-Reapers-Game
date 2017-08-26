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
        boss_slots,
        lord_slots,
    }
    public SlotType slotType;

    public Sprite defaultImage;
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
        JSONObject items = instanceAbility.SendAbilityInventory();
        instanceInventory.SendItemInventory(items);
    }

    public static void SetDungeonInventory()
    {
        instanceDungeon.SendDungeonInventory();
    }

    public void SendDungeonInventory()
    {
        ItemBase[] itemBases = GetComponentsInChildren<ItemBase>(true);
        // Create a json object for storing the json arrays
        JSONObject jsonObject = new JSONObject(JSONObject.Type.ARRAY);
        for (int n = 0; n < itemBases.Length; n++)
        {
            // Create a new JSON array for storing the fields
            JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
            // Add the item ID
            arr.Add(itemBases[n].itemID.ToString());
            // Set slot number
            arr.Add(itemBases[n].transform.GetSiblingIndex());
            // Add the position of the inventory and add it to the main json object
            jsonObject.Add(arr);
        }

        WWWForm www = new WWWForm();
        www.AddField("uuid", clientData.GetSession());
        www.AddField("slotType", slotType.ToString());
        www.AddField("dungeonSelected", (InventoryManager.instance.selectedDungeon).ToString());
        www.AddField("j", jsonObject.Print());
        Debug.Log(jsonObject.Print());
        WWW w = new WWW(slotInventorySite, www.data);
        StartCoroutine(SetInventorySlot(w));
    }

    public JSONObject SendAbilityInventory()
    {
        /*
        ItemBase[] itemBases = GetComponentsInChildren<ItemBase>(true);
        // Create a json object for storing the json arrays
        JSONObject jsonObject = new JSONObject(JSONObject.Type.ARRAY);
        JSONObject itemObject = new JSONObject(JSONObject.Type.ARRAY);
        for (int n = 0; n < itemBases.Length; n++)
        {
            // Create a new JSON array for storing the fields
            JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
            // Add the item ID
            arr.Add(itemBases[n].itemID.ToString());
            // Identify whether abilities or items
            int slotIndex = itemBases[n].transform.GetSiblingIndex();
            if (slotIndex > 10)
            {
                // Items
                // TODO: Add item count
                arr.Add(1);
                // Set slot number, for items adds onto the end (20 + i - 11)
                arr.Add(9 + itemBases[n].transform.GetSiblingIndex());
                // Set the slot tag
                InventorySlot slot = itemBases[n].GetComponent<InventorySlot>();
                if (slot.slotTag.Contains(InventorySlot.SlotTag.Mainhand) && slot.slotTag.Contains(InventorySlot.SlotTag.Offhand))
                {
                    arr.Add(9);
                }
                else
                {
                    arr.Add((int)slot.slotTag[0]);
                }
                itemObject.Add(arr);
            }
            else
            {
                // Abilities
                // Set slot number
                arr.Add(itemBases[n].transform.GetSiblingIndex());
                // Add the position of the inventory and add it to the main json object
                jsonObject.Add(arr);
            }
        }

        WWWForm www = new WWWForm();
        www.AddField("uuid", clientData.GetSession());
        www.AddField("slotType", slotType.ToString());
        www.AddField("j", jsonObject.Print());
        Debug.Log(jsonObject.Print());
        WWW w = new WWW(slotInventorySite, www.data);
        StartCoroutine(SetInventorySlot(w));

        return itemObject;
        */
        return null;
    }

    public void SendItemInventory(JSONObject items)
    {
        // LOOP THROUGH TO CHECK FOR CHANGES AND SEND THOSE TODO: UPDATE EXISTING INVENTORY SO THAT IT DOESENT USE OLD SLOTS WHEN CHANGING MULTIPLE TIMES
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

                    // Check whether itemID has changed
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
            inventoryIcons[i].sprite = defaultImage;

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