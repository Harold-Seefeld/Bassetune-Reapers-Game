using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

[AddComponentMenu("Helper/Popup")]
public class Popup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public static Popup instance = null;
    public static bool onPopup = false;

	public RectTransform rectTransform;
	public Text nameText;
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

        Button[] buttons = gameObject.GetComponentsInChildren<Button>(true);
        for (int il = 0; il < buttons.Length; il++)
        {
            EventTrigger eventTrigger = buttons[il].gameObject.GetComponent<EventTrigger>();
            if (buttons[il].GetComponentsInChildren<Text>(true)[0].text == "Buy")
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
            else
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
