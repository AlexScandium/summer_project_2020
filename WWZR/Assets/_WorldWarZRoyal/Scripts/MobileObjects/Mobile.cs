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
		[SerializeField] protected float lifePoint = 3f;
		
		protected float LifePoint
        {
            get {
				return lifePoint; 
			}
            set {
				lifePoint = value;
				Debug.Log(String.Concat(this, " has got ", lifePoint, " left."),this);
				if (lifePoint <= 0)
                {
					Die();
                }
			}
        }
		protected Action DoAction;
		protected Action DoHitAction;

		#endregion

		#region Methods

		protected abstract void Init();

		protected void SetModeWait() {
			DoAction = DoActionWait;
		}

		virtual protected void SetModeMove()
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

		public abstract void Hit(Mobile mobile, uint damage);
		public abstract void Hurt(uint damage);

		protected abstract void Die();

        protected abstract void Destroy();

        #endregion

        #region Unity Methods

		protected virtual void Start()
        {
			Init();
        }

        protected virtual void Update()
		{
			DoAction();
		}

        #endregion
    }
}