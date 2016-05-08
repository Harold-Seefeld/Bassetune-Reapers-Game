using UnityEngine;
using System.Collections;

public class InventoryToggle : MonoBehaviour {

	public GameObject inventoryPanel;

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown("Inventory"))
		{
			if (inventoryPanel.activeInHierarchy)
			{
				inventoryPanel.SetActive(false);
			}
			else
			{
                inventoryPanel.SetActive(true);
                if (InventoryMenu.instance) InventoryMenu.instance.UpdateMenu();
            }
		}
	}
}
