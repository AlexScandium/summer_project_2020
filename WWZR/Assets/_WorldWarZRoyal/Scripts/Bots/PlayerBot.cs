///-----------------------------------------------------------------
/// Author : Andy BASTEL
/// Date : 28/06/2020 16:09
///-----------------------------------------------------------------

using Com.WWZR.WorldWarZRoyal;
using Com.WWZR.WorldWarZRoyal.Bots;
using Com.WWZR.WorldWarZRoyal.WeaponActions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.AndyBastel.ExperimentLab.ExperimentLab.IA {
	public class PlayerBot : ZombieBot {
		private const string TRIGGER_HIT = "Hit";

		/// <summary>
		/// Container of the gameObject of the weapon
		/// </summary>
		[SerializeField] private Transform weaponContainer = null;

		/// <summary>
		/// List of scriptable objects Weapon which can be equipped to the player
		/// </summary>
		[SerializeField] private List<Weapon> weaponList = new List<Weapon>();


		/// <summary>
		/// Custom object to save the informations of the current weapon equipped
		/// </summary>
		private class EquippedWeapon
		{
			public Weapon weapon;
			public GameObject weaponObject;

			public EquippedWeapon(Weapon wp = null, GameObject gameObject = null)
			{
				weapon = wp;
				weaponObject = gameObject;
			}
		}

		readonly private EquippedWeapon currentEquippedWeapon = new EquippedWeapon();

		private const string WEAPON_STICK = "Stick";
		private const string WEAPON_REVOLVER = "Revolver";


		protected override void Init()
		{
			base.Init();
			GetCurrentWeaponStart();
		}

		protected override void ChildTrigger_OnChildTriggerEnter(Collider other)
		{
			if (other.CompareTag(ZombieBot.TAG))
			{
				if (target && (transform.position - target.transform.position).magnitude < (transform.position - other.transform.position).magnitude) return;

				target = other.transform;
				if (currentEquippedWeapon.weapon.Type != WeaponType.GUN)
					SetModeChase();
				else SetModeEscape();
			}
		}

		#region DoActions

		private void GetCurrentWeaponStart()
		{
			if (weaponContainer.childCount > 0 && currentEquippedWeapon.weaponObject == null)
			{
				currentEquippedWeapon.weaponObject = weaponContainer.GetChild(0).gameObject;
				Weapon wp = weaponList.Find(x => x.Name == currentEquippedWeapon.weaponObject.GetComponent<WeaponAction>().Name);
				currentEquippedWeapon.weapon = wp;

				if (wp != null)
				{
					switch (wp.Type)
					{
						case WeaponType.GUN:
							SetHitModeGun();
							break;
						case WeaponType.BLUNT:
							SetHitModeMelee();
							break;
						case WeaponType.EDGED:
							SetHitModeMelee();
							break;
						case WeaponType.NONE:
							SetHitModeNone();
							break;
						default:
							break;
					}
				}
				else
				{
					SetHitModeNone();
				}
			}
			else
				SetHitModeNone();
		}

		private void SetHitMode(Action action)
		{
			DoHitAction = action;
		}

		private void SetHitModeNone()
		{
			SetHitMode(DoHitActionNone);
		}

		private void DoHitActionNone() { }

		private void SetHitModeMelee()
		{
			SetHitMode(DoHitActionMelee);
		}

		private void DoHitActionMelee()
		{
			animator.SetTrigger(TRIGGER_HIT);
		}

		private void SetHitModeGun()
		{
			SetHitMode(DoHitActionGun);
		}

		private void DoHitActionGun()
		{
			currentEquippedWeapon.weaponObject.GetComponent<WeaponAction>().Shot();
		}

		public void AddMaxAmmosToEquippedWeapon()
		{
			if (currentEquippedWeapon.weapon == null) return;
			if (currentEquippedWeapon.weapon.Type == WeaponType.GUN)
			{
				GunAction gunAction = currentEquippedWeapon.weaponObject.GetComponent<GunAction>();

				gunAction.AddAmmos(gunAction.MaxAmmos);
			}
		}

		//private void AddAmmosToEquippedWeapon(float addedAmmos)
		//{
		//    if (currentEquippedWeapon.weapon == null) return;
		//    if (currentEquippedWeapon.weapon.Type == WeaponType.GUN)
		//    {
		//        GunAction gunAction = currentEquippedWeapon.weaponObject.GetComponent<GunAction>();

		//        gunAction.AddAmmos((uint)addedAmmos);
		//    }
		//}

		public void AddStick()
		{
			AddWeapon(WEAPON_STICK);
		}

		public void AddRevolver()
		{
			AddWeapon(WEAPON_REVOLVER);
		}

		public void RemoveWeapon()
		{
			if (Application.isEditor)
				GetCurrentWeaponStart();

			if (currentEquippedWeapon.weaponObject != null)
			{
				if (Application.isEditor)
					DestroyImmediate(currentEquippedWeapon.weaponObject);
				else
					Destroy(currentEquippedWeapon.weaponObject);

				currentEquippedWeapon.weaponObject = null;
				currentEquippedWeapon.weapon = null;
			}
		}

		public void AddWeapon(string weaponName)
		{
			Weapon wp = weaponList.Find(x => x.Name == weaponName);

			if (wp == null)
			{
				Debug.LogError("This weapon does not exist");
				return;
			}

			if (Application.isEditor)
				GetCurrentWeaponStart();

			if (currentEquippedWeapon.weaponObject != null)
			{
				if (Application.isEditor)
					DestroyImmediate(currentEquippedWeapon.weaponObject);
				else
					Destroy(currentEquippedWeapon.weaponObject);
			}

			currentEquippedWeapon.weaponObject = Instantiate(wp.Prefab, weaponContainer);
			currentEquippedWeapon.weapon = wp;

			switch (wp.Type)
			{
				case WeaponType.GUN:
					SetHitModeGun();
					break;
				case WeaponType.BLUNT:
					SetHitModeMelee();
					break;
				case WeaponType.EDGED:
					SetHitModeMelee();
					break;
				case WeaponType.NONE:
					SetHitModeNone();
					break;
				default:
					break;
			}
		}

		private void SetModeEscape()
		{
			DoAction = DoActionEscape;
		}

		private void DoActionEscape()
		{
			if (target == null)
			{
				SetModeMove();
				return;
			}

			Vector3 direction = -(target.position - transform.position).normalized * speed * Time.deltaTime;
			direction.y = 0;
			transform.position += direction;

			Rotate(-direction);
		}
		#endregion

		private void Hit()
		{
			if (target) DoHitAction();
		}

		protected override void Update()
		{
			base.Update();
			Hit();
		}
	}
}