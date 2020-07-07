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
        private const string SPRINT_BUTTON = "Fire1";

        [SerializeField] private float speed = 5f;
        [SerializeField] private float maxSpeed = 7.5f;
        private float startSpeed;
        
        [SerializeField] private float acceleration = 0.5f;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            startSpeed = speed;
            SetModeMove();
        }

        private Action DoAction;
        

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
            Vector3 direction = new Vector3();

            if (Input.GetButton(SPRINT_BUTTON))
            {
                Debug.Log("on sprint");
                speed += acceleration * Time.deltaTime;
                speed = Mathf.Clamp(speed, startSpeed, maxSpeed);
            }
            else
            {
                speed = startSpeed;
            }

            if (horizontalAxisValue != 0)
            {
                direction.z= horizontalAxisValue * speed * Time.deltaTime;
            }
            if (verticalAxisValue != 0)
            {
                direction.x = -verticalAxisValue * speed * Time.deltaTime;
            }

            if (direction != Vector3.zero) transform.position += direction;
        }

        private void Update()
        {
            DoAction();
        }
    }
}