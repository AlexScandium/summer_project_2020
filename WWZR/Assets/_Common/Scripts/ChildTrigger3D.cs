///-----------------------------------------------------------------
/// Author : Andy BASTEL
/// Date : 26/06/2020 23:13
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.AndyBastel.ExperimentLab.Common {
	//[RequireComponent(typeof(Collider))]
	/// <summary>
	/// This class permit to send events when the collider of this MonoBehaviour is triggered (Is Trigger must be checked in the collider component).
	/// </summary>
	public class ChildTrigger3D : MonoBehaviour
	{

		public event Action<Collider> OnChildTriggerEnter;
		public event Action<Collider> OnChildTriggerStay;
		public event Action<Collider> OnChildTriggerExit;


		private void OnTriggerEnter(Collider other)
		{
			InvokeOnChildTriggerEnter(other);
		}

		private void OnTriggerStay(Collider other)
		{
			InvokeOnChildTriggerStay(other);
		}

		private void OnTriggerExit(Collider other)
		{
			InvokeOnChildTriggerExit(other);
		}

		//  EVENTS

		private void InvokeOnChildTriggerEnter(Collider other)
		{
			OnChildTriggerEnter?.Invoke(other);
		}

		private void InvokeOnChildTriggerStay(Collider other)
		{
			OnChildTriggerStay?.Invoke(other);
		}

		private void InvokeOnChildTriggerExit(Collider other)
		{
			OnChildTriggerExit?.Invoke(other);
		}
	}
}