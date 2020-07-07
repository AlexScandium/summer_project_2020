///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 07/07/2020 18:46
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal {
	public class FollowingCamera : MonoBehaviour
	{
        #region Properties

        [SerializeField] private GameObject target = null;
        [SerializeField] private float speed = 1f;
        private Vector3 previousTargetPos;

        #endregion

        #region Methods
        private void Init()
        {
            previousTargetPos = target.transform.position;
        }

        #endregion

        #region UnityMethods

        private void Start()
        {
            Init();
        }

        private void LateUpdate()
        {
            Vector3 currentPos = target.transform.position;
            Debug.Log(previousTargetPos != currentPos);

            if (previousTargetPos != currentPos)
            {
                transform.position += (currentPos - previousTargetPos) * speed;
                previousTargetPos = currentPos;
            }
        }

        #endregion
    }
}