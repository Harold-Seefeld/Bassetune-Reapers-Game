using UnityEngine;
using System.Collections;

public class ItemBase : MonoBehaviour {

	public enum ItemSide {
		Knight,
		Boss
	}
	public ItemSide itemSide;

	public enum BossItemType {
		Boss,
		Miniboss,
		Trap,
		Creature
	}
	public BossItemType bossItemType;

	public Sprite itemIcon;
	public GameObject itemEffect;
	public string itemName;
	public string itemBuyPrice;
	public string itemSellPrice;
	public string itemDescription;
	public string itemType;
	// Index
	public int itemID;

}
