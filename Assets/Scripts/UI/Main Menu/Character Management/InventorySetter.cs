using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySetter : MonoBehaviour
{

    private string slotInventorySite = "/slots";
    private ClientData clientData;

    public enum SlotType
    {
        knight_slots,
        boss_slots,
        ability_slots,
        equipped_slots
    }
    public SlotType slotType;

    public Sprite defaultImage;
    public InventoryManager inventoryManager;

    public static InventorySetter instanceAbility;
    public static InventorySetter instanceInventory;
    public static InventorySetter instance;

    public void Start()
    {
        if (gameObject.name == "Ability List") instanceAbility = this;
        if (gameObject.name == "Inventory List") instanceInventory = this;

        instance = this;

        slotInventorySite = inventoryManager.server + slotInventorySite;
        clientData = FindObjectOfType<ClientData>() as ClientData;
    }

    public static void SetInventory()
    {
        instanceAbility.SendInventory();
        instanceInventory.SendInventory();
    }

    public void SendInventory()
    {
        ItemBase[] itemBases = GetComponentsInChildren<ItemBase>(true);
        // Create a json object for storing the json arrays
        JSONObject jsonObject = new JSONObject(JSONObject.Type.ARRAY);
        for (int n = 0; n < itemBases.Length; n++)
        {
            // Create a new JSON array for storing the fields
            JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
            // Add the item ID
            arr.Add(itemBases[n].itemID.ToString());
            // TODO: Add item count
            arr.Add("1");
            // Set the slot number
            arr.Add(itemBases[n].transform.GetSiblingIndex());
            // Set the slot tag
            InventorySlot slot = itemBases[n].GetComponent<InventorySlot>();
            if (slot.slotTags.Contains(InventorySlot.SlotTag.Mainhand) && slot.slotTags.Contains(InventorySlot.SlotTag.Offhand))
            {
                arr.Add(9);
            }
            else
            {
                arr.Add((int)slot.slotTags[0]);
            }
            // Add the position of the inventory and add it to the main json object
            jsonObject.Add(arr);
        }

        WWWForm www = new WWWForm();
        www.AddField("uuid", clientData.GetSession());
        www.AddField("slotType", slotType.ToString());
        www.AddField("j", jsonObject.Print());
        Debug.Log(jsonObject.Print());
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

        Debug.Log(w.text);
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