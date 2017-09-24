using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{

    public string server = "bassetune.com";
    private string getInventorySite = "";
    private string setInventorySite = "";
    private string buyFeatureSite = "";

    // Stores current JSON from the server
    public JSONObject inventoryJSON;

    // UI References
    public GameObject shopPanel;
    public GameObject inventoryPanel;
    public GameObject inventorySetPanel;
    public Text notificationText;
    public Button notificationButton;
    public RectTransform notificationRect;
    public GameObject abilitySlotPanel;
    public GameObject dungeonSelectorPanel;
    public GameObject dungeonSlotPanel;

    // Label to create an entry
    public GameObject labelPrefab;

    // Buttons for selecting between dungeon compositions
    public GameObject compositionSelectorPrefab;

    // All items and abilities
    public PrefabStore[] items;

    public static InventoryManager instance = null;

    [SerializeField]
    private ClientData clientData;

    public int selectedDungeon = 0;

    void Start()
    {
        instance = this;

        setInventorySite = server + "/setInventory";
        getInventorySite = server + "/getInventory";
        buyFeatureSite = server + "/purchaseFeature";

        clientData = FindObjectOfType<ClientData>() as ClientData;
        UpdateInventory();

        ShowShop("ability");
    }

    public void UpdateInventory()
    {
        WWWForm www = new WWWForm();
        www.AddField("uuid", clientData.GetSession());
        WWW w = new WWW(getInventorySite, www.data);
        StartCoroutine(UpdateInventory(w));
    }

    IEnumerator UpdateInventory(WWW w)
    {
        yield return w;
        inventoryJSON = new JSONObject(w.text);
        Debug.Log(inventoryJSON.Print());

        ShowInventory("ability");

        // Update Selector slots (dungeon defaults to show dungeon 1)
        DisplayInventory();
        DisplayAbility();
        DisplayDungeonSelector();
        DisplayDungeon(0);

        Debug.Log("Downloaded Inventory Successfully.");
    }

    private ItemBase GetItemInfo(string panel, int id)
    {
        for (int n = 0; n < items.Length; n++)
        {
            if (items[n].prefabs.Length > 0)
            {
                if (panel == "weapon" && !items[n].prefabs[0].GetComponent<Weapon>())
                {
                    continue;
                }
                if (panel == "consumable" && !items[n].prefabs[0].GetComponent<Consumable>())
                {
                    continue;
                }
                if (panel == "equipable" && !items[n].prefabs[0].GetComponent<Equipable>())
                {
                    continue;
                }
                if (panel == "ability" && !items[n].prefabs[0].GetComponent<Ability>())
                {
                    continue;
                }
                for (int i = 0; i < items[n].prefabs.Length; i++)
                {
                    ItemBase item = items[n].prefabs[i].GetComponent<ItemBase>();
                    if (items[n].prefabs[i].GetComponent<ItemBase>().itemID == id)
                    {
                        return item;
                    }
                }
            }
        }
        return null;
    }

    public void ShowInventory(string panel)
    {
        int inventoryLength = inventoryJSON["purchased"].Count;
        // Clear previous entries in inventory
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }
        // Add all items based on respective list
        for (int n = 0; n < items.Length; n++)
        {
            if (items[n].prefabs.Length <= 0)
            {
                continue;
            }

            if (panel == "weapon" && !items[n].prefabs[0].GetComponent<Weapon>())
            {
                continue;
            }
            if (panel == "consumable" && !items[n].prefabs[0].GetComponent<Consumable>())
            {
                continue;
            }
            if (panel == "equipable" && !items[n].prefabs[0].GetComponent<Equipable>())
            {
                continue;
            }
            if (panel == "ability" && !items[n].prefabs[0].GetComponent<Ability>())
            {
                continue;
            }

            for (int a = 0; a < items[n].prefabs.Length; a++)
            {
                for (int i = 0; i < inventoryLength; i++)
                {
                    // Create Entry
                    ItemBase item = items[n].prefabs[a].GetComponent<ItemBase>();
                    if (inventoryJSON["purchased"].list[i].n != item.itemID)
                    {
                        continue;
                    }

                    GameObject newObject = Instantiate(labelPrefab);
                    newObject.transform.SetParent(inventoryPanel.transform);

                    newObject.GetComponentInChildren<Text>().text = item.itemName;
                    newObject.GetComponentsInChildren<Image>()[1].sprite = item.itemIcon;
                    ItemBase itemBase = newObject.AddComponent<ItemBase>();
                    itemBase.itemID = item.itemID;
                    itemBase.itemName = item.itemName;
                    itemBase.itemIcon = item.itemIcon;
                    itemBase.itemDescription = item.itemDescription;
                    itemBase.itemCount = item.itemCount;
                    itemBase.itemBuyPrice = item.itemBuyPrice;
                    itemBase.itemSellPrice = item.itemSellPrice;
                }
            }
        }
    }

    public void ShowShop(string panel)
    {
        // Clear previous entries in inventory
        foreach (Transform child in shopPanel.transform)
        {
            Destroy(child.gameObject);
        }
        // Add all items based on respective list
        for (int n = 0; n < items.Length; n++)
        {
            if (items[n].prefabs.Length > 0)
            {
                if (panel == "weapon" && !items[n].prefabs[0].GetComponent<Weapon>())
                {
                    continue;
                }
                if (panel == "consumable" && !items[n].prefabs[0].GetComponent<Consumable>())
                {
                    continue;
                }
                if (panel == "equipable" && !items[n].prefabs[0].GetComponent<Equipable>())
                {
                    continue;
                }
                if (panel == "ability" && !items[n].prefabs[0].GetComponent<Ability>())
                {
                    continue;
                }
                for (int i = 0; i < items[n].prefabs.Length; i++)
                {
                    // Create Entry
                    ItemBase item = items[n].prefabs[i].GetComponent<ItemBase>();
                    GameObject newObject = Instantiate(labelPrefab);
                    newObject.transform.SetParent(shopPanel.transform);

                    newObject.GetComponentInChildren<Text>().text = item.itemName;
                    newObject.GetComponentsInChildren<Image>()[1].sprite = item.itemIcon;
                    ItemBase itemBase = newObject.AddComponent<ItemBase>();
                    itemBase.itemID = item.itemID;
                    itemBase.itemName = item.itemName;
                    itemBase.itemIcon = item.itemIcon;
                    itemBase.itemDescription = item.itemDescription;
                    itemBase.itemCount = item.itemCount;
                    itemBase.itemBuyPrice = item.itemBuyPrice;
                    itemBase.itemSellPrice = item.itemSellPrice;

                    newObject.GetComponent<InventoryDrag>().draggable = false;
                }
            }
        }
    }

    public void SortInventory(string filter)
    {
        // TODO: Edit to include filters for sorting the inventory
    }

    public void SortShop(string filter)
    {
        // TODO: Edit to include filters for sorting the inventory
    }

    public void BuyItem(int itemIndex, int itemAmount)
    {
        WWWForm www = new WWWForm();
        www.AddField("uuid", clientData.GetSession());
        www.AddField("amount", itemAmount);
        www.AddField("commandType", "Buy");
        www.AddField("item", itemIndex);
        WWW w = new WWW(setInventorySite, www.data);
        StartCoroutine(BuyItem(w));
    }

    IEnumerator BuyItem(WWW w)
    {
        yield return w;

        if (w.text == "Successfully Purchased.")
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "Successfully Purchased.";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
            UpdateInventory();
        }
        else if (w.text == "Account ID is undefined.")
        {
            Application.Quit();
        }
        else if (w.text == "Not enough gold.")
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "Not enough gold.";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
        }
        else
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "An error occurred";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
        }
    }

    public void SellItem(int itemIndex, int itemAmount)
    {
        WWWForm www = new WWWForm();
        www.AddField("uuid", clientData.GetSession());
        www.AddField("amount", itemAmount);
        www.AddField("commandType", "Sell");
        www.AddField("item", itemIndex);
        WWW w = new WWW(setInventorySite, www.data);
        StartCoroutine(SellItem(w));
    }

    IEnumerator SellItem(WWW w)
    {
        yield return w;

        if (w.text == "Successfully sold.")
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "Successfully Sold.";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
            UpdateInventory();
        }
        else if (w.text == "Account ID is undefined.")
        {
            Application.Quit();
        }
        else if (w.text == "Couldn't find item." || w.text == "Could not retrieve any item results.")
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "Item doesn't exist.";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
            UpdateInventory();
        }
        else if (w.text == "Sell amount too big.")
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "You don't have enough items.";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
        }
        else
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "An error occurred";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
        }
    }

    private void DisplayInventory()
    {
        Image[] inventorySlots = inventorySetPanel.GetComponentsInChildren<Image>();
        List<JSONObject> itemInventory = inventoryJSON["knight"].list;
        for (int n = 0; n < itemInventory.Count; n++)
        {
            var item = itemInventory[n];
            if (item[1].n >= inventorySlots.Length)
            {
                // Try to display on ability inventory menu
                DisplayAbilityItem(item);
                continue;
            }
            var itemSlot = inventorySlots[(int)item[1].n].gameObject;

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
                        newItem.itemBuyPrice = itemBase.itemBuyPrice;
                        newItem.itemSellPrice = itemBase.itemSellPrice;

                        k = items.Length;
                        break;
                    }
                }
            }
        }
    }

    private void DisplayAbilityItem(JSONObject item)
    {
        Image[] abilitySlots = abilitySlotPanel.GetComponentsInChildren<Image>();

        int slotID = (int)item[1].n - 9;

        if (slotID >= abilitySlots.Length)
        {
            return;
        }
        var itemSlot = abilitySlots[slotID].gameObject;

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
                    newItem.itemBuyPrice = itemBase.itemBuyPrice;
                    newItem.itemSellPrice = itemBase.itemSellPrice;

                    k = items.Length;
                    break;
                }
            }
        }
    }

    private void DisplayAbility()
    {
        Image[] inventorySlots = abilitySlotPanel.GetComponentsInChildren<Image>();
        List<JSONObject> itemInventory = inventoryJSON["ability"].list;
        for (int n = 0; n < itemInventory.Count; n++)
        {
            var item = itemInventory[n];
            if (item[1].n >= inventorySlots.Length)
            {
                continue;
            }
            var itemSlot = inventorySlots[(int)item[1].n].gameObject;

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
                        newItem.itemBuyPrice = itemBase.itemBuyPrice;
                        newItem.itemSellPrice = itemBase.itemSellPrice;

                        k = items.Length;
                        break;
                    }
                }
            }
        }
    }

    private void DisplayDungeon(int index)
    {
        // Clear existing slots
        Image[] inventorySlots = dungeonSlotPanel.GetComponentsInChildren<Image>();
        foreach (Image image in inventorySlots)
        {
            image.sprite = null;
            Destroy(image.GetComponent<ItemBase>());
        }

        List<JSONObject> itemInventory = inventoryJSON["lord"].list;
        if (itemInventory == null) return;

        for (int n = 0; n < itemInventory.Count; n++)
        {
            var item = itemInventory[n].list;
            if (item[1].n >= inventorySlots.Length || item[2].n != index)
            {
                continue;
            }
            var itemSlot = inventorySlots[(int)item[1].n].gameObject;

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
                        newItem.itemBuyPrice = itemBase.itemBuyPrice;
                        newItem.itemSellPrice = itemBase.itemSellPrice;

                        k = items.Length;
                        break;
                    }
                }
            }
        }
    }

    private void DisplayDungeonSelector()
    {
        int dungeonCount = (int)inventoryJSON["count"].n;

        // Clear existing children
        foreach (Transform child in dungeonSelectorPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i <= dungeonCount - 1; i++)
        {
            int index = i;

            GameObject selector = (GameObject)Instantiate(compositionSelectorPrefab, dungeonSelectorPanel.transform);
            EventTrigger eventTrigger = selector.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) =>
            {
                selectedDungeon = index;
                DisplayDungeon(index);
                // Set current one to inactive button and rest to active
                foreach (Transform child in dungeonSelectorPanel.transform)
                {
                    child.GetComponent<Button>().interactable = true;
                }
                selector.GetComponent<Button>().interactable = false;
            });
            eventTrigger.triggers.Add(entry);

            selector.GetComponentInChildren<Text>().text = (i + 1).ToString();

            if (i == 0)
            {
                selector.GetComponent<Button>().interactable = false;
            }
        }

        if (dungeonCount != 4)
        {
            GameObject selector = (GameObject)Instantiate(compositionSelectorPrefab, dungeonSelectorPanel.transform);
            EventTrigger eventTrigger = selector.GetComponent<EventTrigger>();
            eventTrigger.triggers.Clear();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) =>
            {
                // TODO: Display dialogue for purchase
                BuyItem(dungeonCount + 9999, 1);
            });
            eventTrigger.triggers.Add(entry);

            selector.GetComponentInChildren<Text>().text = "+";
        }
    }
}