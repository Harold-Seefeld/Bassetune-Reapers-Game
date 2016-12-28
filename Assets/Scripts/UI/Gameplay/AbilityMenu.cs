using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class AbilityMenu : MonoBehaviour
{

    public static AbilityMenu instance;

    [SerializeField]
    private PrefabStore[] items;

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    public void UpdateMenu()
    {
        Server server = Server.instance;
        for (int i = 0; i < server.players.Length; i++)
        {
            var player = server.players[i];
            if (player.id != server.currentPlayerID)
            {
                continue;
            }

            List<Image> abilitySlots = GetComponentsInChildren<Image>().ToList();
            abilitySlots.RemoveAt(0);
            abilitySlots.RemoveAt(0);
            abilitySlots.RemoveAt(8);

            for (int n = 0; n < player.abilityInventory.Count; n++)
            {
                var item = player.abilityInventory[n];
                if (item[2].n >= abilitySlots.Count)
                {
                    continue;
                }

                GameObject itemSlot = abilitySlots[(int)item[2].n].gameObject;

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

            for (int n = 0; n < abilitySlots.Count; n++)
            {
                // Create an item base slot if its empty
                if (!abilitySlots[n].GetComponent<ItemBase>())
                {
                    abilitySlots[n].gameObject.AddComponent<ItemBase>();
                }
            }
        }
    }
}
