using UnityEngine;
using System.Collections;

public class Consumable : ItemBase {

    public float consumeTime = 1;
    public float duration = 1;
    public bool interruptedByStun = false;
    public bool interruptedByDamage = false;
    public bool interruptedByAbilityUse = false;
    public bool interruptedByMove = false;
    public string purpose = "heal";
    public float range = 5;
    public int value = 5;

}
