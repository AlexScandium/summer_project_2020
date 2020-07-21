///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 07/07/2020 18:46
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal.Cameras {
	public class FollowingCamera : MonoBehaviour
	{
        #region Properties

        [SerializeField] private GameObject target = null;
        [SerializeField] private float speed = 1f;
        [SerializeField] private float nearSpeed = 4f;
        private float startSpeed;
        private Vector3 previousTargetPos;
        [SerializeField] private Vector3 cameraOffset = new Vector3();
        #endregion

        #region Methods
        private void Init()
        {
            startSpeed = speed;
            SetStartCameraPosition();
            previousTargetPos = target.transform.position;
        }

        public void SetNearSpeed()
        {
            speed = nearSpeed;
        }

        public void ResetSpeed()
        {
            speed = startSpeed;
        }

        private void SetStartCameraPosition()
        {
            if (cameraOffset == Vector3.zero) 
            {
                Debug.LogWarning("Forget to set a camera settings or to paste camera position to this Vector3," +
                    " it will use the position of the camera. Ignore this warning if you volontarily us the Vector3.zero as a parameter");
                cameraOffset = transform.position;
            }
            
            transform.position = target.transform.position + cameraOffset;
        }

        #endregion

        #region UnityMethods

        private void Start()
        {
            Init();
        }

        private void LateUpdate()
        {
            Vector3 desiredPostion = target.transform.position + cameraOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPostion, speed * Time.deltaTime);
            transform.position = smoothedPosition;

        }

        #endregion
    }
}