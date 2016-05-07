using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class InventoryManager : MonoBehaviour
{

    public string server = "bassetune.com";
    private string getInventorySite = "";
    private string setInventorySite = "";

    // Stores current JSON from the server
    public JSONObject inventoryJSON;

    // UI References
    public GameObject shopPanel;
    public GameObject inventoryPanel;
    public Text notificationText;
    public Button notificationButton;
    public RectTransform notificationRect;

    // Label to create an entry
    public GameObject labelPrefab;

    // All items and abilities
    public PrefabStore[] items;

    [SerializeField]
    private ClientData clientData;

    void Start()
    {
        setInventorySite = server + "/setInventory";
        getInventorySite = server + "/getInventory";

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
        Debug.Log(w.text);
        inventoryJSON = new JSONObject(w.text);

        ShowInventory("ability");

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
        int inventoryLength = inventoryJSON["all"].Count;
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
                    if (inventoryJSON["all"].list[i][0].n != item.itemID)
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

        if (w.text == "Successfully purchased.")
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
}