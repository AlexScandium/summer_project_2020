///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 21/07/2020 20:50
///-----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal.Cameras {
    public class CameraZoomTrigger : MonoBehaviour
    {
        private Camera mainCamera;
        private FollowingCamera cameraFollow;
        private List<IEnumerator> currentCoroutines = new List<IEnumerator>();
        private float startOrthographicSize;

        private void OnEnable()
        {
            mainCamera = Camera.main;
            cameraFollow = mainCamera.GetComponent<FollowingCamera>();
            startOrthographicSize = mainCamera.orthographicSize;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                mainCamera.orthographicSize = 5;
                cameraFollow.SetNearSpeed();
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                mainCamera.orthographicSize = startOrthographicSize;
                cameraFollow.ResetSpeed();
            }
        }
    }
}