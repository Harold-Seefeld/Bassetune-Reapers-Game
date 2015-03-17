using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class InventoryManager : MonoBehaviour {

	[SerializeField] string getInventorySite = "ec2-52-0-51-109.compute-1.amazonaws.com/getinventory";
	[SerializeField] string setInventorySite = "ec2-52-0-51-109.compute-1.amazonaws.com/setinventory";
	public JSONObject inventoryJSON;

	public GameObject[] itemList;
	public GameObject[] weaponList;
	public GameObject[] abilityList; 

	public GameObject equipmentShop;
	public GameObject equipmentInventory;
	public GameObject abilityShop;
	public GameObject abilityInventory;
	public GameObject weaponShop;
	public GameObject weaponInventory;
	public GameObject bossShop;
	public GameObject bossInventory;
	public GameObject minibossShop;
	public GameObject minibossInventory;
	public GameObject trapShop;
	public GameObject trapInventory;
	public GameObject creatureShop;
	public GameObject creatureInventory;
	public Text notificationText;
	public Button notificationButton;
	public RectTransform notificationRect;
	public GameObject inventoryLabel;
	public GameObject shopLabel;

	public SessionManager sessionManager;

	public void UpdateInventory() 
	{
		WWWForm www = new WWWForm();
		www.AddField("uuid", sessionManager.GetSession());
		WWW w = new WWW (getInventorySite, www.data);
		StartCoroutine(UpdateInventory(w));
	}
	
	IEnumerator UpdateInventory(WWW w) 
	{
		yield return w;
		inventoryJSON = new JSONObject(w.text);

		Debug.Log("Downloaded Inventory Successfully.");
		
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

		Debug.Log ("Filtered Inventory Successfully.");
	}

	void SetShopText(GameObject shopList, GameObject[] items)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].GetComponent<ItemBase>() && items[i].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Knight)
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(shopLabel);
				newObject.transform.SetParent(shopList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[i].GetComponent<ItemBase>().itemName;

				Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
				foreach(Button newObjectButton in newObjectButtons)
				{
					int itemIndex = i;
					if (newObjectButton.GetComponentsInChildren<Text>(true)[0].text == "Buy")
					{
						newObjectButton.onClick.RemoveAllListeners(); // TODO Get Item Purchase Amount
						newObjectButton.onClick.AddListener(() => {BuyItem(itemIndex, itemList, 1, "Item");});;
						newObjectButton.GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<ItemBase>().itemBuyPrice;
					}
				}
			}
			else if (items[i].GetComponent<WeaponBase>())
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(shopLabel);
				newObject.transform.SetParent(shopList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[i].GetComponent<WeaponBase>().weaponName;

				Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
				foreach(Button newObjectButton in newObjectButtons)
				{
					int itemIndex = i;
					if (newObjectButton.GetComponentsInChildren<Text>(true)[0].text == "Buy")
					{
						newObjectButton.onClick.RemoveAllListeners();
						newObjectButton.onClick.AddListener(() => {BuyItem(itemIndex, itemList, 1, "Weapon");});;
						newObjectButton.GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<WeaponBase>().weaponBuyPrice.ToString();
					}
				}
			}
			else if (items[i].GetComponent<AbilityBase>())
			{
				Debug.Log("Found one ability");
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(shopLabel);
				newObject.transform.SetParent(shopList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[i].GetComponent<AbilityBase>().abilityName;

				Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
				foreach(Button newObjectButton in newObjectButtons)
				{
					int itemIndex = i;
					if (newObjectButton.GetComponentsInChildren<Text>(true)[0].text == "Buy")
					{
						newObjectButton.onClick.RemoveAllListeners();
						newObjectButton.onClick.AddListener(() => {BuyItem(itemIndex, itemList, 1, "Ability");});;
						newObjectButton.GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<AbilityBase>().buyPrice.ToString();
					}
				}
			}
		}
		
		RectTransform rectTransform = shopList.GetComponent<RectTransform>();
		if (shopList.GetComponentsInChildren<Text>(true).Length > 5)
		{
			rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (shopList.GetComponentsInChildren<Text>(true).Length - 180) * 4);
		}
	}

	void SetShopText(GameObject shopList, GameObject[] items, ItemBase.BossItemType itemType)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].GetComponent<ItemBase>().bossItemType == itemType && items[i].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Boss)
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(shopLabel);
				newObject.transform.SetParent(shopList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[i].GetComponent<ItemBase>().itemName;

				Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
				foreach(Button newObjectButton in newObjectButtons)
				{
					int itemIndex = i;
					if (newObjectButton.GetComponentsInChildren<Text>(true)[0].text == "Buy")
					{
						newObjectButton.onClick.RemoveAllListeners(); // TODO Get Item Purchase Amount
						newObjectButton.onClick.AddListener(() => {BuyItem(itemIndex, itemList, 1, "Item");});;
						newObjectButton.GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<ItemBase>().itemBuyPrice;
					}
				}
			}
		}
		
		RectTransform rectTransform = shopList.GetComponent<RectTransform>();
		if (shopList.GetComponentsInChildren<Text>(true).Length > 5)
		{
			rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (shopList.GetComponentsInChildren<Text>(true).Length - 180) * 4);
		}
	}

	void SetInventory(GameObject shopList, GameObject inventoryList, GameObject[] items)
	{
		for (int i = 0; i < inventoryJSON.Count; i++)
		{
			if (inventoryJSON[i][0].ToString() != "null" && items == itemList && items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Knight)
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(inventoryLabel);
				newObject.transform.SetParent(inventoryList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemName;
				ItemBase itemBase = newObject.AddComponent<ItemBase>();
				CopyItemBase(items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>(), itemBase);
				
			    Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
				foreach(Button newObjectButton in newObjectButtons)
				{
					int itemIndex = i;
					if (newObjectButton.GetComponentsInChildren<Text>(true)[0].text == "Sell")
					{
						newObjectButton.onClick.RemoveAllListeners();
						newObjectButton.GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<ItemBase>().itemSellPrice.ToString();
						newObjectButton.onClick.AddListener(() => {SellItem(itemIndex, itemList, 1, "Item");});;
					}
				}
			}
			else if (inventoryJSON[i][2].ToString() != "null" && items == weaponList)
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(inventoryLabel);
				newObject.transform.SetParent(inventoryList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][2]) - 1].GetComponent<WeaponBase>().weaponName;
				WeaponBase weaponBase = newObject.AddComponent<WeaponBase>();
				CopyWeaponBase(items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<WeaponBase>(), weaponBase);
				
				Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
				foreach(Button newObjectButton in newObjectButtons)
				{
					int itemIndex = i;
					if (newObjectButton.GetComponentsInChildren<Text>(true)[0].text == "Sell")
					{
						newObjectButton.onClick.RemoveAllListeners();
						newObjectButton.GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<WeaponBase>().weaponSellPrice.ToString();
						newObjectButton.onClick.AddListener(() => {SellItem(itemIndex, itemList, 1, "Weapon");});;
					}
				}
			}
			else if (inventoryJSON[i][3].ToString() != "null" && items == abilityList)
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(inventoryLabel);
				newObject.transform.SetParent(inventoryList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][3]) - 1].GetComponent<AbilityBase>().abilityName;
				AbilityBase abilityBase = newObject.AddComponent<AbilityBase>();
				CopyAbilityBase(items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<AbilityBase>(), abilityBase);
				
				Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
				foreach(Button newObjectButton in newObjectButtons)
				{
					int itemIndex = i;
					if (newObjectButton.GetComponentsInChildren<Text>(true)[0].text == "Sell")
					{
						newObjectButton.onClick.RemoveAllListeners();
						newObjectButton.GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<AbilityBase>().sellPrice.ToString();
						newObjectButton.onClick.AddListener(() => {SellItem(itemIndex, itemList, 1, "Ability");});;
					}
				}
			}
		}

		RectTransform rectTransform = inventoryList.GetComponent<RectTransform>();
		if (inventoryList.GetComponentsInChildren<Text>(true).Length > 5)
		{
			rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (inventoryList.GetComponentsInChildren<Text>(true).Length - 180) * 4);
		}
	}

	void SetInventory(GameObject shopList, GameObject inventoryList, GameObject[] items, ItemBase.BossItemType itemType)
	{
		for (int i = 0; i < inventoryJSON.Count; i++)
		{
			if (inventoryJSON[i][0].ToString() != "null" && items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Boss && items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().bossItemType == itemType)
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(inventoryLabel);
				newObject.transform.SetParent(inventoryList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemName;
				ItemBase itemBase = newObject.AddComponent<ItemBase>();
				CopyItemBase(items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>(), itemBase);

				Button[] newObjectButtons = newObject.GetComponentsInChildren<Button>(true);
				foreach(Button newObjectButton in newObjectButtons)
				{
					int itemIndex = i;
					if (newObjectButton.GetComponentsInChildren<Text>(true)[0].text == "Sell")
					{
						newObjectButton.onClick.RemoveAllListeners();
						newObjectButton.GetComponentsInChildren<Text>(true)[0].text = "G| " + items[i].GetComponent<AbilityBase>().sellPrice.ToString();
						newObjectButton.onClick.AddListener(() => {SellItem(itemIndex, itemList, 1, "Item");});;
					}
				}
			}
		}
		
		RectTransform rectTransform = inventoryList.GetComponent<RectTransform>();
		if (inventoryList.GetComponentsInChildren<Text>(true).Length > 5)
		{
			rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMax.x - (inventoryList.GetComponentsInChildren<Text>(true).Length - 150 * 30));
			rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y);
		}
	}

	void BuyItem(int itemIndex, GameObject[] itemList, int itemAmount, string itemType)
	{
		WWWForm www = new WWWForm();
		www.AddField("uuid", sessionManager.GetSession());
		www.AddField("itemAmount", itemAmount);
		www.AddField("commandType", "Buy");
		www.AddField("itemType", itemType);
		www.AddField("item", itemIndex);
		WWW w = new WWW (setInventorySite, www.data);
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
			notificationButton.onClick.AddListener(() => {notificationRect.transform.gameObject.SetActive(false);});;
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
			notificationButton.onClick.AddListener(() => {notificationRect.transform.gameObject.SetActive(false);});;
		}
		else
		{
			notificationRect.transform.gameObject.SetActive(true);
			notificationRect.SetAsLastSibling();
			notificationText.text = "An error occurred";
			notificationButton.onClick.RemoveAllListeners();
			notificationButton.onClick.AddListener(() => {notificationRect.transform.gameObject.SetActive(false);});;
			Debug.LogWarning(w.text);
		}

	}

	void SellItem(int itemIndex, GameObject[] itemList, int itemAmount, string itemType)
	{
		WWWForm www = new WWWForm();
		www.AddField("uuid", sessionManager.GetSession());
		www.AddField("itemAmount", itemAmount);
		www.AddField("commandType", "Sell");
		www.AddField("itemType", itemType);
		www.AddField("item", itemIndex);
		WWW w = new WWW (setInventorySite, www.data);
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
			notificationButton.onClick.AddListener(() => {notificationRect.transform.gameObject.SetActive(false);});;
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
			notificationButton.onClick.AddListener(() => {notificationRect.transform.gameObject.SetActive(false);});;
		}
		else if (w.text == "Sell amount too big.")
		{
			notificationRect.transform.gameObject.SetActive(true);
			notificationRect.SetAsLastSibling();
			notificationText.text = "You don't have enough items.";
			notificationButton.onClick.RemoveAllListeners();
			notificationButton.onClick.AddListener(() => {notificationRect.transform.gameObject.SetActive(false);});;
		}
		else
		{
			notificationRect.transform.gameObject.SetActive(true);
			notificationRect.SetAsLastSibling();
			notificationText.text = "An error occurred";
			notificationButton.onClick.RemoveAllListeners();
			notificationButton.onClick.AddListener(() => {notificationRect.transform.gameObject.SetActive(false);});;
		}
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