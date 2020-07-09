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
        [Header("Move properties")]
        
        [SerializeField] private float speed = 5f;
        [SerializeField] private float speedRotation = 5f;

        /// <summary>
        /// Factor to accelerate the rotation when is on extreme rotation
        /// </summary>
        [SerializeField] private float speedExtremeRotation = 5f;

        /// <summary>
        /// Current main camera
        /// </summary>
        private Camera mainCamera = null;

        /// <summary>
        /// Vector Forward of the main camera
        /// </summary>
        private Vector3 cameraForward;

        /// <summary>
        /// Vector Right of the main camera
        /// </summary>
        private Vector3 cameraRight;

        /// <summary>
        /// Directing vector at the beginning of the rotation
        /// </summary>
        private Vector3 startOrientation = new Vector3();

        /// <summary>
        /// Directing vector of the target of the rotation
        /// </summary>
        private Vector3 endOrientation = new Vector3();

        /// <summary>
        /// Determine if a rotation is too high and need to accelerate the rotation
        /// </summary>
        private bool isExtremeRotation = false;

            #region Getters
        private bool IsNoKeyPressed { get => (!IsLeft && !IsRight && !IsForward && !IsBack); }
        private bool IsLeftForward { get => (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.Z)); }
        private bool IsLeftBack { get => (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S)); }
        private bool IsRightForward { get => (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Z)); }
        private bool IsRightBack { get => (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.S)); }
        private bool IsLeft { get => Input.GetKey(KeyCode.Q); }
        private bool IsRight { get => Input.GetKey(KeyCode.D); }
        private bool IsForward { get => Input.GetKey(KeyCode.Z); }
        private bool IsBack { get => Input.GetKey(KeyCode.S); }
            #endregion

        #endregion

        #region Methods

        protected override void Init()
        {
            mainCamera = Camera.main;

            cameraForward = mainCamera.transform.forward;
            cameraRight = mainCamera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            SetModeMove();
        }

        protected override void DoActionMove()
        {
            Rotate();
            Move();
        }

        /// <summary>
        /// Movement of the player by it forward
        /// </summary>
        private void Move()
        {
            if (IsNoKeyPressed) return;

            //Avancée du joueur
            transform.position = transform.position + transform.forward * Time.deltaTime * speed;
        }

        /// <summary>
        /// Rotate the player depending of the orientation of the camera
        /// </summary>
        private void Rotate()
        {
            //Detect if a key is pressed
            if (IsNoKeyPressed) return;

            //Determination of the orientation of the player
            startOrientation = transform.position + transform.forward;

            //Determination of the direction to look depending on the keys pressed
            if (IsLeftForward)
                endOrientation = transform.position + cameraForward - cameraRight;
            else if (IsLeftBack)
                endOrientation = transform.position - cameraForward + cameraRight;
            else if (IsRightForward)
                endOrientation = transform.position + cameraForward + cameraRight;
            else if (IsRightBack)
                endOrientation = transform.position - cameraForward - cameraRight;
            else if (IsForward)
                endOrientation = transform.position + cameraForward;
            else if (IsBack)
                endOrientation = transform.position - cameraForward;
            else if (IsRight)
                endOrientation = transform.position + cameraRight;
            else if (IsLeft)
                endOrientation = transform.position - cameraRight;

            //Determine if a rotation need to be accelerate
            Vector3 playerToEnd = endOrientation - transform.position;
            float angleBtwForwardAndEnd = Vector3.Angle(transform.forward, playerToEnd);

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

            //Rotate the player
            transform.rotation = Quaternion.Lerp(transform.rotation,
                                                Quaternion.LookRotation(playerToEnd, Vector3.up),
                                                (isExtremeRotation ? speedRotation * speedExtremeRotation : speedRotation) * Time.deltaTime);
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