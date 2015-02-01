using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

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
		for (int i = 0; i < inventoryJSON.Count - 1; i++)
		{
			if (inventoryJSON[i][0].ToString() != "null")
			{
				// Implement code for sorting prefabs
			}
			if (inventoryJSON[i][2].ToString() != "null")
			{
				// Implement code for sorting prefabs
			}			
			if (inventoryJSON[i][3].ToString() != "null")
			{
				// Implement code for sorting prefabs
			}
		}
	}
}
