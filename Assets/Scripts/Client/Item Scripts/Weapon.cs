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
		Whip,
		Scythe,
		Spear,
		Shield,
		ShortBow,
        HorsebackBow,
        CompositeBow,
		LongBow,
        Goatbow,
		LightCrossbow,
        RepeaterCrossbow,
		HeavyCrossbow,
        Arbalest,
        AntiArmorCrossbow,
        AssaultCrossbow,
        HeadhunterCrossbow,
        BattleAxe,
        Hatchet,
        DoubleBitGreatAxe,
        GreatBladedAxe,
        GreatAxe,
        WarHammer,
        Javelin,
        ThrowingDagger,
        ThrowingKnife,
        ThrowingNeddle,
        Sling,
        Arrow,
        Rock,
        Bomb,
        Halberd,
        Lance,
        SpikedWhip,
        BallAndChain,
        Flail
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
