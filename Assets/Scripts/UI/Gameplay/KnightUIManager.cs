using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class KnightUIManager : MonoBehaviour {

    public PrefabStore[] items;
    public static KnightUIManager instance;

    public GameObject AbilityPanelUI;
    public GameObject InventoryPanelUI;

    // Use this for initialization
    void Start () {
        instance = this;
	}

    public void UpdateInventory()
    {
        StartCoroutine(UpdateInventoryEnum());
    }

    IEnumerator UpdateInventoryEnum()
    {
        UpdateAbilityBar();
        UpdateKnightInventory();

        Image[] inventorySlots = InventoryPanelUI.GetComponentsInChildren<Image>(true);

        List<Image> abilitySlots = AbilityPanelUI.GetComponentsInChildren<Image>().ToList();
        abilitySlots.RemoveAt(0);
        abilitySlots.RemoveAt(0);
        abilitySlots.RemoveAt(8);

        yield return new WaitForEndOfFrame();

        // Fill remaining item slots with empty item bases
        for (int k = 0; k < inventorySlots.Length; k++)
        {
            if (!inventorySlots[k].GetComponent<ItemBase>())
            {
                inventorySlots[k].gameObject.AddComponent<ItemBase>();
            }
        }

        for (int n = 0; n < abilitySlots.Count; n++)
        {
            // Create an item base slot if its empty
            if (!abilitySlots[n].GetComponent<ItemBase>())
            {
                abilitySlots[n].gameObject.AddComponent<ItemBase>();
            }
        }
    }

    public void UpdateKnightInventory()
    {
        Server server = Server.instance;

        Image[] inventorySlots = InventoryPanelUI.GetComponentsInChildren<Image>(true);

        List<Image> abilitySlots = AbilityPanelUI.GetComponentsInChildren<Image>().ToList();
        abilitySlots.RemoveAt(0);
        abilitySlots.RemoveAt(0);
        abilitySlots.RemoveAt(8);

        for (int i = 0; i < server.players.Length; i++)
        {
            var player = server.players[i];
            if (player.id != server.currentPlayerID)
            {
                continue;
            }

            for (int n = 0; n < inventorySlots.Length; n++)
            {
                Destroy(inventorySlots[n].GetComponent<ItemBase>());
            }

            for (int n = 0; n < player.itemInventory.Count; n++)
            {
                var item = player.itemInventory[n];
                if (item[1].n >= inventorySlots.Length)
                {
                    UpdateAbilityItemBar(item, abilitySlots);
                    continue;
                }
                var itemSlot = inventorySlots[(int)item[1].n].gameObject;

                for (int k = 0; k < items.Length; k++)
                {
                    for (int p = 0; p < items[k].prefabs.Length; p++)
                    {
                        ItemBase itemBase = items[k].prefabs[p].GetComponent<ItemBase>();
                        if (itemBase.itemID == item[0].n)
                        {
                            itemSlot.GetComponent<Image>().sprite = itemBase.itemIcon;

                            if (item[2].n == 9)
                            {
                                itemSlot.GetComponent<InventorySlot>().SetTag(InventorySlot.SlotTag.Both);
                            }
                            else
                            {
                                itemSlot.GetComponent<InventorySlot>().SetTag((InventorySlot.SlotTag)(int)item[2].n);
                            }

                            ItemBase newItem = itemSlot.AddComponent<ItemBase>();
                            newItem.itemID = itemBase.itemID;
                            newItem.itemName = itemBase.itemName;
                            newItem.itemIcon = itemBase.itemIcon;
                            newItem.itemDescription = itemBase.itemDescription;

                            k = items.Length;
                            break;
                        }
                    }
                }
            }
        }
    }

    public void UpdateAbilityItemBar(JSONObject item, List<Image> abilitySlots)
    {
        // Item slot is (-20 + i + 11)
        var itemSlot = abilitySlots[(int)item[1 ].n - 9].gameObject;
        if (itemSlot.GetComponent<ItemBase>())
        {
            Destroy(itemSlot.GetComponent<ItemBase>());
        }

        for (int k = 0; k < items.Length; k++)
        {
            for (int p = 0; p < items[k].prefabs.Length; p++)
            {
                ItemBase itemBase = items[k].prefabs[p].GetComponent<ItemBase>();
                if (itemBase.itemID == item[0].n)
                {
                    itemSlot.GetComponent<Image>().sprite = itemBase.itemIcon;

                    if (item[2].n == 9)
                    {
                        itemSlot.GetComponent<InventorySlot>().SetTag(InventorySlot.SlotTag.Both);
                    }
                    else
                    {
                        itemSlot.GetComponent<InventorySlot>().SetTag((InventorySlot.SlotTag)(int)item[2].n);
                    }

                    ItemBase newItem = itemSlot.AddComponent<ItemBase>();
                    newItem.itemID = itemBase.itemID;
                    newItem.itemName = itemBase.itemName;
                    newItem.itemIcon = itemBase.itemIcon;
                    newItem.itemDescription = itemBase.itemDescription;

                    k = items.Length;
                    break;
                }
            }
        }
    }

    public void UpdateAbilityBar()
    {
        Server server = Server.instance;
        for (int i = 0; i < server.players.Length; i++)
        {
            var player = server.players[i];
            if (player.id != server.currentPlayerID)
            {
                continue;
            }

            List<Image> abilitySlots = AbilityPanelUI.GetComponentsInChildren<Image>().ToList();
            abilitySlots.RemoveAt(0);
            abilitySlots.RemoveAt(0);
            abilitySlots.RemoveAt(8);

            for (int n = 0; n < player.abilityInventory.Count; n++)
            {
                var item = player.abilityInventory[n];
                if (item[1].n >= abilitySlots.Count)
                {
                    continue;
                }

                GameObject itemSlot = abilitySlots[(int)item[1].n].gameObject;

                if (itemSlot.GetComponent<ItemBase>())
                {
                    Destroy(itemSlot.GetComponent<ItemBase>());
                }

                for (int k = 0; k < items.Length; k++)
                {
                    for (int p = 0; p < items[k].prefabs.Length; p++)
                    {
                        ItemBase itemBase = items[k].prefabs[p].GetComponent<ItemBase>();
                        if (itemBase.itemID == item[0].n)
                        {
                            itemSlot.GetComponent<Image>().sprite = itemBase.itemIcon;

                            ItemBase newItem = itemSlot.AddComponent<ItemBase>();
                            newItem.itemID = itemBase.itemID;
                            newItem.itemName = itemBase.itemName;
                            newItem.itemIcon = itemBase.itemIcon;
                            newItem.itemDescription = itemBase.itemDescription;

                            k = items.Length;
                            break;
                        }
                    }
                }
            }
        }
    }
}
