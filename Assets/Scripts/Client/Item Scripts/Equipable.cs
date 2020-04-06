using UnityEngine;
using System.Collections;

public class Equipable : ItemBase {

	public enum ItemType {
        Boss,
        Miniboss,
        Creature,
        Trap
    }
    public ItemType itemType;

}
