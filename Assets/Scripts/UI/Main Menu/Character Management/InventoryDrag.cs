using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class InventoryDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler{
	
	public static GameObject itemBeingDragged;
    public static bool onInventoryDrag = false;
    public static bool swapped = false;
    public bool draggable = true;
    public bool swappable = false;
    public bool clearOnDrop = false;
	
	private Vector3 startPosition;
	private Transform startParent;
	private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
	
	public void OnBeginDrag(PointerEventData eventData)
	{
        if (!draggable) return;

        if (!gameObject.GetComponent<ItemBase>()) return;

		itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	
	public void OnDrag(PointerEventData eventData)
	{
        if (!draggable) return;

        if (!itemBeingDragged) return;

        transform.position = Input.mousePosition;
	}
	
	public void OnEndDrag (PointerEventData eventData)
	{
        if (!draggable) return;

        if (clearOnDrop && !swapped)
        {
            ItemBase item = GetComponent<ItemBase>();
            if (item) Destroy(item);

            Image image = GetComponent<Image>();
            if (image) image.sprite = null;
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;

		if (transform.parent == startParent)
		{
			transform.position = startPosition;
		}

        itemBeingDragged = null;
        swapped = false;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
        onInventoryDrag = true;
		if(GetComponent<ItemBase>())
		{
			ItemBase _item = GetComponent<ItemBase>();

			if(!Popup.instance)
				Debug.LogError("Please make sure you have Popup Gameobject on your scene");

			Popup.instance.gameObject.SetActive (true);
            if (Input.mousePosition.x > Screen.width / 2)
            {
                Popup.instance.MenuDisplay(rectTransform.position - new Vector3(Screen.width / 4, Screen.height / 24),
                        _item);
            }
            else
            {
                Popup.instance.MenuDisplay(rectTransform.position + new Vector3(Screen.width / 21, Screen.height / 24),
                        _item);
            }
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
        onInventoryDrag = false;

		if(!Popup.instance)
        {
            Debug.LogError("Please make sure you have Popup Gameobject on your scene");
        }

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
