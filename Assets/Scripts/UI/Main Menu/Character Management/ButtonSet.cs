using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSet : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        var parentObject = transform.parent.gameObject;
        var buttons = parentObject.GetComponentsInChildren<Button>();

        for (var i = 0; i < buttons.Length; i++) buttons[i].interactable = true;

        gameObject.GetComponent<Button>().interactable = false;
    }
}