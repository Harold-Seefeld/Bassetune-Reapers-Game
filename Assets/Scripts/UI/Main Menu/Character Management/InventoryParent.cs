using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class InventoryParent : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        InventoryDrag.droppedOnParent = true;
    }

}
