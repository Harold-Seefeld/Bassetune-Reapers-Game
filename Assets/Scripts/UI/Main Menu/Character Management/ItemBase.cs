using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public enum BossItemType
    {
        Boss,
        Miniboss,
        Trap,
        Creature
    }

    public enum ItemSide
    {
        Knight,
        Boss
    }

    public BossItemType bossItemType;
    public string itemAnimation;
    public string itemBuyPrice;
    public string itemDescription;

    public Sprite itemIcon;
    public string itemName;
    public string itemSellPrice;
    public ItemSide itemSide;
    public string itemType;
}