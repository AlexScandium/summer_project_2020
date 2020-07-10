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

		[Header("Move properties")]
		[SerializeField] protected float speed = 5f;
		[SerializeField] protected float speedRotation = 180f;

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

		protected void MoveForward(float speed)
		{
			transform.position += transform.forward * Time.deltaTime * speed;
		}

		protected void Rotate(Vector3 direction, float rotationSpeed)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
		}

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