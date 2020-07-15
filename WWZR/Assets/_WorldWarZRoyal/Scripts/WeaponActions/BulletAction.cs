///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 15/07/2020 12:26
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal.WeaponActions {
	public class BulletAction : MonoBehaviour
	{
        #region Properties

        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifeTime = 3f;

        #endregion

        #region Methods

        protected void MoveForward(float speed)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            Destroy(this.gameObject, lifeTime);
        }

        private void FixedUpdate()
        {
            MoveForward(speed);   
        }

        #endregion
    }
}