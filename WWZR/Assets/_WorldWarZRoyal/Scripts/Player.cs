///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 07/07/2020 14:23
///-----------------------------------------------------------------

using Com.WWZR.WorldWarZRoyal.MobileObjects;
using System;
using System.Collections;
using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal {
	public class Player : Mobile
	{
        #region Properties

        [SerializeField] private float speed = 5f;
        [SerializeField] private float speedRotation = 5f;
        [SerializeField] private float speedExtremeRotation = 5f;
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
            Rotate();
            Move();
        }

        private void Move()
        {
            if (!Left && !Right && !Forward && !Back)
            {
                return;
            }

            //Avancée du joueur
            transform.position += transform.forward * Time.deltaTime * speed;
        }

        private void Rotate()
        {
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            if (!Left && !Right && !Forward && !Back)
            {
                return;
            }


            startLookPosition = transform.position + transform.forward;


            if (Forward)
            {
                endLookPosition = transform.position + cameraForward;
            }
            if (Back)
            {
                endLookPosition = transform.position - cameraForward;
            }

            if (Right)
            {
                endLookPosition = transform.position + cameraRight;
            }
            if (Left)
            {
                endLookPosition = transform.position - cameraRight;
            }

            if (LeftForward)
            {
                endLookPosition = transform.position + cameraForward - cameraRight;
            }
            if (LeftBack)
            {
                endLookPosition = transform.position - cameraForward + cameraRight;
            }
            if (RightForward)
            {
                endLookPosition = transform.position + cameraForward + cameraRight;
            }
            if (RightBack)
            {
                endLookPosition = transform.position - cameraForward - cameraRight;
            }


            Vector3 playerToForward = startLookPosition - transform.position;
            Vector3 playerToEnd = endLookPosition - transform.position;
            float angleBtwForwardAndEnd = Vector3.Angle(playerToForward, playerToEnd);

            if (!isExtremeRotation && angleBtwForwardAndEnd >= 90)
            {
                Debug.LogWarning("start extreme rotation");
                isExtremeRotation = true;
            }
            else if (isExtremeRotation && angleBtwForwardAndEnd <= 0.1f)
            {
                Debug.LogWarning("finish extreme rotation");
                isExtremeRotation = false;
            }

            // Move by LookAt
            lookPosition = Vector3.Lerp(
                startLookPosition,
                endLookPosition,
                (isExtremeRotation ? speedRotation * speedExtremeRotation : speedRotation) * Time.deltaTime);

            transform.LookAt(lookPosition);


            //Rotation instantanée
            //transform.rotation = Quaternion.LookRotation(endLookPosition, Vector3.up);

            //transform.rotation = Quaternion.Lerp(transform.rotation,
            //                                    Quaternion.LookRotation(endLookPosition, Vector3.up),
            //                                    (isExtremeRotation ? speedRotation * speedExtremeRotation : speedRotation) * Time.deltaTime);

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