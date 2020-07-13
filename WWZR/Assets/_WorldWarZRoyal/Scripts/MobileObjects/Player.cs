///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 07/07/2020 14:23
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal.MobileObjects
{
    public class Player : Mobile
	{
        #region Properties
                
        /// <summary>
        /// Factor to accelerate the rotation when is on extreme rotation
        /// </summary>
        [SerializeField] private float speedRotationFactor = 5f;

        /// <summary>
        /// Angle from which the rotation must be accelerated
        /// </summary>
        [SerializeField] private float AngleForAcceleratedRotation = 90f;

        /// <summary>
        /// Percentage left of the rotation of the player when the acceleration stops
        /// </summary>
        [SerializeField, Range(0f, 1f)] private float endAccelerationPercentage = 0.1f;

        /// <summary>
        /// Directing vector of the target of the rotation
        /// </summary>
        private Vector3 orientationTargeted = new Vector3();

        /// <summary>
        /// Determine if a rotation is too high and need to accelerate the rotation
        /// </summary>
        private bool isLargeAngleRotation = false;

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


        [SerializeField] private Transform weaponContainer = null;
        [SerializeField] private List<Weapon> weaponList = new List<Weapon>();

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

        [SerializeField] private Animator animator;

        #endregion

        #region Methods

        protected override void Init()
        {
            GetCameraInfos();
            SetModeMove();
            AddStick();
            //AddRevolver();
        }

        private void GetCameraInfos()
        {
            mainCamera = Camera.main;

            cameraForward = mainCamera.transform.forward;
            cameraRight = mainCamera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;
        }

        protected override void DoActionMove()
        {
            //Detect if a key is pressed
            if (IsNoKeyPressed) return;

            RotateByCamera();
            MoveForward(speed);
        }

        /// <summary>
        /// Rotate the player depending of the orientation of the camera
        /// </summary>
        private void RotateByCamera()
        {
            //Determination of the direction to look depending on the keys pressed
            if (IsLeftForward)
                orientationTargeted = transform.position + cameraForward - cameraRight;
            else if (IsLeftBack)
                orientationTargeted = transform.position - cameraForward + cameraRight;
            else if (IsRightForward)
                orientationTargeted = transform.position + cameraForward + cameraRight;
            else if (IsRightBack)
                orientationTargeted = transform.position - cameraForward - cameraRight;
            else if (IsForward)
                orientationTargeted = transform.position + cameraForward;
            else if (IsBack)
                orientationTargeted = transform.position - cameraForward;
            else if (IsRight)
                orientationTargeted = transform.position + cameraRight;
            else if (IsLeft)
                orientationTargeted = transform.position - cameraRight;

            //Determine if a rotation need to be accelerate
            Vector3 directionToLook = orientationTargeted - transform.position;
            float angleBtwForwardAndEnd = Vector3.Angle(transform.forward, directionToLook);

            if (!isLargeAngleRotation && angleBtwForwardAndEnd >= AngleForAcceleratedRotation)
                isLargeAngleRotation = true;
            else if (isLargeAngleRotation && angleBtwForwardAndEnd <= endAccelerationPercentage)
                isLargeAngleRotation = false;

            Rotate(directionToLook, isLargeAngleRotation ? speedRotation * speedRotationFactor : speedRotation);
        }

        private void AddStick()
        {
            AddWeapon("Stick");
        }

        private void AddRevolver()
        {
            AddWeapon("Revolver");
        }

        private void AddWeapon(String weaponName)
        {
            Weapon wp = weaponList.Find(x => x.Name == weaponName);

            if (wp == null)
            {
                Debug.LogError("this weapon does not exist");
                return;
            }
            Instantiate(wp.Prefab, weaponContainer);
        }

        protected override void Hit()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Hit");
            }
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

        protected override void Update()
        {
            base.Update();
            Hit();
        }

        #endregion
    }
}