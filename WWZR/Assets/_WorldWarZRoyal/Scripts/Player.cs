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
        [SerializeField] private float speedRotation = 5f;
        private Camera mainCamera = null;


        #region Getters
        private bool LeftForward
        {
            get { return (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.Z)); }
        }

        private bool LeftBack
        {
            get { return (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S)); }
        }
        
        private bool RightForward
        {
            get { return (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Z)); }
        }

        private bool RightBack
        {
            get { return (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.S)); }
        }

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

        private Vector3 startLookPosition = new Vector3();
        private Vector3 endLookPosition = new Vector3();
        private Vector3 lookPosition = new Vector3();
        private bool isExtremeRotation = false;

        protected override void DoActionMove()
        {
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;
            
            cameraForward.y = 0;
            cameraRight.y = 0;

            if (!Left && !Right && !Forward && !Back) return;

            startLookPosition = transform.position + transform.forward;

            if (Forward)
            {
                Debug.Log("Up");
                endLookPosition = transform.position + cameraForward;
            }
            if (Back)
            {
                Debug.Log("Down");
                endLookPosition = transform.position - cameraForward;
            }

            if (Right)
            {
                Debug.Log("Right");
                endLookPosition = transform.position + cameraRight;
            }
            if (Left)
            {
                Debug.Log("Left");
                endLookPosition = transform.position - cameraRight;
            }

            if (LeftForward) 
            {
                Debug.Log("LeftForward");
                endLookPosition = transform.position + cameraForward - cameraRight;
            }
            if (LeftBack) 
            {
                Debug.Log("LeftBack");
                endLookPosition = transform.position - cameraForward + cameraRight;
            }
            if (RightForward) 
            {
                Debug.Log("RightForward");
                endLookPosition = transform.position + cameraForward + cameraRight;
            }
            if (RightBack) 
            {
                Debug.Log("RightBack");
                endLookPosition = transform.position - cameraForward - cameraRight;
            }

            if (startLookPosition.x == -endLookPosition.x || startLookPosition.z == -endLookPosition.z)
            {
                Debug.LogWarning("start extreme rotation");
                isExtremeRotation = true;
            }

            
            lookPosition = Vector3.Lerp(
                startLookPosition,
                endLookPosition, 
                (isExtremeRotation ? speedRotation*2 : speedRotation) * Time.deltaTime);

            if (lookPosition == endLookPosition) 
            {
                isExtremeRotation = false;
            } 
            transform.LookAt(lookPosition);

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