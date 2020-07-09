///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 07/07/2020 21:40
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal.MobileObjects {
	public abstract class Mobile : MonoBehaviour
	{
        #region Properties

        protected Action DoAction;

        #endregion

        #region Methods

        protected abstract void Init();

		protected void SetModeWait() {
			DoAction = DoActionWait;
		}

		protected void SetModeMove()
        {
			DoAction = DoActionMove;
		}

		protected void DoActionWait() { }
		protected abstract void DoActionMove();

		protected abstract void Hit();

		protected abstract void Die();

        protected abstract void Destroy();

        #endregion

        #region Unity Methods

        protected void Update()
		{
			DoAction();
		}

        #endregion
    }
}