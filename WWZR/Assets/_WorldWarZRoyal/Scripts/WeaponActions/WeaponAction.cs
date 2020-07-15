///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 15/07/2020 12:23
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal.WeaponActions {
	public class WeaponAction : MonoBehaviour
	{
        #region Properties

        public string Name;

        #endregion

        #region Methods

        /// <summary>
        /// Action of the current weapon
        /// </summary>
        public virtual void Shot()
        {
        }

        #endregion
    }
}