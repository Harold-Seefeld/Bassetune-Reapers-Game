using UnityEngine;
using System.Collections;

public class Weapon : ItemBase {

	public enum WeaponType {
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
		ShortBow,
		LongBow,
		LightCrossbow,
		HeavyCrossbow
	}
    public WeaponType weaponType;

    public enum TwoHanded
    {
        TwoHandedSword,
        GreatSword,
        GreatHammer,
        GreatBladedAxe,
        GreatAxe,
        BallAndChain,
        Scythe,
        Lance,
        ShortBow,
        LongBow,
        HeavyCrossbow
    }


}
