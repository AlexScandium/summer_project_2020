///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 21/07/2020 20:50
///-----------------------------------------------------------------

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal.Cameras {
    public class CameraZoomTrigger : MonoBehaviour
    {
        [SerializeField] private float orthograpicZoomValue = 5f;
        [SerializeField] private float zoomingDurationIn = 1f;
        [SerializeField] private float zoomingDurationOut = 2f;

        private Camera mainCamera;
        private FollowingCamera cameraFollow;

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
                mainCamera.DOOrthoSize(orthograpicZoomValue, zoomingDurationIn).SetEase(Ease.InOutCubic);
                cameraFollow.SetNearSpeed();
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                mainCamera.DOOrthoSize(startOrthographicSize, zoomingDurationOut).SetEase(Ease.OutSine);
                cameraFollow.ResetSpeed();
            }
        }
    }
}