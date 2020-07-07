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

            if (horizontalAxisValue != 0)
            {
                transform.position += new Vector3(0,0, horizontalAxisValue * speed * Time.deltaTime);
            }
            if (verticalAxisValue != 0)
            {
                transform.position += new Vector3(-verticalAxisValue * speed * Time.deltaTime, 0,0 );
            }
        }

        private void Update()
        {
            DoAction();
        }
    }
}