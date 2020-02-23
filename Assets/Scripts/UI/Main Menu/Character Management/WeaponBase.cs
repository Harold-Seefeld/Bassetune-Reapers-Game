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

    public string weaponBuyPrice;
    public string weaponDescription;

    public Sprite weaponIcon;
    public string weaponName;
    public string weaponSellPrice;
    public WeaponType weaponType;
}