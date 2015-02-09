using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class InventoryManager : MonoBehaviour {

	private string getInventorySite = "ec2-52-0-51-109.compute-1.amazonaws.com/getinventory";
	public JSONObject inventoryJSON;

	public GameObject[] itemList;
	public GameObject[] weaponList;
	public GameObject[] abilityList; 

	public GameObject equipmentShop;
	public GameObject equipmentInventory;
	public GameObject abilityShop;
	public GameObject abilityInventory;
	public GameObject bossShop;
	public GameObject bossInventory;
	public GameObject minibossShop;
	public GameObject minibossInventory;
	public GameObject trapShop;
	public GameObject trapInventory;
	public GameObject creatureShop;
	public GameObject creatureInventory;

	[SerializeField] SessionManager sessionManager;

	private GameObject textObject;

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

		// Clear all text on lists
		ClearText(equipmentShop.GetComponentsInChildren<Text>());
		ClearText(equipmentInventory.GetComponentsInChildren<Text>());
		ClearText(bossShop.GetComponentsInChildren<Text>());
		ClearText(bossInventory.GetComponentsInChildren<Text>());
		ClearText(minibossShop.GetComponentsInChildren<Text>());
		ClearText(minibossInventory.GetComponentsInChildren<Text>());
		ClearText(trapShop.GetComponentsInChildren<Text>());
		ClearText(trapInventory.GetComponentsInChildren<Text>());
		ClearText(creatureShop.GetComponentsInChildren<Text>());
		ClearText(creatureInventory.GetComponentsInChildren<Text>());
		ClearText(abilityShop.GetComponentsInChildren<Text>());
		ClearText(abilityInventory.GetComponentsInChildren<Text>());
		
		// Set Shop Text
		SetShopText(equipmentShop, itemList);
		SetShopText(abilityShop, abilityList);
		SetShopText(bossShop, itemList, ItemBase.BossItemType.Boss);
		SetShopText(minibossShop, itemList, ItemBase.BossItemType.Miniboss);
		SetShopText(trapShop, itemList, ItemBase.BossItemType.Trap);
		SetShopText(creatureShop, itemList, ItemBase.BossItemType.Creature);
		
		// Set Inventory Text
		SetInventory(equipmentShop, equipmentInventory, itemList);
		SetInventory(abilityShop, abilityInventory, abilityList);
		SetInventory(bossShop, bossInventory, itemList, ItemBase.BossItemType.Boss);
		SetInventory(minibossShop, minibossInventory, itemList, ItemBase.BossItemType.Miniboss);
		SetInventory(trapShop, trapInventory, itemList, ItemBase.BossItemType.Trap);
		SetInventory(creatureShop, creatureInventory, itemList, ItemBase.BossItemType.Creature);

		Debug.Log ("Filtered Inventory Successfully.");
	}

	void ClearText(Text[] textToClear)
	{
		for (int i = 0; i < textToClear.Length; i++)
		{
			if (textToClear[i].transform.parent.GetComponent<VerticalLayoutGroup>())
			{
				textToClear[i].text = "<color=#ffffffff>N/A</color>";
			}
		}
	}

	void SetShopText(GameObject shopList, GameObject[] items)
	{
		Text[] textList = shopList.GetComponentsInChildren<Text>(true);

		for (int i = 0; i < textList.Length; i++)
		{
			if (textList[i].transform.parent == shopList.transform)
			{
				textObject = textList[i].gameObject;
				break;
			}
		}

		textObject.transform.parent.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
		textObject.transform.parent.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
		textObject.transform.parent.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = true;
		
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].GetComponent<ItemBase>() && items[i].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Knight)
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(textObject);
				newObject.transform.SetParent(shopList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[i].GetComponent<ItemBase>().itemName;
			}
			else if (items[i].GetComponent<WeaponBase>())
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(textObject);
				newObject.transform.SetParent(shopList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[i].GetComponent<WeaponBase>().weaponName;
			}
			else if (items[i].GetComponent<AbilityBase>())
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(textObject);
				newObject.transform.SetParent(shopList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[i].GetComponent<AbilityBase>().abilityName;
			}
		}

		Destroy (textObject);
	}

	void SetShopText(GameObject shopList, GameObject[] items, ItemBase.BossItemType itemType)
	{
		Text[] textList = shopList.GetComponentsInChildren<Text>(true);

		for (int i = 0; i < textList.Length; i++)
		{
			if (textList[i].transform.parent == shopList.transform)
			{
				textObject = textList[i].gameObject;
				break;
			}
		}

		textObject.transform.parent.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
		textObject.transform.parent.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
		textObject.transform.parent.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = true;

		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].GetComponent<ItemBase>().bossItemType == itemType && items[i].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Boss)
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(textObject);
				newObject.transform.SetParent(shopList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[i].GetComponent<ItemBase>().itemName;
			}
		}

		Destroy (textObject);
	}

	void SetInventory(GameObject shopList, GameObject inventoryList, GameObject[] items)
	{
		Text[] shopTextList = shopList.GetComponentsInChildren<Text>(true);
		Text[] inventoryTextList = inventoryList.GetComponentsInChildren<Text>(true);
		
		for (int i = 0; i < inventoryTextList.Length; i++)
		{
			if (inventoryTextList[i].transform.parent == inventoryList.transform)
			{
				textObject = inventoryTextList[i].gameObject;
				break;
			}
		}
		
		textObject.transform.parent.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
		textObject.transform.parent.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
		textObject.transform.parent.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = true;
		
		for (int i = 0; i < inventoryJSON.Count; i++)
		{
			if (inventoryJSON[i][0].ToString() != "null" && items == itemList && items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Knight)
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(textObject);
				newObject.transform.SetParent(inventoryList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemName;

				for (int il = 0; il < shopTextList.Length; il++)
				{
					if (shopTextList[il].text == items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemName)
					{
						Destroy(shopTextList[il].gameObject);
						break;
					}
				}
			}
			else if (inventoryJSON[i][2].ToString() != "null" && items == weaponList)
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(textObject);
				newObject.transform.SetParent(inventoryList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][2]) - 1].GetComponent<WeaponBase>().weaponName;

				for (int il = 0; il < shopTextList.Length; il++)
				{
					if (shopTextList[il].text == items[Convert.ToInt16(inventoryJSON[i][2]) - 1].GetComponent<WeaponBase>().weaponName)
					{
						Destroy(shopTextList[il].gameObject);
						break;
					}
				}
			}
			else if (inventoryJSON[i][3].ToString() != "null" && items == abilityList)
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(textObject);
				newObject.transform.SetParent(inventoryList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][3]) - 1].GetComponent<AbilityBase>().abilityName;

				for (int il = 0; il < shopTextList.Length; il++)
				{
					if (shopTextList[il].text == items[Convert.ToInt16(inventoryJSON[i][3]) - 1].GetComponent<AbilityBase>().abilityName)
					{
						Destroy(shopTextList[il].gameObject);
						break;
					}
				}
			}
		}

		RectTransform rectTransform = inventoryList.GetComponent<RectTransform>();
		if (inventoryList.GetComponentsInChildren<Text>(true).Length > 5)
		{
			rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (inventoryList.GetComponentsInChildren<Text>(true).Length - 180) * 4);
		}
	
		rectTransform = shopList.GetComponent<RectTransform>();
		if (shopList.GetComponentsInChildren<Text>(true).Length > 5)
		{
			rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (shopList.GetComponentsInChildren<Text>(true).Length - 180) * 4);
		}

		Destroy (textObject);
	}

	void SetInventory(GameObject shopList, GameObject inventoryList, GameObject[] items, ItemBase.BossItemType itemType)
	{
		Text[] shopTextList = shopList.GetComponentsInChildren<Text>(true);
		Text[] inventoryTextList = inventoryList.GetComponentsInChildren<Text>(true);
		
		for (int i = 0; i < inventoryTextList.Length; i++)
		{
			if (inventoryTextList[i].transform.parent == inventoryList.transform)
			{
				textObject = inventoryTextList[i].gameObject;
				break;
			}
		}
		
		textObject.transform.parent.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
		textObject.transform.parent.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
		textObject.transform.parent.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = true;
		
		for (int i = 0; i < inventoryJSON.Count; i++)
		{
			if (inventoryJSON[i][0].ToString() != "null" && items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Boss && items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().bossItemType == itemType)
			{
				GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(textObject);
				newObject.transform.SetParent(inventoryList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemName;

				
				for (int il = 0; il < shopTextList.Length; il++)
				{
					if (shopTextList[il].text == items[Convert.ToInt16(inventoryJSON[i][0]) - 1].GetComponent<ItemBase>().itemName)
					{
						Destroy(shopTextList[il].gameObject);
						break;
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

		Destroy (textObject);
	}
}
