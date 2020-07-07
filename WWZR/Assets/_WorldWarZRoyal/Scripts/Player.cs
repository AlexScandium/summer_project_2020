///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 07/07/2020 14:23
///-----------------------------------------------------------------

using Com.WWZR.WorldWarZRoyal.MobileObjects;
using System;
using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal {
	public class Player : Mobile
	{
        #region Properties

        [SerializeField] private float speed = 5f;
        private Camera mainCamera = null;


        #region Getters
        private bool Left
        {
            get { return Input.GetKey(KeyCode.Q); }
        }

        private bool Right
        {
            get { return Input.GetKey(KeyCode.D); }
        }

        private bool Forward
        {
            get { return Input.GetKey(KeyCode.Z); }
        }

        private bool Back
        {
            get { return Input.GetKey(KeyCode.S); }
        }
            #endregion

        #endregion

        #region Methods

        protected override void Init()
        {
            mainCamera = Camera.main;
            SetModeMove();
        }

        protected override void DoActionMove()
        {
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;

            if (!Left && !Right && !Forward && !Back) return;

            if (Forward)
            {
                Debug.Log("Up");
                transform.LookAt(transform.position + cameraForward);
            }
            if (Back)
            {
                Debug.Log("Down");
                transform.LookAt(transform.position - cameraForward);
            }

            if (Right)
            {
                Debug.Log("Right");
                transform.LookAt(transform.position + cameraRight);
            }
            if(Left)
            {
                Debug.Log("Left");
                transform.LookAt(transform.position - cameraRight);
            }

            transform.position += transform.forward * Time.deltaTime * speed;
        }

        protected override void Hit()
        {
            throw new NotImplementedException();
        }

        protected override void Die()
        {
            throw new NotImplementedException();
        }

        protected override void Destroy()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Unity Methods

        private void Start()
        {
            Init();
        }

        #endregion
    }
}