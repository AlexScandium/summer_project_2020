///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 07/07/2020 14:23
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal {
	public class Player : MonoBehaviour
	{
        private const string HORIZONTAL_AXIS = "Horizontal";
        private const string VERTICAL_AXIS = "Vertical";

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

        private Action DoAction;

        private Camera mainCamera = null;

        [SerializeField] private float speed = 5f;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            mainCamera = Camera.main;
            SetModeMove();
        }

        private void SetModeWait()
        {
            DoAction = DoActionWait;
        }

        private void DoActionWait()
        {

        }

        private void SetModeMove()
        {
            DoAction = DoActionMove;
        }

        private void DoActionMove()
        {
            float horizontalAxisValue = Input.GetAxis(HORIZONTAL_AXIS);
            float verticalAxisValue = Input.GetAxis(VERTICAL_AXIS);

            Vector3 camF = mainCamera.transform.forward;
            Vector3 camR = mainCamera.transform.right;
            camF.y = 0;
            camR.y = 0;

            if (!Left && !Right && !Forward && !Back) return;

            if (verticalAxisValue > 0)
            {
                Debug.Log("Up");
                transform.LookAt(transform.position + camF);
                Debug.Log(transform.rotation);
                Debug.Log(transform.position + camF);
            }
            else if (verticalAxisValue < 0 )
            {
                Debug.Log("Down");
                transform.LookAt(transform.position - camF);
                Debug.Log(transform.rotation);
                Debug.Log(transform.position - camF);
            }

            if (horizontalAxisValue > 0)
            {
                Debug.Log("Right");
                transform.LookAt(transform.position + camR);
                Debug.Log(transform.rotation);
                Debug.Log(transform.position + camR);
            }
            else if(horizontalAxisValue < 0)
            {
                Debug.Log("Left");
                transform.LookAt(transform.position - camR);
                Debug.Log(transform.rotation);
                Debug.Log(transform.position - camR);
            }

            transform.position += transform.forward * Time.deltaTime * speed;
        }

        private void Update()
        {
            DoAction();
        }
    }
}