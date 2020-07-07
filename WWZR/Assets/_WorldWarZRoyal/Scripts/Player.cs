///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 07/07/2020 14:23
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal {
	public class Player : MonoBehaviour
	{
        [SerializeField] private const string HORIZONTAL_AXIS = "Horizontal";
        [SerializeField] private const string VERTICAL_AXIS = "Vertical";
        [SerializeField] private float speed = 5f;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
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