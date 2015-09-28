using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySetter : MonoBehaviour
{

    private string slotInventorySite = "";
    private ClientData clientData;

    public Sprite defaultImage;
    public InventoryManager inventoryManager;
    

    public void Start()
    {
        slotInventorySite = inventoryManager.server + "/slotInventory";
        clientData = FindObjectOfType<ClientData>() as ClientData;
    }

    public void SetInventory()
    {
        Image[] inventoryIcons = GetComponentsInChildren<Image>();
        // Create a json object for storing the json arrays
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);

        for (int i = 0; i < inventoryIcons.Length; i++)
        {
            if (inventoryIcons[i].GetComponentsInChildren<ItemBase>(true).Length > 0)
            {
                ItemBase[] itemBase = inventoryIcons[i].GetComponentsInChildren<ItemBase>(true);
                for (int n = 0; n < inventoryManager.items.Length; n++)
                {
                    GameObject[] prefabList = inventoryManager.items[n].prefabs;
                    for (int x = 0; i < prefabList.Length; i++)
                    {
                        ItemBase item = prefabList[x].GetComponent<ItemBase>();
                        if (item.itemID == itemBase[0].itemID)
                        {
                            // Create a new JSON array for storing the fields
                            JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
                            // Add the item ID
                            arr.Add(item.itemID.ToString());
                            // TODO: Add item count
                            arr.Add(item.itemCount.ToString());
                            // Say that the type is an item
                            arr.Add("i");
                            // Add the position of the inventory and add it to the main json object
                            jsonObject.AddField((inventoryIcons[i].transform.parent.GetSiblingIndex() * 3 + inventoryIcons[i].transform.GetSiblingIndex()).ToString(), arr);
                        }
                    }
                }
            }
        }

        WWWForm www = new WWWForm();
        www.AddField("uuid", clientData.GetSession());
        www.AddField("j", jsonObject.Print());
        WWW w = new WWW(slotInventorySite, www.data);
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
            inventoryManager.notificationButton.onClick.AddListener(() => { inventoryManager.notificationRect.transform.gameObject.SetActive(false); }); ;
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
            else if (inventoryIcons[i].GetComponent<Ability>())
            {
                Destroy(inventoryIcons[i].GetComponent<Ability>());
            }
        }

    }
}