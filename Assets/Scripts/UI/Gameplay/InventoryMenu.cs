using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class InventoryMenu : MonoBehaviour {

    public static InventoryMenu instance;

    [SerializeField] private PrefabStore[] items;

	// Use this for initialization
	void Start () {
        instance = this;

        transform.parent.gameObject.SetActive(false);
    }
	
	public void UpdateMenu ()
    {
        Server server = Server.instance;
        for (int i = 0; i < server.players.Length; i++)
        {
            var player = server.players[i];
            if (player.id != server.currentPlayerID)
            {
                continue;
            }

            Image[] inventorySlots = GetComponentsInChildren<Image>();
            List<Image> abilitySlots = AbilityMenu.instance.GetComponentsInChildren<Image>().ToList();
            abilitySlots.RemoveAt(0);
            abilitySlots.RemoveAt(0);
            abilitySlots.RemoveAt(8);

            for (int n = 0; n < player.itemInventory.Count; n++)
            {
                var item = player.itemInventory[n];
                if (item[2].n >= inventorySlots.Length)
                {
                    UpdateAbilityBar(item, abilitySlots);
                    continue;
                }
                var itemSlot = inventorySlots[(int)item[2].n].gameObject;

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

                            if (item[3].n == 9)
                            {
                                itemSlot.GetComponent<InventorySlot>().SetTag(InventorySlot.SlotTag.Mainhand, true);
                                itemSlot.GetComponent<InventorySlot>().SetTag(InventorySlot.SlotTag.Offhand, false);
                            }
                            else
                            {
                                itemSlot.GetComponent<InventorySlot>().SetTag((InventorySlot.SlotTag)(int)item[3].n, true);
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

            // Fill remaining item slots with empty item bases
            for (int k = 0; k < inventorySlots.Length; k++)
            {
                if (!inventorySlots[k].GetComponent<ItemBase>())
                {
                    inventorySlots[k].gameObject.AddComponent<ItemBase>();
                }
            }
        }
	}

    public void UpdateAbilityBar(JSONObject item, List<Image> abilitySlots)
    {
        // Item slot is (-20 + i + 11)
        var itemSlot = abilitySlots[(int)item[2].n - 9].gameObject;
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

                    if (item[3].n == 9)
                    {
                        itemSlot.GetComponent<InventorySlot>().SetTag(InventorySlot.SlotTag.Mainhand, true);
                        itemSlot.GetComponent<InventorySlot>().SetTag(InventorySlot.SlotTag.Offhand, false);
                    }
                    else
                    {
                        itemSlot.GetComponent<InventorySlot>().SetTag((InventorySlot.SlotTag)(int)item[3].n, true);
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
