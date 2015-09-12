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

    [SerializeField]
    public ClientData clientData;

    void Start()
    {
        setInventorySite = server + "/setInventory";
        getInventorySite = server + "/getInventory";
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

        Debug.Log("Downloaded Inventory Successfully.");
    }

    public void ShowInventory(string panel)
    {

    }

    public void ShowShop(string panel)
    {

    }

    public void SortInventory(string filter)
    {

    }

    public void SortShop(string filter)
    {

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

    //void SetInventory(GameObject shopList, GameObject list, GameObject[] items)
    //{
    //    for (int i = 0; i < inventoryJSON.Count; i++)
    //    {
    //        if (inventoryJSON[i][0].ToString() != "null" && items == inventoryList.itemList && items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Knight)
    //        {
    //            GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(inventoryLabel);
    //            newObject.transform.SetParent(inventoryList.transform);
    //            newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    //            newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    //            newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemName;
    //            ItemBase itemBase = newObject.AddComponent<ItemBase>();
    //            CopyItemBase(items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>(), itemBase);

    //            Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
    //            for (int il = 0; il < newObjectButtons.Length; il++)
    //            {
    //                int itemIndex = Convert.ToInt16(inventoryJSON[i][0]) - 1;
    //                if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Sell")
    //                {
    //                    newObjectButtons[il].onClick.RemoveAllListeners();
    //                    newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<ItemBase>().itemSellPrice.ToString();
    //                    newObjectButtons[il].onClick.AddListener(() => { SellItem(itemIndex, 1, "Item"); }); ;
    //                }
    //            }
    //        }
    //        else if (inventoryJSON[i][2].ToString() != "null" && items == inventoryList.weaponList)
    //        {
    //            GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(inventoryLabel);
    //            newObject.transform.SetParent(inventoryList.transform);
    //            newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    //            newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    //            newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][2]) - 1].GetComponent<WeaponBase>().weaponName;
    //            WeaponBase weaponBase = newObject.AddComponent<WeaponBase>();
    //            CopyWeaponBase(items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<WeaponBase>(), weaponBase);

    //            Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
    //            for (int il = 0; il < newObjectButtons.Length; il++)
    //            {
    //                int itemIndex = Convert.ToInt16(inventoryJSON[i][2]) - 1;
    //                if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Sell")
    //                {
    //                    newObjectButtons[il].onClick.RemoveAllListeners();
    //                    newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<WeaponBase>().weaponSellPrice.ToString();
    //                    newObjectButtons[il].onClick.AddListener(() => { SellItem(itemIndex, 1, "Weapon"); }); ;
    //                }
    //            }
    //        }
    //        else if (inventoryJSON[i][3].ToString() != "null" && items == inventoryList.abilityList)
    //        {
    //            GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(inventoryLabel);
    //            newObject.transform.SetParent(inventoryList.transform);
    //            newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    //            newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    //            newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][3]) - 1].GetComponent<AbilityBase>().abilityName;
    //            AbilityBase abilityBase = newObject.AddComponent<AbilityBase>();
    //            CopyAbilityBase(items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<AbilityBase>(), abilityBase);

    //            Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
    //            for (int il = 0; il < newObjectButtons.Length; il++)
    //            {
    //                int itemIndex = Convert.ToInt16(inventoryJSON[i][3]) - 1;
    //                if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Sell")
    //                {
    //                    newObjectButtons[il].onClick.RemoveAllListeners();
    //                    newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<AbilityBase>().sellPrice.ToString();
    //                    newObjectButtons[il].onClick.AddListener(() => { SellItem(itemIndex, 1, "Ability"); }); ;
    //                }
    //            }
    //        }
    //    }

    //    RectTransform rectTransform = inventoryList.GetComponent<RectTransform>();
    //    if (inventoryList.GetComponentsInChildren<Text>(true).Length > 5)
    //    {
    //        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (inventoryList.GetComponentsInChildren<Text>(true).Length - 180) * 4);
    //    }
    //}

    //void SetInventory(GameObject shopList, GameObject inventoryList, GameObject[] items, ItemBase.BossItemType itemType)
    //{
    //    for (int i = 0; i < inventoryJSON.Count; i++)
    //    {
    //        if (inventoryJSON[i][0].ToString() != "null" && items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Boss && items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().bossItemType == itemType)
    //        {
    //            GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(inventoryLabel);
    //            newObject.transform.SetParent(inventoryList.transform);
    //            newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    //            newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    //            newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemName;
    //            ItemBase itemBase = newObject.AddComponent<ItemBase>();
    //            CopyItemBase(items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>(), itemBase);

    //            Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
    //            for (int il = 0; il < newObjectButtons.Length; il++)
    //            {
    //                int itemIndex = Convert.ToInt16(inventoryJSON[i][0]) - 1;
    //                if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Sell")
    //                {
    //                    newObjectButtons[il].onClick.RemoveAllListeners();
    //                    newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<AbilityBase>().sellPrice.ToString();
    //                    newObjectButtons[il].onClick.AddListener(() => { SellItem(itemIndex, 1, "Item"); }); ;
    //                }
    //            }
    //        }
    //    }

    //    RectTransform rectTransform = inventoryList.GetComponent<RectTransform>();
    //    if (inventoryList.GetComponentsInChildren<Text>(true).Length > 5)
    //    {
    //        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMax.x - (inventoryList.GetComponentsInChildren<Text>(true).Length - 150 * 30));
    //        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y);
    //    }
    //}

    //void BuyItem(int itemIndex, int itemAmount, string itemType)
    //{
    //    WWWForm www = new WWWForm();
    //    www.AddField("uuid", clientData.GetSession());
    //    www.AddField("itemAmount", itemAmount);
    //    www.AddField("commandType", "Buy");
    //    www.AddField("itemType", itemType);
    //    www.AddField("item", itemIndex + 1);
    //    WWW w = new WWW(setInventorySite, www.data);
    //    StartCoroutine(BuyItem(w));
    //}

    //IEnumerator BuyItem(WWW w)
    //{
    //    yield return w;

    //    if (w.text == "Successfully purchased.")
    //    {
    //        notificationRect.transform.gameObject.SetActive(true);
    //        notificationRect.SetAsLastSibling();
    //        notificationText.text = "Successfully Purchased.";
    //        notificationButton.onClick.RemoveAllListeners();
    //        notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
    //        UpdateInventory();
    //    }
    //    else if (w.text == "Account ID is undefined.")
    //    {
    //        Application.Quit();
    //    }
    //    else if (w.text == "Not enough gold.")
    //    {
    //        notificationRect.transform.gameObject.SetActive(true);
    //        notificationRect.SetAsLastSibling();
    //        notificationText.text = "Not enough gold.";
    //        notificationButton.onClick.RemoveAllListeners();
    //        notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
    //    }
    //    else
    //    {
    //        notificationRect.transform.gameObject.SetActive(true);
    //        notificationRect.SetAsLastSibling();
    //        notificationText.text = "An error occurred";
    //        notificationButton.onClick.RemoveAllListeners();
    //        notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
    //    }
    //    Debug.Log(w.text);
    //}

    //void SellItem(int itemIndex, int itemAmount, string itemType)
    //{
    //    WWWForm www = new WWWForm();
    //    www.AddField("uuid", clientData.GetSession());
    //    www.AddField("itemAmount", itemAmount);
    //    www.AddField("commandType", "Sell");
    //    www.AddField("itemType", itemType);
    //    www.AddField("item", itemIndex + 1);
    //    Debug.Log(itemIndex + 1);
    //    WWW w = new WWW(setInventorySite, www.data);
    //    StartCoroutine(SellItem(w));
    //}

    //IEnumerator SellItem(WWW w)
    //{
    //    yield return w;

    //    if (w.text == "Successfully sold.")
    //    {
    //        notificationRect.transform.gameObject.SetActive(true);
    //        notificationRect.SetAsLastSibling();
    //        notificationText.text = "Successfully Sold.";
    //        notificationButton.onClick.RemoveAllListeners();
    //        notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
    //        UpdateInventory();
    //    }
    //    else if (w.text == "Account ID is undefined.")
    //    {
    //        Application.Quit();
    //    }
    //    else if (w.text == "Couldn't find item." || w.text == "Could not retrieve any item results.")
    //    {
    //        notificationRect.transform.gameObject.SetActive(true);
    //        notificationRect.SetAsLastSibling();
    //        notificationText.text = "Item doesn't exist.";
    //        notificationButton.onClick.RemoveAllListeners();
    //        notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
    //        UpdateInventory();
    //    }
    //    else if (w.text == "Sell amount too big.")
    //    {
    //        notificationRect.transform.gameObject.SetActive(true);
    //        notificationRect.SetAsLastSibling();
    //        notificationText.text = "You don't have enough items.";
    //        notificationButton.onClick.RemoveAllListeners();
    //        notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
    //    }
    //    else
    //    {
    //        notificationRect.transform.gameObject.SetActive(true);
    //        notificationRect.SetAsLastSibling();
    //        notificationText.text = "An error occurred";
    //        notificationButton.onClick.RemoveAllListeners();
    //        notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); }); ;
    //    }

    //    Debug.Log(w.text);
    //}

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