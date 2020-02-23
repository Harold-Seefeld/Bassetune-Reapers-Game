using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private readonly string getInventorySite = "ec2-52-0-51-109.compute-1.amazonaws.com/getinventory";
    [SerializeField] private readonly string setInventorySite = "ec2-52-0-51-109.compute-1.amazonaws.com/setinventory";
    public GameObject abilityInventory;
    public GameObject[] abilityList;
    public GameObject abilityShop;
    public GameObject bossInventory;
    public GameObject bossShop;
    public GameObject creatureInventory;
    public GameObject creatureShop;
    public GameObject equipmentInventory;

    public GameObject equipmentShop;
    public JSONObject inventoryJSON;
    public GameObject inventoryLabel;

    public GameObject[] itemList;
    public GameObject minibossInventory;
    public GameObject minibossShop;
    public Button notificationButton;
    public RectTransform notificationRect;
    public Text notificationText;

    public SessionManager sessionManager;
    public GameObject shopLabel;
    public GameObject trapInventory;
    public GameObject trapShop;
    public GameObject weaponInventory;
    public GameObject[] weaponList;
    public GameObject weaponShop;

    public void UpdateInventory()
    {
        var www = new WWWForm();
        www.AddField("uuid", sessionManager.GetSession());
        var w = new WWW(getInventorySite, www.data);
        StartCoroutine(UpdateInventory(w));
    }

    private IEnumerator UpdateInventory(WWW w)
    {
        yield return w;
        inventoryJSON = new JSONObject(w.text);

        Debug.Log("Downloaded Inventory Successfully.");

        Debug.Log(inventoryJSON.Print());

        // Clear all existing texts
        ClearText(abilityShop.GetComponentsInChildren<Text>());
        ClearText(weaponShop.GetComponentsInChildren<Text>());
        ClearText(creatureShop.GetComponentsInChildren<Text>());
        ClearText(equipmentShop.GetComponentsInChildren<Text>());
        ClearText(minibossShop.GetComponentsInChildren<Text>());
        ClearText(bossShop.GetComponentsInChildren<Text>());
        ClearText(trapShop.GetComponentsInChildren<Text>());
        ClearText(abilityInventory.GetComponentsInChildren<Text>());
        ClearText(weaponInventory.GetComponentsInChildren<Text>());
        ClearText(creatureInventory.GetComponentsInChildren<Text>());
        ClearText(equipmentInventory.GetComponentsInChildren<Text>());
        ClearText(minibossInventory.GetComponentsInChildren<Text>());
        ClearText(bossInventory.GetComponentsInChildren<Text>());
        ClearText(trapInventory.GetComponentsInChildren<Text>());

        // Set Shop Text
        SetShopText(equipmentShop, itemList);
        SetShopText(abilityShop, abilityList);
        SetShopText(weaponShop, weaponList);
        SetShopText(bossShop, itemList, ItemBase.BossItemType.Boss);
        SetShopText(minibossShop, itemList, ItemBase.BossItemType.Miniboss);
        SetShopText(trapShop, itemList, ItemBase.BossItemType.Trap);
        SetShopText(creatureShop, itemList, ItemBase.BossItemType.Creature);

        // Set Inventory Text
        SetInventory(equipmentShop, equipmentInventory, itemList);
        SetInventory(abilityShop, abilityInventory, abilityList);
        SetInventory(weaponShop, weaponInventory, weaponList);
        SetInventory(bossShop, bossInventory, itemList, ItemBase.BossItemType.Boss);
        SetInventory(minibossShop, minibossInventory, itemList, ItemBase.BossItemType.Miniboss);
        SetInventory(trapShop, trapInventory, itemList, ItemBase.BossItemType.Trap);
        SetInventory(creatureShop, creatureInventory, itemList, ItemBase.BossItemType.Creature);

        Debug.Log("Filtered Inventory Successfully.");
    }

    private void ClearText(Text[] textsToClear)
    {
        for (var i = 0; i < textsToClear.Length; i++) Destroy(textsToClear[i].gameObject);
    }

    private void SetShopText(GameObject shopList, GameObject[] items)
    {
        for (var i = 0; i < items.Length; i++)
            if (items[i].GetComponent<ItemBase>() &&
                items[i].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Knight)
            {
                var newObject = Instantiate(shopLabel);
                newObject.transform.SetParent(shopList.transform);
                newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                newObject.GetComponent<Text>().text = items[i].GetComponent<ItemBase>().itemName;

                var newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
                for (var il = 0; il < newObjectButtons.Length; il++)
                {
                    var itemIndex = i;
                    if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Buy")
                    {
                        newObjectButtons[il].onClick.RemoveAllListeners(); // TODO Get Item Purchase Amount
                        newObjectButtons[il].onClick.AddListener(() => { BuyItem(itemIndex, 1, "Item"); });
                        ;
                        newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text =
                            "G| " + items[i].GetComponent<ItemBase>().itemBuyPrice;
                    }
                }
            }
            else if (items[i].GetComponent<WeaponBase>())
            {
                var newObject = Instantiate(shopLabel);
                newObject.transform.SetParent(shopList.transform);
                newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                newObject.GetComponent<Text>().text = items[i].GetComponent<WeaponBase>().weaponName;

                var newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
                for (var il = 0; il < newObjectButtons.Length; il++)
                {
                    var itemIndex = i;
                    if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Buy")
                    {
                        newObjectButtons[il].onClick.RemoveAllListeners();
                        newObjectButtons[il].onClick.AddListener(() => { BuyItem(itemIndex, 1, "Weapon"); });
                        ;
                        newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text =
                            "G| " + items[i].GetComponent<WeaponBase>().weaponBuyPrice;
                    }
                }
            }
            else if (items[i].GetComponent<AbilityBase>())
            {
                var newObject = Instantiate(shopLabel);
                newObject.transform.SetParent(shopList.transform);
                newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                newObject.GetComponent<Text>().text = items[i].GetComponent<AbilityBase>().abilityName;

                var newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
                for (var il = 0; il < newObjectButtons.Length; il++)
                {
                    var itemIndex = i;
                    if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Buy")
                    {
                        newObjectButtons[il].onClick.RemoveAllListeners();
                        newObjectButtons[il].onClick.AddListener(() => { BuyItem(itemIndex, 1, "Ability"); });
                        ;
                        newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text =
                            "G| " + items[i].GetComponent<AbilityBase>().buyPrice;
                    }
                }
            }

        var rectTransform = shopList.GetComponent<RectTransform>();
        if (shopList.GetComponentsInChildren<Text>(true).Length > 5)
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x,
                (shopList.GetComponentsInChildren<Text>(true).Length - 180) * 4);
    }

    private void SetShopText(GameObject shopList, GameObject[] items, ItemBase.BossItemType itemType)
    {
        for (var i = 0; i < items.Length; i++)
            if (items[i].GetComponent<ItemBase>().bossItemType == itemType &&
                items[i].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Boss)
            {
                var newObject = Instantiate(shopLabel);
                newObject.transform.SetParent(shopList.transform);
                newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                newObject.GetComponent<Text>().text = items[i].GetComponent<ItemBase>().itemName;

                var newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
                for (var il = 0; il < newObjectButtons.Length; il++)
                {
                    var itemIndex = i;
                    if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Buy")
                    {
                        newObjectButtons[il].onClick.RemoveAllListeners(); // TODO Get Item Purchase Amount
                        newObjectButtons[il].onClick.AddListener(() => { BuyItem(itemIndex, 1, "Item"); });
                        ;
                        newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text =
                            "G| " + items[i].GetComponent<ItemBase>().itemBuyPrice;
                    }
                }
            }

        var rectTransform = shopList.GetComponent<RectTransform>();
        if (shopList.GetComponentsInChildren<Text>(true).Length > 5)
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x,
                (shopList.GetComponentsInChildren<Text>(true).Length - 180) * 4);
    }

    private void SetInventory(GameObject shopList, GameObject inventoryList, GameObject[] items)
    {
        for (var i = 0; i < inventoryJSON.Count; i++)
            if (inventoryJSON[i][0].ToString() != "null" && items == itemList &&
                items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemSide ==
                ItemBase.ItemSide.Knight)
            {
                var newObject = Instantiate(inventoryLabel);
                newObject.transform.SetParent(inventoryList.transform);
                newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                newObject.GetComponent<Text>().text =
                    items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemName;
                var itemBase = newObject.AddComponent<ItemBase>();
                CopyItemBase(items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>(), itemBase);

                var newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
                for (var il = 0; il < newObjectButtons.Length; il++)
                {
                    var itemIndex = Convert.ToInt16(inventoryJSON[i][0]) - 1;
                    if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Sell")
                    {
                        newObjectButtons[il].onClick.RemoveAllListeners();
                        newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text =
                            "G| " + items[i].GetComponent<ItemBase>().itemSellPrice;
                        newObjectButtons[il].onClick.AddListener(() => { SellItem(itemIndex, 1, "Item"); });
                        ;
                    }
                }
            }
            else if (inventoryJSON[i][2].ToString() != "null" && items == weaponList)
            {
                var newObject = Instantiate(inventoryLabel);
                newObject.transform.SetParent(inventoryList.transform);
                newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][2]) - 1]
                    .GetComponent<WeaponBase>().weaponName;
                var weaponBase = newObject.AddComponent<WeaponBase>();
                CopyWeaponBase(items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<WeaponBase>(), weaponBase);

                var newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
                for (var il = 0; il < newObjectButtons.Length; il++)
                {
                    var itemIndex = Convert.ToInt16(inventoryJSON[i][2]) - 1;
                    if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Sell")
                    {
                        newObjectButtons[il].onClick.RemoveAllListeners();
                        newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text =
                            "G| " + items[i].GetComponent<WeaponBase>().weaponSellPrice;
                        newObjectButtons[il].onClick.AddListener(() => { SellItem(itemIndex, 1, "Weapon"); });
                        ;
                    }
                }
            }
            else if (inventoryJSON[i][3].ToString() != "null" && items == abilityList)
            {
                var newObject = Instantiate(inventoryLabel);
                newObject.transform.SetParent(inventoryList.transform);
                newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][3]) - 1]
                    .GetComponent<AbilityBase>().abilityName;
                var abilityBase = newObject.AddComponent<AbilityBase>();
                abilityBase.onMainMenu = true;
                CopyAbilityBase(items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<AbilityBase>(),
                    abilityBase);

                var newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
                for (var il = 0; il < newObjectButtons.Length; il++)
                {
                    var itemIndex = Convert.ToInt16(inventoryJSON[i][3]) - 1;
                    if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Sell")
                    {
                        newObjectButtons[il].onClick.RemoveAllListeners();
                        newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text =
                            "G| " + items[i].GetComponent<AbilityBase>().sellPrice;
                        newObjectButtons[il].onClick.AddListener(() => { SellItem(itemIndex, 1, "Ability"); });
                        ;
                    }
                }
            }

        var rectTransform = inventoryList.GetComponent<RectTransform>();
        if (inventoryList.GetComponentsInChildren<Text>(true).Length > 5)
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x,
                (inventoryList.GetComponentsInChildren<Text>(true).Length - 180) * 4);
    }

    private void SetInventory(GameObject shopList, GameObject inventoryList, GameObject[] items,
        ItemBase.BossItemType itemType)
    {
        for (var i = 0; i < inventoryJSON.Count; i++)
            if (inventoryJSON[i][0].ToString() != "null" &&
                items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemSide ==
                ItemBase.ItemSide.Boss &&
                items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().bossItemType == itemType)
            {
                var newObject = Instantiate(inventoryLabel);
                newObject.transform.SetParent(inventoryList.transform);
                newObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                newObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                newObject.GetComponent<Text>().text =
                    items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemName;
                var itemBase = newObject.AddComponent<ItemBase>();
                CopyItemBase(items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>(), itemBase);

                var newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
                for (var il = 0; il < newObjectButtons.Length; il++)
                {
                    var itemIndex = Convert.ToInt16(inventoryJSON[i][0]) - 1;
                    if (newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text == "Sell")
                    {
                        newObjectButtons[il].onClick.RemoveAllListeners();
                        newObjectButtons[il].GetComponentsInChildren<Text>(true)[0].text =
                            "G| " + items[i].GetComponent<AbilityBase>().sellPrice;
                        newObjectButtons[il].onClick.AddListener(() => { SellItem(itemIndex, 1, "Item"); });
                        ;
                    }
                }
            }

        var rectTransform = inventoryList.GetComponent<RectTransform>();
        if (inventoryList.GetComponentsInChildren<Text>(true).Length > 5)
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x,
                rectTransform.offsetMax.x - (inventoryList.GetComponentsInChildren<Text>(true).Length - 150 * 30));
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y);
        }
    }

    private void BuyItem(int itemIndex, int itemAmount, string itemType)
    {
        var www = new WWWForm();
        www.AddField("uuid", sessionManager.GetSession());
        www.AddField("itemAmount", itemAmount);
        www.AddField("commandType", "Buy");
        www.AddField("itemType", itemType);
        www.AddField("item", itemIndex + 1);
        var w = new WWW(setInventorySite, www.data);
        StartCoroutine(BuyItem(w));
    }

    private IEnumerator BuyItem(WWW w)
    {
        yield return w;

        if (w.text == "Successfully purchased.")
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "Successfully Purchased.";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); });
            ;
            UpdateInventory();
        }
        else if (w.text == "Account ID is undefined.")
        {
            sessionManager.next.enabled = false;
            sessionManager.current.enabled = true;
            sessionManager.notification.text = "You have been logged out. Please log in again.";
        }
        else if (w.text == "Not enough gold.")
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "Not enough gold.";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); });
            ;
        }
        else
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "An error occurred";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); });
            ;
        }

        Debug.Log(w.text);
    }

    private void SellItem(int itemIndex, int itemAmount, string itemType)
    {
        var www = new WWWForm();
        www.AddField("uuid", sessionManager.GetSession());
        www.AddField("itemAmount", itemAmount);
        www.AddField("commandType", "Sell");
        www.AddField("itemType", itemType);
        www.AddField("item", itemIndex + 1);
        Debug.Log(itemIndex + 1);
        var w = new WWW(setInventorySite, www.data);
        StartCoroutine(SellItem(w));
    }

    private IEnumerator SellItem(WWW w)
    {
        yield return w;

        if (w.text == "Successfully sold.")
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "Successfully Sold.";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); });
            ;
            UpdateInventory();
        }
        else if (w.text == "Account ID is undefined.")
        {
            sessionManager.next.enabled = false;
            sessionManager.current.enabled = true;
            sessionManager.notification.text = "You have been logged out. Please log in again.";
        }
        else if (w.text == "Couldn't find item." || w.text == "Could not retrieve any item results.")
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "Item doesn't exist.";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); });
            ;
            UpdateInventory();
        }
        else if (w.text == "Sell amount too big.")
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "You don't have enough items.";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); });
            ;
        }
        else
        {
            notificationRect.transform.gameObject.SetActive(true);
            notificationRect.SetAsLastSibling();
            notificationText.text = "An error occurred";
            notificationButton.onClick.RemoveAllListeners();
            notificationButton.onClick.AddListener(() => { notificationRect.transform.gameObject.SetActive(false); });
            ;
        }

        Debug.Log(w.text);
    }

    public void CopyItemBase(ItemBase oldBase, ItemBase newBase)
    {
        newBase.itemIcon = oldBase.itemIcon;
        newBase.itemName = oldBase.itemName;
        newBase.itemSide = oldBase.itemSide;
        newBase.itemDescription = oldBase.itemDescription;
        newBase.itemSellPrice = oldBase.itemSellPrice;
        newBase.itemBuyPrice = oldBase.itemBuyPrice;
        newBase.itemAnimation = oldBase.itemAnimation;
    }

    public void CopyWeaponBase(WeaponBase oldBase, WeaponBase newBase)
    {
        newBase.weaponIcon = oldBase.weaponIcon;
        newBase.weaponName = oldBase.weaponName;
        newBase.weaponDescription = oldBase.weaponDescription;
        newBase.weaponSellPrice = oldBase.weaponSellPrice;
        newBase.weaponBuyPrice = oldBase.weaponBuyPrice;
    }

    public void CopyAbilityBase(AbilityBase oldBase, AbilityBase newBase)
    {
        newBase.icon = oldBase.icon;
        newBase.abilityName = oldBase.abilityName;
        newBase.buyPrice = oldBase.buyPrice;
        newBase.sellPrice = oldBase.sellPrice;
        newBase.description = oldBase.description;
        newBase.abilityType = oldBase.abilityType;
    }
}