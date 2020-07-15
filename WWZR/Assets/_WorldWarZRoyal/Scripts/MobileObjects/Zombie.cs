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
        [SerializeField] private float invulnerabilityDuration = 1f;
        private Rigidbody rb;
        private bool isInvulnerable = false;

        #endregion

        #region Method

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
            Debug.Log("is dead", this);
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
            if (other.CompareTag("MeleeWeapon"))
            {
                if (!isInvulnerable) 
                {
                    StartCoroutine(SetInvulnerableState());
                    Debug.Log("hurt by melee weapon");
                    LifePoint--;
                    Vector3 weaponPos = other.transform.position;

                    Vector3 propulsionDir = transform.position - weaponPos;
                    propulsionDir.y = 0;
                    propulsionDir = propulsionDir.normalized;

                    rb.AddForce(propulsionDir * other.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
                    rb.AddForce(Vector3.up * 2.5f, ForceMode.Impulse);
                }
            }
        }
        #endregion
    }
}