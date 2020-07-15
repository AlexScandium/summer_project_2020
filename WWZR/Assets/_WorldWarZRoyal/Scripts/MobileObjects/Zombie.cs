///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 11/07/2020 14:54
///-----------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal.MobileObjects {
    [RequireComponent(typeof(Rigidbody))]
    public class Zombie : Mobile
    {
        #region Properties

        [SerializeField] private const string MELEE_TAG = "Weapons/Melee";
        [SerializeField] private const string BULLET_TAG = "Weapons/Bullet";

        /// <summary>
        /// Duration of invulnerable state after a hit
        /// </summary>
        [SerializeField] private float invulnerabilityDuration = 1f;

        /// <summary>
        /// Bool defying the current state of the invulnerability
        /// </summary>
        private bool isInvulnerable = false;

        /// <summary>
        /// Rigidbody of the current GameObject
        /// </summary>
        private Rigidbody rb;

        #endregion

        #region Methods

        protected override void Init()
        {
            rb = GetComponent<Rigidbody>();
            SetModeWait();
        }

        private IEnumerator SetInvulnerableState()
        {
            isInvulnerable = true;
            yield return new WaitForSeconds(invulnerabilityDuration);
            isInvulnerable = false;
        }

        protected override void Hit()
        {
            throw new System.NotImplementedException();
        }

        protected override void DoActionMove()
        {
            throw new System.NotImplementedException();
        }
        
        protected override void Die()
        {
            Debug.Log(this + " is dead", this);
            Destroy(gameObject);
        }

        protected override void Destroy()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(MELEE_TAG) || other.CompareTag(BULLET_TAG))
            {
                if (isInvulnerable) return;

                StartCoroutine(SetInvulnerableState());
                Debug.Log("Hurt by " + other);

                LifePoint--;
                Vector3 weaponPos = other.transform.position;

                Vector3 propulsionDir = transform.position - weaponPos;
                propulsionDir.y = 0;
                propulsionDir = propulsionDir.normalized;

                rb.AddForce(propulsionDir * other.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
                rb.AddForce(Vector3.up * 2.5f, ForceMode.Impulse);
            }
        }
        #endregion
    }
}