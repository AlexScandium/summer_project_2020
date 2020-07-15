///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 15/07/2020 11:46
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal.WeaponActions {
	public class GunAction : WeaponAction
	{
		[SerializeField] private Transform[] startPointsBullets = null;
		[SerializeField] private GameObject bulletPrefab = null;
		[SerializeField] private uint maxAmmos = 10;
		public uint MaxAmmos => maxAmmos;
		private uint leftAmmos;

        private void Start()
        {
			leftAmmos = maxAmmos;
        }

        public override void Shot()
        {
			if (leftAmmos < 1)
            {
				Debug.LogWarning("not enought ammo");
				return;
            }

			leftAmmos--;
			GameObject bullet = Instantiate(bulletPrefab);
			bullet.transform.position = startPointsBullets[0].position;
			bullet.transform.rotation = startPointsBullets[0].rotation;
		}

		public void AddAmmos(uint addedAmmos)
        {
			leftAmmos = (uint)Mathf.Clamp(leftAmmos + addedAmmos, 0, maxAmmos);
        }
	}
}