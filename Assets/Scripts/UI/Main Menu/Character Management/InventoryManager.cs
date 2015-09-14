using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class InventoryManager : MonoBehaviour
{

    [SerializeField]
    private string server = "bassetune.com";
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
    public PrefabStore abilities;

    [SerializeField]
    private ClientData clientData;

    void Start()
    {
        setInventorySite = server + "/setInventory";
        getInventorySite = server + "/getInventory";

        clientData = FindObjectOfType<ClientData>() as ClientData;
        UpdateInventory();
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
                if (panel == "armor" && !items[n].prefabs[0].GetComponent<Armor>())
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

    private AbilityBase GetAbilityInfo(int id)
    {
        for (int i = 0; i < abilities.prefabs.Length; i++)
        {
            AbilityBase ability = abilities.prefabs[i].GetComponent<AbilityBase>();
            if (abilities.prefabs[i].GetComponent<AbilityBase>().abilityID == id)
            {
                return ability;
            }
        }
        return null;
    }

    public void ShowInventory(string panel)
    {
        int inventoryLength = inventoryJSON.Count;
        // Clear previous entries in inventory
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }
        // Start creating entries
        for (int i = 0; i < inventoryLength; i++)
        {
            if (inventoryJSON[i][0].ToString() == "null")
            {
                continue;
            }
            // Get and set variables for the item entry
            if (panel == "ability")
            {
                AbilityBase item = GetAbilityInfo(i);
                if (item != null)
                {
                    // Create entry
                    GameObject newObject = Instantiate(labelPrefab);
                    newObject.transform.SetParent(inventoryPanel.transform);

                    newObject.GetComponentInChildren<Text>().text = item.abilityName;
                    newObject.GetComponentsInChildren<Image>()[1].sprite = item.icon;
                    newObject.AddComponent<AbilityBase>().abilityID = item.abilityID;
                }
            }
            else
            {
                ItemBase item = GetItemInfo(panel, i);
                if (item != null)
                {
                    // Create entry
                    GameObject newObject = Instantiate(labelPrefab);
                    newObject.transform.SetParent(inventoryPanel.transform);

                    newObject.GetComponentInChildren<Text>().text = item.itemName;
                    newObject.GetComponentsInChildren<Image>()[1].sprite = item.itemIcon;
                    newObject.AddComponent<ItemBase>().itemID = item.itemID;
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
        if (panel == "ability")
        {
            for (int i = 0; i < abilities.prefabs.Length; i++)
            {
                // Create entry
                AbilityBase item = abilities.prefabs[i].GetComponent<AbilityBase>();
                GameObject newObject = Instantiate(labelPrefab);
                newObject.transform.SetParent(shopPanel.transform);

                newObject.GetComponentInChildren<Text>().text = item.abilityName;
                newObject.GetComponentsInChildren<Image>()[1].sprite = item.icon;
                newObject.AddComponent<AbilityBase>().abilityID = item.abilityID;
            }
        }
        else
        {
            for (int n = 0; n < items.Length; n++)
            {
                if (items[n].prefabs.Length > 0)
                {
                    if (panel == "weapon" && !items[n].prefabs[0].GetComponent<Weapon>())
                    {
                        continue;
                    }
                    if (panel == "armor" && !items[n].prefabs[0].GetComponent<Armor>())
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
                    for (int i = 0; i < items[n].prefabs.Length; i++)
                    {
                        // Create Entry
                        ItemBase item = items[n].prefabs[i].GetComponent<ItemBase>();
                        GameObject newObject = Instantiate(labelPrefab);
                        newObject.transform.SetParent(shopPanel.transform);

                        newObject.GetComponentInChildren<Text>().text = item.itemName;
                        newObject.GetComponentsInChildren<Image>()[1].sprite = item.itemIcon;
                        newObject.AddComponent<ItemBase>().itemID = item.itemID;
                    }
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

    //void SetShopText(GameObject shopList, GameObject[] items)
    //{
    //    for (int i = 0; i < items.Length; i++)
    //    {
    //        if (items[i].GetComponent<ItemBase>() && items[i].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Knight)
    //        {
    //            GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(shopLabel);
    //            newObject.transform.SetParent(shopList.transform);
    //            newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    //            newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    //            newObject.GetComponent<Text>().text = items[i].GetComponent<ItemBase>().itemName;

    //            Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
    //            for (int il = 0; il < newObjectButtons.Length; il++)
    //            {
    //                int itemIndex = i;
    //                if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Buy")
    //                {
    //                    newObjectButtons[il].onClick.RemoveAllListeners(); // TODO Get Item Purchase Amount
    //                    newObjectButtons[il].onClick.AddListener(() => { BuyItem(itemIndex, 1, "Item"); }); ;
    //                    newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<ItemBase>().itemBuyPrice;
    //                }
    //            }
    //        }
    //        else if (items[i].GetComponent<WeaponBase>())
    //        {
    //            GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(shopLabel);
    //            newObject.transform.SetParent(shopList.transform);
    //            newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    //            newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    //            newObject.GetComponent<Text>().text = items[i].GetComponent<WeaponBase>().weaponName;

    //            Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
    //            for (int il = 0; il < newObjectButtons.Length; il++)
    //            {
    //                int itemIndex = i;
    //                if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Buy")
    //                {
    //                    newObjectButtons[il].onClick.RemoveAllListeners();
    //                    newObjectButtons[il].onClick.AddListener(() => { BuyItem(itemIndex, 1, "Weapon"); }); ;
    //                    newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<WeaponBase>().weaponBuyPrice.ToString();
    //                }
    //            }
    //        }
    //        else if (items[i].GetComponent<AbilityBase>())
    //        {
    //            GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(shopLabel);
    //            newObject.transform.SetParent(shopList.transform);
    //            newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    //            newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    //            newObject.GetComponent<Text>().text = items[i].GetComponent<AbilityBase>().abilityName;

    //            Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
    //            for (int il = 0; il < newObjectButtons.Length; il++)
    //            {
    //                int itemIndex = i;
    //                if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Buy")
    //                {
    //                    newObjectButtons[il].onClick.RemoveAllListeners();
    //                    newObjectButtons[il].onClick.AddListener(() => { BuyItem(itemIndex, 1, "Ability"); }); ;
    //                    newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<AbilityBase>().buyPrice.ToString();
    //                }
    //            }
    //        }
    //    }

    //    RectTransform rectTransform = shopList.GetComponent<RectTransform>();
    //    if (shopList.GetComponentsInChildren<Text>(true).Length > 5)
    //    {
    //        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (shopList.GetComponentsInChildren<Text>(true).Length - 180) * 4);
    //    }
    //}

    //void SetShopText(GameObject shopList, GameObject[] items, ItemBase.BossItemType itemType)
    //{
    //    for (int i = 0; i < items.Length; i++)
    //    {
    //        if (items[i].GetComponent<ItemBase>().bossItemType == itemType && items[i].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Boss)
    //        {
    //            GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(shopLabel);
    //            newObject.transform.SetParent(shopList.transform);
    //            newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    //            newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    //            newObject.GetComponent<Text>().text = items[i].GetComponent<ItemBase>().itemName;

    //            Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
    //            for (int il = 0; il < newObjectButtons.Length; il++)
    //            {
    //                int itemIndex = i;
    //                if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Buy")
    //                {
    //                    newObjectButtons[il].onClick.RemoveAllListeners(); // TODO Get Item Purchase Amount
    //                    newObjectButtons[il].onClick.AddListener(() => { BuyItem(itemIndex, 1, "Item"); }); ;
    //                    newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<ItemBase>().itemBuyPrice;
    //                }
    //            }
    //        }
    //    }

    //    RectTransform rectTransform = shopList.GetComponent<RectTransform>();
    //    if (shopList.GetComponentsInChildren<Text>(true).Length > 5)
    //    {
    //        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (shopList.GetComponentsInChildren<Text>(true).Length - 180) * 4);
    //    }
    //}

    void BuyItem(int itemIndex, int itemAmount, string itemType)
    {
        WWWForm www = new WWWForm();
        www.AddField("uuid", clientData.GetSession());
        www.AddField("itemAmount", itemAmount);
        www.AddField("commandType", "Buy");
        www.AddField("itemType", itemType);
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
        Debug.Log(w.text);
    }

    void SellItem(int itemIndex, int itemAmount, string itemType)
    {
        WWWForm www = new WWWForm();
        www.AddField("uuid", clientData.GetSession());
        www.AddField("itemAmount", itemAmount);
        www.AddField("commandType", "Sell");
        www.AddField("itemType", itemType);
        www.AddField("item", itemIndex);
        Debug.Log(itemIndex);
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

        Debug.Log(w.text);
    }

    //public void CopyItemBase(ItemBase oldBase, ItemBase newBase)
    //{
    //    newBase.itemIcon = oldBase.itemIcon;
    //    newBase.itemName = oldBase.itemName;
    //    newBase.itemSide = oldBase.itemSide;
    //    newBase.itemDescription = oldBase.itemDescription;
    //    newBase.itemSellPrice = oldBase.itemSellPrice;
    //    newBase.itemBuyPrice = oldBase.itemBuyPrice;
    //}

    //public void CopyWeaponBase(WeaponBase oldBase, WeaponBase newBase)
    //{
    //    newBase.weaponIcon = oldBase.weaponIcon;
    //    newBase.weaponName = oldBase.weaponName;
    //    newBase.weaponDescription = oldBase.weaponDescription;
    //    newBase.weaponSellPrice = oldBase.weaponSellPrice;
    //    newBase.weaponBuyPrice = oldBase.weaponBuyPrice;
    //}

    //public void CopyAbilityBase(AbilityBase oldBase, AbilityBase newBase)
    //{
    //    newBase.icon = oldBase.icon;
    //    newBase.abilityName = oldBase.abilityName;
    //    newBase.buyPrice = oldBase.buyPrice;
    //    newBase.sellPrice = oldBase.sellPrice;
    //    newBase.description = oldBase.description;
    //    newBase.abilityType = oldBase.abilityType;
    //}
}