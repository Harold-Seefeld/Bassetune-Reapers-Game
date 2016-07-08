using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class AbilityMenu : MonoBehaviour
{

    public static AbilityMenu instance;
    private Server server;

    [SerializeField]
    private PrefabStore[] items;

    // Use this for initialization
    void Start()
    {
        instance = this;
        server = FindObjectOfType<Server>();
    }

    // Update is called once per frame
    public void UpdateMenu()
    {
        for (int i = 0; i < server.players.Length; i++)
        {
            var player = server.players[i];
            if (player.id != server.currentPlayerID)
            {
                continue;
            }

            List<Image> inventorySlots = GetComponentsInChildren<Image>().ToList();
            inventorySlots.RemoveAt(0);
            inventorySlots.RemoveAt(0);
            inventorySlots.RemoveAt(8);
            for (int n = 0; n < player.itemInventory.Count; n++)
            {
                var item = player.itemInventory[n];
                if (item[2].n >= inventorySlots.Count)
                {
                    continue;
                }

                GameObject itemSlot = inventorySlots[(int)item[2].n].gameObject;

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
                
                // Create an item base slot if its empty
                if (itemSlot.GetComponent<ItemBase>())
                {
                    itemSlot.AddComponent<ItemBase>();
                }
            }
        }
    }
}
