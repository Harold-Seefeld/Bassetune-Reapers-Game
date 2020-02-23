using UnityEngine;

public class WeaponBase : MonoBehaviour
{

    public enum WeaponType
    {
        Dagger,
        Knife,
        Sword,
        Longsword,
        Katana,
        Rapier,
        TwoHandedSword,
        GreatSword,
        Hammer,
        HeavyHammer,
        DualBladedAxeHammer,
        SmallHammer,
        GreatHammer,
        Axe,
        HeavyAxe,
        GreatBladedAxe,
        GreatAxe,
        Whip,
        Scythes,
        Spears,
        Shield,
        Shortbow,
        Longbow,
        LightCrossbow,
        HeavyCrossbow
    }
    public WeaponType weaponType;

    public Sprite weaponIcon;
    public string weaponName;
    public string weaponDescription;
    public string weaponBuyPrice;
    public string weaponSellPrice;

}
