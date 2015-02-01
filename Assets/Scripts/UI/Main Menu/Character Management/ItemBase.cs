using UnityEngine;
using System.Collections;

public class ItemBase : MonoBehaviour {

	public enum ItemSide {
		Knight,
		Boss
	}

	public string itemAnimation;
	public string itemName;
	public string itemBuyPrice;
	public string itemSellPrice;
	public string itemDescription;
}
