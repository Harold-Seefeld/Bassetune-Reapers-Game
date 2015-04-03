using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class AbilitySlot : MonoBehaviour, IDropHandler
{
    private enum SlotType
    {
        Item,
        Defence,
        Offence
    }
    [SerializeField] private SlotType slotType;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject item = InventoryDrag.itemBeingDragged;

        if (slotType == SlotType.Item)
        {
            if (item.GetComponent<ItemBase>())
            {
                item.transform.SetParent(transform);
                gameObject.GetComponent<Image>().sprite = item.GetComponent<ItemBase>().itemIcon;
                item.SetActive(false);
            }
        }
        else if (slotType == SlotType.Defence)
        {
            if (item.GetComponent<AbilityBase>() && item.GetComponent<AbilityBase>().abilityType == AbilityType.Defensive)
            {
                item.transform.SetParent(transform);
                gameObject.GetComponent<Image>().sprite = item.GetComponent<AbilityBase>().icon;
                item.SetActive(false);
            }
        }
        else if (slotType == SlotType.Offence)
        {
            if (item.GetComponent<AbilityBase>() && item.GetComponent<AbilityBase>().abilityType == AbilityType.Offensive)
            {
                item.transform.SetParent(transform);
                gameObject.GetComponent<Image>().sprite = item.GetComponent<AbilityBase>().icon;
                item.SetActive(false);
            }  
        }
    }

}
