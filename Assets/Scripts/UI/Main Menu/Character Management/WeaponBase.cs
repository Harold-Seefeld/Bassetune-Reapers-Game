using UnityEngine;
using System.Collections;

public class WeaponBase : MonoBehaviour {

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
		Shortbow,
		Longbow,
		LightCrossbow,
		HeavyCrossbow
	}
	public WeaponType weaponType;

	public string weaponName;
	public string weaponDescription;
	public string weaponBuyPrice;
	public string weaponSellPrice;

}
