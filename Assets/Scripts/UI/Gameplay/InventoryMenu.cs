using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour {

    public static InventoryMenu instance;
    private Server server;

    [SerializeField] private PrefabStore[] items;

	// Use this for initialization
	void Start () {
        instance = this;
        server = FindObjectOfType<Server>();

        transform.parent.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	public void UpdateMenu () {
        for (int i = 0; i < server.players.Length; i++)
        {
            var player = server.players[i];
            if (player.id != server.currentPlayerID)
            {
                continue;
            }

            Image[] inventorySlots = GetComponentsInChildren<Image>();
            for (int n = 0; n < player.itemInventory.Count; n++)
            {
                var item = player.itemInventory[n];
                if (item[2].n >= inventorySlots.Length)
                {
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
}
