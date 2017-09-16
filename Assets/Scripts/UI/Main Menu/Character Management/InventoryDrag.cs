using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using System;
using SocketIO;

[RequireComponent(typeof(CanvasGroup))]
public class InventoryDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler { 
	
	public static GameObject itemBeingDragged;
    public static bool onInventoryDrag = false;
    public static bool swapped = false;
    public static bool droppedOnParent = false;

    public bool draggable = true;
	
	private Vector3 startPosition;
	private Transform startParent;
	private RectTransform rectTransform;
    private SocketIOComponent socket;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (Server.instance)
        {
            // Get socket object
            socket = FindObjectOfType<SocketIOComponent>();
        }
    }
	
	public void OnBeginDrag(PointerEventData eventData)
	{
        if (!draggable) return;

        ItemBase itemInfo = gameObject.GetComponent<ItemBase>();
        if (!itemInfo) return;
        if (itemInfo.itemName == null) return;

        itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;

        Popup.instance.gameObject.SetActive(false);
    }
	
	public void OnDrag(PointerEventData eventData)
	{
        if (!draggable) return;
        if (!itemBeingDragged) return;
        if (gameObject.GetComponent<ItemBase>().itemName == null) return;

        transform.position = Input.mousePosition;
	}
	
	public void OnEndDrag (PointerEventData eventData)
	{
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (!draggable) return;
        if (gameObject.GetComponent<ItemBase>().itemName == null) return;

        if (!Server.instance)
        {
            InventorySlot inventorySlot = GetComponent<InventorySlot>();
            if (swapped || InventorySlot.recievedParentName != null)
            {
                // Send request to update slot inventory after delay
                StartCoroutine(OnDungeonEndDrag());
            }
            else if (!swapped && inventorySlot && inventorySlot.clearOnDrop && !droppedOnParent)
            {
                ItemBase item = GetComponent<ItemBase>();
                if (item) Destroy(item);

                Image image = GetComponent<Image>();
                if (image) image.sprite = null;

                // Send request to update slot inventory after delay
                StartCoroutine(OnEndDrag());
            }
        }

        if (transform.parent == startParent)
		{
			transform.position = startPosition;
		}

        itemBeingDragged = null;
        swapped = false;
        droppedOnParent = false;
        InventorySlot.recievedParentName = null;
    }

    IEnumerator OnEndDrag()
    {
        yield return new WaitForEndOfFrame();
        // Send request to update slot inventory
        if (InventorySetter.instance) InventorySetter.SetInventory();
    }

    IEnumerator OnDungeonEndDrag()
    {
        yield return new WaitForEndOfFrame();
        // Send request to update slot inventory
        if (InventorySetter.instance) InventorySetter.SetInventory();
    }

    public void OnPointerEnter(PointerEventData eventData)
	{
        onInventoryDrag = true;
		if(GetComponent<ItemBase>())
		{
			ItemBase _item = GetComponent<ItemBase>();

            if (_item.itemName == null) return;

			if(!Popup.instance)
				Debug.LogError("Please make sure you have Popup Gameobject on your scene");

            if (InventoryDrag.itemBeingDragged) return;

            StartCoroutine("OpenPopup", _item);
		}
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Server.instance) return;

        ItemBase item = GetComponent<ItemBase>();
        InventorySlot inventorySlot = GetComponent<InventorySlot>();

        if (!item) return;
        if (!inventorySlot) return;

        // Set item slot id to be swapped
        int itemSlot = item.transform.GetSiblingIndex();
        if (transform.parent.name == "Defensive Skills / Items")
        {
            itemSlot = itemSlot + 17;
        }

        // TODO: Detect double click then equip (mainhand - left, offhand - right, others - left)
        if (eventData.clickCount % 2 == 0)
        {
            SocketIOComponent socket = CharacterManager.instance.socket;

            JSONObject ItemSwap = new JSONObject(JSONObject.Type.OBJECT);

            // Double click detected
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // Double left click
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

                    if (twoHanded)
                    {
                        ItemSwap.AddField("slotID", itemSlot);
                        ItemSwap.AddField("target", 9);
                        socket.Emit(SocketIOEvents.Output.Knight.CHANGE_EQUIPPED, ItemSwap);
                    }
                    else
                    {
                        ItemSwap.AddField("slotID", itemSlot);
                        ItemSwap.AddField("target", 2);
                        socket.Emit(SocketIOEvents.Output.Knight.CHANGE_EQUIPPED, ItemSwap);
                    }
                }

                if (item.isArmor())
                {
                    ItemSwap.AddField("slotID", itemSlot);
                    ItemSwap.AddField("target", 4);
                    socket.Emit(SocketIOEvents.Output.Knight.CHANGE_EQUIPPED, ItemSwap);
                }

                if (item.isAmmo())
                {
                    ItemSwap.AddField("slotID", itemSlot);
                    ItemSwap.AddField("target", 5);
                    socket.Emit(SocketIOEvents.Output.Knight.CHANGE_EQUIPPED, ItemSwap);
                }
            }

            // Double click detected
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                // Double left click
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

                    if (twoHanded)
                    {
                        ItemSwap.AddField("slotID", itemSlot);
                        ItemSwap.AddField("target", 9);
                        socket.Emit(SocketIOEvents.Output.Knight.CHANGE_EQUIPPED, ItemSwap);
                    }
                    else
                    {
                        ItemSwap.AddField("slotID", itemSlot);
                        ItemSwap.AddField("target", 3);
                        socket.Emit(SocketIOEvents.Output.Knight.CHANGE_EQUIPPED, ItemSwap);
                    }
                }

                if (item.isArmor())
                { 
                    ItemSwap.AddField("slotID", itemSlot);
                    ItemSwap.AddField("target", 4);
                    socket.Emit(SocketIOEvents.Output.Knight.CHANGE_EQUIPPED, ItemSwap);
                }

                if (item.isAmmo())
                {
                    ItemSwap.AddField("slotID", itemSlot);
                    ItemSwap.AddField("target", 5);
                    socket.Emit(SocketIOEvents.Output.Knight.CHANGE_EQUIPPED, ItemSwap);
                }
            }
        }
    }

    IEnumerator OpenPopup (ItemBase _item)
    {
        if (Server.instance)
        {
            yield return new WaitForSeconds(0.5f);
            
            // TODO: Use a custom pop-up for in game
            Popup.instance.gameObject.SetActive(true);

            // Always display to the left of ability/item
            Popup.instance.MenuDisplay(rectTransform.position - new Vector3(412, -16),
                                         _item);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            Popup.instance.gameObject.SetActive(true);
            if (Input.mousePosition.x > Screen.width / 2)
            {
                Popup.instance.MenuDisplay(rectTransform.position - new Vector3(412, -16),
                        _item);
            }
            else
            {
                Popup.instance.MenuDisplay(rectTransform.position + new Vector3(Screen.width / 21, Screen.height / 24),
                        _item);
            }
        }

        yield return 0;
    }

	public void OnPointerExit(PointerEventData eventData)
	{
        onInventoryDrag = false;

		if(!Popup.instance)
        {
            Debug.LogError("Please make sure you have Popup Gameobject on your scene");
        }

        StopCoroutine("OpenPopup");

        StartCoroutine(PointerExitHandler());
	}

    IEnumerator PointerExitHandler()
    {
        yield return new WaitForEndOfFrame();

        if (!Popup.onPopup)
        {
            Popup.instance.gameObject.SetActive(false);
        }
    }
}
