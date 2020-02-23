using UnityEngine;

public class ItemBase : MonoBehaviour
{

    public enum ItemSide
    {
        Knight,
        Boss
    }
    public ItemSide itemSide;

    public enum BossItemType
    {
        Boss,
        Miniboss,
        Trap,
        Creature
    }
    public BossItemType bossItemType;

    public Sprite itemIcon;
    public string itemAnimation;
    public string itemName;
    public string itemBuyPrice;
    public string itemSellPrice;
    public string itemDescription;
    public string itemType;

}
