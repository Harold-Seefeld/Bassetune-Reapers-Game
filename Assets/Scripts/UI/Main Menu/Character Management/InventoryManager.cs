using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryManager : MonoBehaviour {

	private string inventorySite = "ec2-54-152-118-98.compute-1.amazonaws.com/inventory";
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
		WWW w = new WWW (inventorySite, www.data);
		StartCoroutine(UpdateInventory(w));
	}
	
	IEnumerator UpdateInventory(WWW w) 
	{
		yield return w;
		inventoryJSON = new JSONObject(w.text);

		// Clear all text on lists
		ClearText(equipmentShop.GetComponentsInChildren<Text>(true));
		ClearText(equipmentInventory.GetComponentsInChildren<Text>(true));
		ClearText(bossShop.GetComponentsInChildren<Text>(true));
		ClearText(bossInventory.GetComponentsInChildren<Text>(true));
		ClearText(minibossShop.GetComponentsInChildren<Text>(true));
		ClearText(minibossInventory.GetComponentsInChildren<Text>(true));
		ClearText(trapShop.GetComponentsInChildren<Text>(true));
		ClearText(trapInventory.GetComponentsInChildren<Text>(true));
		ClearText(creatureShop.GetComponentsInChildren<Text>(true));
		ClearText(creatureInventory.GetComponentsInChildren<Text>(true));
		ClearText(abilityShop.GetComponentsInChildren<Text>(true));
		ClearText(abilityInventory.GetComponentsInChildren<Text>(true));

		// Set Shop Text
		SetShopText(equipmentShop, itemList);
		SetShopText(abilityShop, abilityList);
		SetShopText(bossShop, itemList, ItemBase.BossItemType.Boss);
		SetShopText(minibossShop, itemList, ItemBase.BossItemType.Miniboss);
		SetShopText(trapShop, itemList, ItemBase.BossItemType.Trap);
		SetShopText(creatureShop, itemList, ItemBase.BossItemType.Creature);

		//		for (int i = 0; i < inventoryJSON.Count - 1; i++)
//		{
//			if (inventoryJSON[i][0].ToString() != "null")
//			{
//				Text[] textList = GetComponentsInChildren<Text>() as Text[];
//				
//				// Set all text invisible
//				for (int l = 0; i < textList.Length; i++)
//				{
//					if (textList[l].transform.parent == transform)
//						textList[l].text = "<color=#ffffffff>N/A</color>";
//				}
//				
//				// Set the item text
//				int il = 0;
//				for (int l = 0; i < itemList.Length - 1; i++)
//				{
//					if (textList[il].transform.parent == transform)
//					{
//						textList[il].text = itemList[il].GetComponent<ItemBase>().itemName;
//						textList[il].gameObject.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
//					} else {
//						l--;
//					}
//					il++;
//				}
//				
//				// Disable non-active buttons
//				for (int l = 0; i < textList.Length; i++)
//				{
//					if (textList[l].text == "<color=#ffffffff>N/A</color>")
//					{
//						textList[l].gameObject.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(false);
//					}
//					else
//					{
//						textList[l].gameObject.GetComponentsInChildren<Button>(true)[0].gameObject.SetActive(true);
//					}
//				}
//			}
//			if (inventoryJSON[i][2].ToString() != "null")
//			{
//				// Implement code for sorting prefabs
//			}			
//			if (inventoryJSON[i][3].ToString() != "null")
//			{
//				// Implement code for sorting prefabs
//			}
//		}
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

		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].GetComponent<ItemBase>() && items[i].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Knight)
			{
				GameObject newObject = (GameObject)Object.Instantiate(textObject);
				newObject.transform.SetParent(shopList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[i].GetComponent<ItemBase>().itemName;
			}
			else if (items[i].GetComponent<WeaponBase>())
			{
				GameObject newObject = (GameObject)Object.Instantiate(textObject);
				newObject.transform.SetParent(shopList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[i].GetComponent<WeaponBase>().weaponName;
			}
			else if (items[i].GetComponent<AbilityBase>())
			{
				GameObject newObject = (GameObject)Object.Instantiate(textObject);
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

		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].GetComponent<ItemBase>().bossItemType == itemType && items[i].GetComponent<ItemBase>().itemSide == ItemBase.ItemSide.Boss)
			{
				GameObject newObject = (GameObject)Object.Instantiate(textObject);
				newObject.transform.SetParent(shopList.transform);
				newObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				newObject.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
				newObject.GetComponent<Text>().text = items[i].GetComponent<ItemBase>().itemName;
			}
		}

		Destroy (textObject);
	}

	void Start()
	{
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
	}
}
