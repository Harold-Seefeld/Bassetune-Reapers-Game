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
		
//		// Set all weapon text invisible
//		for (int i = 0; i < textList.Length; i++)
//		{
//			if (textList[l].transform.parent == transform)
//			{
//				textList[l].text = "<color=#ffffffff>N/A</color>";
//			}
//		}
//
//		// Set all ability text invisible
//		for (int i = 0; i < textList.Length; i++)
//		{
//			if (textList[l].transform.parent == transform)
//			{
//				textList[l].text = "<color=#ffffffff>N/A</color>";
//			}
//		}

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
			if (textToClear[i].transform.parent == transform)
			{
				textToClear[i].text = "<color=#ffffffff>N/A</color>";
			}
		}
	}
}
