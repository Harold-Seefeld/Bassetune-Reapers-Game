using UnityEngine;
using System.Collections;

public class ItemBase : MonoBehaviour
{

    public Sprite itemIcon;
    public string itemName;
    public string itemBuyPrice;
    public string itemSellPrice;
    public string itemDescription;
    public int itemCount;
    public int itemID;

    public bool isItem()
    {
        return !(itemID < 1000 || itemID >= 2500);
    }

    public bool isConsumable()
    {
        return !(itemID < 1000 || itemID >= 2400);
    }

    public bool isAmmo()
    {
        return !(itemID < 1900 || itemID >= 2000);
    }

    public bool isWeapon()
    {
        return !(itemID < 2000 || itemID >= 2400);
    }

    public bool isArmor()
    {
        return !(itemID < 2400 || itemID >= 2500);
    }

    public bool isOffensiveAbility()
    {
        return !(itemID < 2500 || itemID >= 2750);
    }

    public bool isDefensiveAbility()
    {
        return !(itemID < 2750 || itemID >= 3000);
    }
}
