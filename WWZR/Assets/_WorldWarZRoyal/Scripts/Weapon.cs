///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 13/07/2020 13:18
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal
{
    [CreateAssetMenu(
		menuName = "Equipments/Weapon",
		fileName = "DefaultWeapon",
		order = 0
	)]
	public class Weapon : ScriptableObject {
		[SerializeField] private string _name = "default";
		public string Name => _name;

		[SerializeField] private WeaponType _type = WeaponType.NONE;
		public WeaponType Type => _type;

		[SerializeField] private float _damage = 1f;
		public float Damage => _damage;

		[SerializeField] private GameObject prefab = null;
		public GameObject Prefab => prefab;
	}

	public enum WeaponType
    {
		GUN = 0,
		BLUNT = 1,
		EDGED = 2,
		NONE = 10
    }
}