using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (inventoryPanel.activeInHierarchy)
                inventoryPanel.SetActive(false);
            else
                inventoryPanel.SetActive(true);
        }
    }
}