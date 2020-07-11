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
        private Vector3 previousTargetPos;
        [SerializeField] private Vector3 cameraFromPlayerPos = new Vector3();
        #endregion

        #region Methods
        private void Init()
        {
            SetStartCameraPosition();
            previousTargetPos = target.transform.position;
        }

        private void SetStartCameraPosition()
        {
            if (cameraFromPlayerPos == Vector3.zero) 
            {
                Debug.LogWarning("Forget to set a camera settings or to paste camera position to this Vector3," +
                    " it will use the position of the camera. Ignore this warning if you volontarily us the Vector3.zero as a parameter");
                cameraFromPlayerPos = transform.position;
            }
            
            transform.position = target.transform.position + cameraFromPlayerPos;
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

            if (previousTargetPos != currentPos)
            {
                transform.position += (currentPos - previousTargetPos) * speed;
                previousTargetPos = currentPos;
            }
        }

        #endregion
    }
}