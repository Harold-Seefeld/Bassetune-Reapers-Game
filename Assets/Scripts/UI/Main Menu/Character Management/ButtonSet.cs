using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSet : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject parentObject = transform.parent.gameObject;
        Button[] buttons = parentObject.GetComponentsInChildren<Button>() as Button[];

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }

        gameObject.GetComponent<Button>().interactable = false;
    }

}
