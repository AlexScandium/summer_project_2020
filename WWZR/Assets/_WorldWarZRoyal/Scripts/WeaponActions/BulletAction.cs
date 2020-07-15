///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 15/07/2020 12:26
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal.WeaponActions {
	public class BulletAction : MonoBehaviour
	{

		[SerializeField] private float speed = 10f;
        [SerializeField] private float timeLife = 3f;

        private void OnEnable()
        {
            Destroy(this.gameObject, timeLife);
        }

        private void FixedUpdate()
        {
            MoveForward(speed);   
        }

        protected void MoveForward(float speed)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
    }
}