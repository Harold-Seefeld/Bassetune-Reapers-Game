using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

[AddComponentMenu("Helper/Popup")]
public class Popup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public static Popup instance = null;
    public static bool onPopup = false;

	public RectTransform rectTransform;
	public Text nameText;
    public Text tagText;
	public Text typeText;
	public Text descriptionText;
    public Image icon;

    private InventoryManager inventoryManager;

	void Awake() {
		if (instance){
			Destroy(gameObject);
			return;
		}
		instance = this;
		rectTransform = GetComponent<RectTransform> ();
        inventoryManager = FindObjectOfType<InventoryManager>();
	}
    
    void Start()
    {
        gameObject.SetActive(false);
    }

	// Display function for menu popup
	public void MenuDisplay(Vector3 position, ItemBase item){
        position.x = position.x + 100;
        position.y = position.y + 60;
		rectTransform.position = position;
		nameText.text = item.itemName;
		typeText.text = item.itemCount.ToString();
		descriptionText.text = item.itemDescription;
        icon.sprite = item.itemIcon;
        rectTransform.SetAsLastSibling();

        InventorySlot inventorySlot = item.GetComponent<InventorySlot>();
        if (inventorySlot && !inventorySlot.slotTags.Contains(InventorySlot.SlotTag.Ability))
        {
            if (inventorySlot.slotTags.Count > 0)
            {
                tagText.text = inventorySlot.slotTags[0].ToString();
            }
            for (int i = 1; i < inventorySlot.slotTags.Count; i++)
            {
                tagText.text = tagText.text + " | " + inventorySlot.slotTags[i].ToString();
            }

            Button[] tButtons = gameObject.GetComponentsInChildren<Button>(true);
            for (int il = 0; il < tButtons.Length; il++)
            {
                if (tButtons[il].gameObject.name == "Slot1")
                {
                    tButtons[il].gameObject.SetActive(false);
                }
                else if (tButtons[il].gameObject.name == "Slot2")
                {
                    tButtons[il].gameObject.SetActive(false);
                }
            }

            // If weapon allow tags of Mainhand|Offhand, if two handed allow as Weapon
            if (item.isWeapon())
            {
                bool twoHanded = false;
                foreach (Weapon.TwoHanded weapon in Enum.GetValues(typeof(Weapon.TwoHanded)))
                {
                    if (inventorySlot.FindWeapon(item.itemID, InventoryManager.instance.items).weaponType.ToString() == weapon.ToString())
                    {
                        twoHanded = true;
                    }
                }

                // Create indicators for equipping it as a main weapon
                if (twoHanded && !inventorySlot.slotTags.Contains(InventorySlot.SlotTag.Mainhand))
                {
                    Button[] tagButtons = gameObject.GetComponentsInChildren<Button>(true);
                    for (int il = 0; il < tagButtons.Length; il++)
                    {
                        EventTrigger eventTrigger = tagButtons[il].gameObject.GetComponent<EventTrigger>();
                        if (tagButtons[il].gameObject.name == "Slot1")
                        {
                            eventTrigger.triggers.Clear();

                            EventTrigger.Entry entry = new EventTrigger.Entry();
                            entry.eventID = EventTriggerType.PointerClick;
                            entry.callback.AddListener((eventData) =>
                            {
                                inventorySlot.SetTag(InventorySlot.SlotTag.Mainhand, true);
                                inventorySlot.SetTag(InventorySlot.SlotTag.Offhand, false);

                                // Send request to update slot inventory
                                if (InventorySetter.instance) InventorySetter.SetInventory();

                                MenuDisplay(new Vector2(position.x - 100, position.y - 60), item);
                            });
                            eventTrigger.triggers.Add(entry);

                            tagButtons[il].GetComponentsInChildren<Text>(true)[0].text = "Weapon";
                            tagButtons[il].gameObject.SetActive(true);
                        }
                        else if (tagButtons[il].gameObject.name == "Slot2")
                        {
                            tagButtons[il].gameObject.SetActive(false);
                        }
                    }
                }
                else if (!inventorySlot.slotTags.Contains(InventorySlot.SlotTag.Mainhand) && !inventorySlot.slotTags.Contains(InventorySlot.SlotTag.Offhand))
                {
                    Button[] tagButtons = gameObject.GetComponentsInChildren<Button>(true);
                    for (int il = 0; il < tagButtons.Length; il++)
                    {
                        EventTrigger eventTrigger = tagButtons[il].gameObject.GetComponent<EventTrigger>();
                        eventTrigger.triggers.Clear();
                        EventTrigger.Entry entry = new EventTrigger.Entry();
                        if (tagButtons[il].gameObject.name == "Slot1")
                        {
                            entry.eventID = EventTriggerType.PointerClick;
                            entry.callback.AddListener((eventData) =>
                            {
                                inventorySlot.SetTag(InventorySlot.SlotTag.Mainhand, true);

                                // Send request to update slot inventory
                                if (InventorySetter.instance) InventorySetter.SetInventory();

                                MenuDisplay(new Vector2(position.x - 100, position.y - 60), item);
                            });
                            eventTrigger.triggers.Add(entry);

                            tagButtons[il].GetComponentsInChildren<Text>(true)[0].text = "Mainhand";
                            tagButtons[il].gameObject.SetActive(true);
                        }
                        else if (tagButtons[il].gameObject.name == "Slot2")
                        {
                            entry.eventID = EventTriggerType.PointerClick;
                            entry.callback.AddListener((eventData) =>
                            {
                                inventorySlot.SetTag(InventorySlot.SlotTag.Offhand, true);

                                // Send request to update slot inventory
                                if (InventorySetter.instance) InventorySetter.SetInventory();

                                MenuDisplay(new Vector2(position.x - 100, position.y - 60), item);
                            });
                            eventTrigger.triggers.Add(entry);

                            tagButtons[il].GetComponentsInChildren<Text>(true)[0].text = "Offhand";
                            tagButtons[il].gameObject.SetActive(true);
                        }
                    }
                }
                else if (!inventorySlot.slotTags.Contains(InventorySlot.SlotTag.Mainhand) && inventorySlot.slotTags.Contains(InventorySlot.SlotTag.Offhand))
                {
                    Button[] tagButtons = gameObject.GetComponentsInChildren<Button>(true);
                    for (int il = 0; il < tagButtons.Length; il++)
                    {
                        EventTrigger eventTrigger = tagButtons[il].gameObject.GetComponent<EventTrigger>();
                        eventTrigger.triggers.Clear();
                        EventTrigger.Entry entry = new EventTrigger.Entry();
                        if (tagButtons[il].gameObject.name == "Slot1")
                        {
                            entry.eventID = EventTriggerType.PointerClick;
                            entry.callback.AddListener((eventData) =>
                            {
                                inventorySlot.SetTag(InventorySlot.SlotTag.Mainhand, true);

                                // Send request to update slot inventory
                                if (InventorySetter.instance) InventorySetter.SetInventory();

                                MenuDisplay(new Vector2(position.x - 100, position.y - 60), item);
                            });
                            eventTrigger.triggers.Add(entry);

                            tagButtons[il].GetComponentsInChildren<Text>(true)[0].text = "Mainhand";
                            tagButtons[il].gameObject.SetActive(true);
                        }
                        else if (tagButtons[il].gameObject.name == "Slot2")
                        {
                            tagButtons[il].gameObject.SetActive(false);
                        }
                    }
                }
                else if (inventorySlot.slotTags.Contains(InventorySlot.SlotTag.Mainhand) && !inventorySlot.slotTags.Contains(InventorySlot.SlotTag.Offhand))
                {
                    Button[] tagButtons = gameObject.GetComponentsInChildren<Button>(true);
                    for (int il = 0; il < tagButtons.Length; il++)
                    {
                        EventTrigger eventTrigger = tagButtons[il].gameObject.GetComponent<EventTrigger>();
                        eventTrigger.triggers.Clear();
                        EventTrigger.Entry entry = new EventTrigger.Entry();
                        if (tagButtons[il].gameObject.name == "Slot1")
                        {
                            tagButtons[il].gameObject.SetActive(false);
                        }
                        else if (tagButtons[il].gameObject.name == "Slot2")
                        {
                            entry.eventID = EventTriggerType.PointerClick;
                            entry.callback.AddListener((eventData) =>
                            {
                                inventorySlot.SetTag(InventorySlot.SlotTag.Offhand, true);

                                // Send request to update slot inventory
                                if (InventorySetter.instance) InventorySetter.SetInventory();

                                MenuDisplay(new Vector2(position.x - 100, position.y - 60), item);
                            });
                            eventTrigger.triggers.Add(entry);

                            tagButtons[il].GetComponentsInChildren<Text>(true)[0].text = "Offhand";
                            tagButtons[il].gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    Button[] tagButtons = gameObject.GetComponentsInChildren<Button>(true);
                    for (int il = 0; il < tagButtons.Length; il++)
                    {
                        EventTrigger eventTrigger = tagButtons[il].gameObject.GetComponent<EventTrigger>();
                        eventTrigger.triggers.Clear();
                        EventTrigger.Entry entry = new EventTrigger.Entry();
                        if (tagButtons[il].gameObject.name == "Slot1")
                        {
                            tagButtons[il].gameObject.SetActive(false);
                        }
                        else if (tagButtons[il].gameObject.name == "Slot2")
                        {
                            tagButtons[il].gameObject.SetActive(false);
                        }
                    }
                }
            }
            // If ammo/bolt allow it to be equipped as ammo

            // If armor allow it to be equipped as 
            else if (item.isArmor())
            {
                Button[] tagButtons = gameObject.GetComponentsInChildren<Button>(true);
                for (int il = 0; il < tagButtons.Length; il++)
                {
                    EventTrigger eventTrigger = tagButtons[il].gameObject.GetComponent<EventTrigger>();
                    eventTrigger.triggers.Clear();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    if (tagButtons[il].gameObject.name == "Slot1")
                    {
                        entry.eventID = EventTriggerType.PointerClick;
                        entry.callback.AddListener((eventData) =>
                        {
                            inventorySlot.SetTag(InventorySlot.SlotTag.Armor, true);

                            // Send request to update slot inventory
                            if (InventorySetter.instance) InventorySetter.SetInventory();

                            inventorySlot.GetComponent<Outline>().effectColor = Color.yellow;

                            MenuDisplay(new Vector2(position.x - 100, position.y - 60), item);
                        });
                        eventTrigger.triggers.Add(entry);

                        tagButtons[il].GetComponentsInChildren<Text>(true)[0].text = "Armor";
                        tagButtons[il].gameObject.SetActive(true);
                    }
                    else if (tagButtons[il].gameObject.name == "Slot2")
                    {
                        tagButtons[il].gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            Button[] tagButtons = gameObject.GetComponentsInChildren<Button>(true);
            for (int il = 0; il < tagButtons.Length; il++)
            {
                if (tagButtons[il].gameObject.name == "Slot1")
                {
                    tagButtons[il].gameObject.SetActive(false);
                }
                else if (tagButtons[il].gameObject.name == "Slot2")
                {
                    tagButtons[il].gameObject.SetActive(false);
                }
            }
        }

        Button[] buttons = gameObject.GetComponentsInChildren<Button>(true);
        for (int il = 0; il < buttons.Length; il++)
        {
            EventTrigger eventTrigger = buttons[il].gameObject.GetComponent<EventTrigger>();
            if (buttons[il].gameObject.name == "Buy Button")
            {
                eventTrigger.triggers.Clear();

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((eventData) =>
                {
                    inventoryManager.BuyItem(item.itemID, 1);
                });
                eventTrigger.triggers.Add(entry);

                Text buttonText = buttons[il].GetComponentsInChildren<Text>(true)[0];

                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener(delegate {
                    ShopButtonChange(true, true, buttonText, item);
                });
                eventTrigger.triggers.Add(entry);

                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerExit;
                entry.callback.AddListener(delegate {
                    ShopButtonChange(true, false, buttonText, item);
                });
                eventTrigger.triggers.Add(entry);
            }
            else if (buttons[il].gameObject.name == "Sell Button")
            {
                eventTrigger.triggers.Clear();

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((eventData) => {
                    //if (item.itemCount > 0)
                    //{
                        inventoryManager.SellItem(item.itemID, 1);
                    //}
                });
                eventTrigger.triggers.Add(entry);

                Text buttonText = buttons[il].GetComponentsInChildren<Text>(true)[0];
                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener(delegate {
                    ShopButtonChange(false, true, buttonText, item);
                });
                eventTrigger.triggers.Add(entry);

                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerExit;
                entry.callback.AddListener(delegate {
                    ShopButtonChange(false, false, buttonText, item);
                });
                eventTrigger.triggers.Add(entry);
            }
        }
    }

    public void ShopButtonChange(bool isBuy, bool isEntered, Text buttonText, ItemBase item)
    {
        if (isBuy && isEntered)
        {
            buttonText.text = "G| " + item.itemBuyPrice.ToString();
        }
        else if (isBuy && !isEntered)
        {
            buttonText.text = "Buy";
        }
        else if (!isBuy && isEntered)
        {
            buttonText.text = "G| " + item.itemSellPrice.ToString();
        }
        else if (!isBuy && !isEntered)
        {
            buttonText.text = "Sell";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPopup = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPopup = false;
        if (!InventoryDrag.onInventoryDrag)
        {
            instance.gameObject.SetActive(false);
        }
    }
}
