///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 15/07/2020 11:46
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal.WeaponActions {
	public class GunAction : WeaponAction
	{
        #region Properties

        /// <summary>
        /// Array of the points where the bullets need to be placed
        /// </summary>
        [SerializeField] private Transform[] startPointsBullets = null;

        /// <summary>
        /// Prefab of the bullet shot by the gun
        /// </summary>
		[SerializeField] private GameObject bulletPrefab = null;

        /// <summary>
        /// Maximum of ammos which can be stored on the weapon
        /// </summary>
		[SerializeField] private uint maxAmmos = 10;
		public uint MaxAmmos => maxAmmos;

        /// <summary>
        /// Count of ammo left on the gun
        /// </summary>
		private uint leftAmmos;

        #endregion

        #region Methods

        public override void Shot()
        {
			if (leftAmmos <= 0)
            {
				Debug.LogWarning("Not enought ammo");
				return;
            }

			leftAmmos--;
			GameObject bullet = Instantiate(bulletPrefab);

            Transform startPointBullet = startPointsBullets[0];

            bullet.transform.position = startPointBullet.position;
			bullet.transform.rotation = startPointBullet.rotation;
		}

		public void AddAmmos(uint addedAmmos)
        {
			leftAmmos = (uint)Mathf.Clamp(leftAmmos + addedAmmos, 0, maxAmmos);
        }

        #endregion

        #region Unity Methods

        private void Start()
        {
			leftAmmos = maxAmmos;
        }

        #endregion
    }
}