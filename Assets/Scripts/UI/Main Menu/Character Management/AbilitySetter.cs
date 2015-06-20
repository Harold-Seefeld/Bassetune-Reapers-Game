using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilitySetter : MonoBehaviour
{

    [SerializeField]
    string slotInventorySite = "ec2-52-0-51-109.compute-1.amazonaws.com/slotAbilities";
    public Sprite defaultImage;
    public InventoryManager inventoryManager;

    public void SetAbilities()
    {
        Image[] abilityIcons = GetComponentsInChildren<Image>();
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);

        for (int i = 0; i < abilityIcons.Length; i++)
        {
            if (abilityIcons[i].GetComponentsInChildren<ItemBase>(true).Length > 0)
            {
                ItemBase[] itemBase = abilityIcons[i].GetComponentsInChildren<ItemBase>(true);
                for (int x = 0; i < inventoryManager.inventoryList.itemList.Length; i++)
                {
                    if (inventoryManager.inventoryList.itemList[i].GetComponent<ItemBase>().itemName == itemBase[0].itemName)
                    {
                        JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
                        jsonObject.AddField((abilityIcons[i].transform.parent.GetSiblingIndex() * 3 + abilityIcons[i].transform.GetSiblingIndex()).ToString(), arr);

                        arr.Add((x + 1).ToString());
                        arr.Add("item");
                    }
                }
            }
            else if (abilityIcons[i].GetComponentsInChildren<AbilityBase>(true).Length > 0)
            {
                AbilityBase[] abilityBase = abilityIcons[i].GetComponentsInChildren<AbilityBase>(true);
                for (int x = 0; i < inventoryManager.inventoryList.itemList.Length; i++)
                {
                    if (inventoryManager.inventoryList.abilityList[i].GetComponentsInChildren<AbilityBase>()[0].abilityName == abilityBase[0].abilityName)
                    {
                        JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
                        jsonObject.AddField((abilityIcons[i].transform.parent.GetSiblingIndex() * 3 + abilityIcons[i].transform.GetSiblingIndex() + 1).ToString(), arr);

                        arr.Add((x + 1).ToString());
                        arr.Add("ability");
                    }
                }
            }
            else
            {
                jsonObject.AddField((abilityIcons[i].transform.parent.GetSiblingIndex() * 3 + abilityIcons[i].transform.GetSiblingIndex() + 1).ToString(), "null");
            }
        }

        WWWForm www = new WWWForm();
        www.AddField("uuid", inventoryManager.sessionManager.GetSession());
        www.AddField("j", jsonObject.Print());
        WWW w = new WWW(slotInventorySite, www.data);
        StartCoroutine(SetAbilitySlot(w));
    }

    IEnumerator SetAbilitySlot(WWW w)
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

    void ResetAbilities()
    {
        Image[] abilityIcons = GetComponentsInChildren<Image>();

        for (int i = 0; i < abilityIcons.Length; i++)
        {
            abilityIcons[i].sprite = defaultImage;

            if (abilityIcons[i].GetComponent<ItemBase>())
            {
                Destroy(abilityIcons[i].GetComponent<ItemBase>());
            }
            else if (abilityIcons[i].GetComponent<AbilityBase>())
            {
                Destroy(abilityIcons[i].GetComponent<AbilityBase>());
            }
        }

        inventoryManager.UpdateInventory();
    }
}