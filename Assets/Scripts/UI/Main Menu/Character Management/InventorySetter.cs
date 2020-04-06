using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySetter : MonoBehaviour {

	[SerializeField] string slotInventorySite = "ec2-52-0-51-109.compute-1.amazonaws.com/slotInventory";
	public Sprite defaultImage;
	public InventoryManager inventoryManager;

	public void SetInventory()
	{
		Image[] inventoryIcons = GetComponentsInChildren<Image>();
		JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);

		for (int i = 0; i < inventoryIcons.Length; i++)
		{
			if (inventoryIcons[i].GetComponentsInChildren<ItemBase>(true).Length > 0)
			{
				ItemBase[] itemBase = inventoryIcons[i].GetComponentsInChildren<ItemBase>(true);
				for (int x = 0; i < inventoryManager.itemList.Length; i++)
				{
					if (inventoryManager.itemList[i].GetComponent<ItemBase>().itemName == itemBase[0].itemName)
					{
						JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
						jsonObject.AddField((inventoryIcons[i].transform.parent.GetSiblingIndex() * 3 + inventoryIcons[i].transform.GetSiblingIndex()).ToString(), arr);
						
						arr.Add((x + 1).ToString());
						arr.Add("item");
					}
				}
			}
			else if (inventoryIcons[i].GetComponentsInChildren<WeaponBase>(true).Length > 0)
			{
				WeaponBase[] weaponBase = inventoryIcons[i].GetComponentsInChildren<WeaponBase>(true);
				for (int x = 0; i < inventoryManager.itemList.Length; i++)
				{
					if (inventoryManager.weaponList[i].GetComponentsInChildren<WeaponBase>()[0].weaponName == weaponBase[0].weaponName)
					{
						JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
						jsonObject.AddField((inventoryIcons[i].transform.parent.GetSiblingIndex() * 3 + inventoryIcons[i].transform.GetSiblingIndex() + 1).ToString(), arr);
						
						arr.Add((x + 1).ToString());
						arr.Add("weapon");
					}
				}
			}
			else
			{
				jsonObject.AddField((inventoryIcons[i].transform.parent.GetSiblingIndex() * 3 + inventoryIcons[i].transform.GetSiblingIndex() + 1).ToString(), "null");
			}
		}
		Debug.Log(jsonObject.Print());

		WWWForm www = new WWWForm();
		www.AddField("uuid", inventoryManager.sessionManager.GetSession());
		www.AddField("j", jsonObject.Print());
		WWW w = new WWW (slotInventorySite, www.data);
		StartCoroutine(SetInventorySlot(w));
	}

	IEnumerator SetInventorySlot(WWW w)
	{
		yield return w;
		
		if (w.text != "Successfully Updated.")
		{
			inventoryManager.notificationRect.transform.gameObject.SetActive(true);
			inventoryManager.notificationRect.SetAsLastSibling();
			inventoryManager.notificationText.text = "An error occurred";
			inventoryManager.notificationButton.onClick.RemoveAllListeners();
			inventoryManager.notificationButton.onClick.AddListener(() => {inventoryManager.notificationRect.transform.gameObject.SetActive(false);});;
		}
	}

	void ResetInventory()
	{
		Image[] inventoryIcons = GetComponentsInChildren<Image>();

		for (int i = 0; i < inventoryIcons.Length; i++)
		{
			inventoryIcons[i].sprite = defaultImage;

			if (inventoryIcons[i].GetComponent<ItemBase>())
			{
				Destroy(inventoryIcons[i].GetComponent<ItemBase>());
			}
			else if (inventoryIcons[i].GetComponent<WeaponBase>())
			{
				Destroy(inventoryIcons[i].GetComponent<WeaponBase>());
			}
			else if (inventoryIcons[i].GetComponent<AbilityBase>())
			{
				Destroy(inventoryIcons[i].GetComponent<AbilityBase>());
			}
		}

		inventoryManager.UpdateInventory();
	}
}