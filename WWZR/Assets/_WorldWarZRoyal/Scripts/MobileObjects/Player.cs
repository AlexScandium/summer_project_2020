///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 07/07/2020 14:23
///-----------------------------------------------------------------

using Com.WWZR.WorldWarZRoyal.WeaponActions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal.MobileObjects
{
    public class Player : Mobile
	{
        #region Properties
                
        [SerializeField] private Animator animator = null;

        private const string TRIGGER_HIT = "Hit";

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

        /// <summary>
        /// Container of the gameObject of the weapon
        /// </summary>
        [SerializeField] private Transform weaponContainer = null;

        /// <summary>
        /// List of scriptable objects Weapon which can be equipped to the player
        /// </summary>
        [SerializeField] private List<Weapon> weaponList = new List<Weapon>();

        /// <summary>
        /// Custom object to save the informations of the current weapon equipped
        /// </summary>
        private class EquippedWeapon
        {
            public Weapon weapon;
            public GameObject weaponObject;

            public EquippedWeapon(Weapon wp = null, GameObject gameObject = null)
            {
                weapon = wp;
                weaponObject = gameObject;
            }
        }

        readonly private EquippedWeapon currentEquippedWeapon = new EquippedWeapon();

        private const string WEAPON_STICK = "Stick";
        private const string WEAPON_REVOLVER = "Revolver";

            #region Controller Getters
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
            GetCameraInfos();
            SetModeMove();
            GetCurrentWeaponStart();
        }

        private void GetCameraInfos()
        {
            mainCamera = Camera.main;

            cameraForward = mainCamera.transform.forward;
            cameraRight = mainCamera.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;
        }

        #region Moving methods
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
        #endregion

        #region Fighting methods
        protected override void Hit()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                DoHitAction();
        }

        private void GetCurrentWeaponStart()
        {
            if (weaponContainer.childCount > 0 && currentEquippedWeapon.weaponObject == null)
            {
                currentEquippedWeapon.weaponObject = weaponContainer.GetChild(0).gameObject;
                Weapon wp = weaponList.Find(x => x.Name == currentEquippedWeapon.weaponObject.GetComponent<WeaponAction>().Name);
                currentEquippedWeapon.weapon = wp;

                if (wp != null)
                {
                    switch (wp.Type)
                    {
                        case WeaponType.GUN:
                            SetHitModeGun();
                            break;
                        case WeaponType.BLUNT:
                            SetHitModeMelee();
                            break;
                        case WeaponType.EDGED:
                            SetHitModeMelee();
                            break;
                        case WeaponType.NONE:
                            SetHitModeNone();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    SetHitModeNone();
                }
            }
            else
                SetHitModeNone();
        }

        private void SetHitMode (Action action)
        {
            DoHitAction = action;
        }

        private void SetHitModeNone()
        {
            SetHitMode(DoHitActionNone);
        }

        private void DoHitActionNone() { }

        private void SetHitModeMelee()
        {
            SetHitMode(DoHitActionMelee);
        }

        private void DoHitActionMelee()
        {
            animator.SetTrigger(TRIGGER_HIT);
        }

        private void SetHitModeGun()
        {
            SetHitMode(DoHitActionGun);
        }

        private void DoHitActionGun()
        {
            currentEquippedWeapon.weaponObject.GetComponent<WeaponAction>().Shot();
        }

        public void AddMaxAmmosToEquippedWeapon()
        {
            if (currentEquippedWeapon.weapon == null) return;
            if(currentEquippedWeapon.weapon.Type == WeaponType.GUN)
            {
                GunAction gunAction = currentEquippedWeapon.weaponObject.GetComponent<GunAction>();

                gunAction.AddAmmos(gunAction.MaxAmmos);
            }
        }

        //private void AddAmmosToEquippedWeapon(float addedAmmos)
        //{
        //    if (currentEquippedWeapon.weapon == null) return;
        //    if (currentEquippedWeapon.weapon.Type == WeaponType.GUN)
        //    {
        //        GunAction gunAction = currentEquippedWeapon.weaponObject.GetComponent<GunAction>();

        //        gunAction.AddAmmos((uint)addedAmmos);
        //    }
        //}

        public void AddStick()
        {
            AddWeapon(WEAPON_STICK);
        }

        public void AddRevolver()
        {
            AddWeapon(WEAPON_REVOLVER);
        }

        public void RemoveWeapon()
        {
            if (Application.isEditor)
                GetCurrentWeaponStart();

            if (currentEquippedWeapon.weaponObject != null)
            {
                if (Application.isEditor)
                    DestroyImmediate(currentEquippedWeapon.weaponObject);
                else
                    Destroy(currentEquippedWeapon.weaponObject);

                currentEquippedWeapon.weaponObject = null;
                currentEquippedWeapon.weapon = null;
            }
        }

        private void AddWeapon(string weaponName)
        {
            Weapon wp = weaponList.Find(x => x.Name == weaponName);

            if (wp == null)
            {
                Debug.LogError("This weapon does not exist");
                return;
            }

            if (Application.isEditor)
                GetCurrentWeaponStart();

            if (currentEquippedWeapon.weaponObject != null)
            {
                if (Application.isEditor)
                    DestroyImmediate(currentEquippedWeapon.weaponObject);
                else
                    Destroy(currentEquippedWeapon.weaponObject);
            }

            currentEquippedWeapon.weaponObject = Instantiate(wp.Prefab, weaponContainer);
            currentEquippedWeapon.weapon = wp;

            switch (wp.Type)
            {
                case WeaponType.GUN:
                    SetHitModeGun();
                    break;
                case WeaponType.BLUNT:
                    SetHitModeMelee();
                    break;
                case WeaponType.EDGED:
                    SetHitModeMelee();
                    break;
                case WeaponType.NONE:
                    SetHitModeNone();
                    break;
                default:
                    break;
            }
        }

        #endregion

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