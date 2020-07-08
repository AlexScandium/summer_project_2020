///-----------------------------------------------------------------
/// Author : Andy BASTEL
/// Date : 26/06/2020 22:51
///-----------------------------------------------------------------

using System;
using Com.AndyBastel.ExperimentLab.Common;
using Com.IsartDigital.Common;
using UnityEngine;

namespace Com.DefaultCompany.ExperimentLab.ExperimentLab.IA {
	public class Bot : StateObject {
		public static string TAG = "Bot";

		[SerializeField] protected float speed = 15f;
		[SerializeField] protected float rotationSpeed = 180f;
		[SerializeField] protected ChildTrigger3D childTrigger = null;

		protected Transform target = null;
		protected Vector3 randomPoint = Vector3.zero;

		protected override void Start()
		{
			base.Start();

			childTrigger.OnChildTriggerEnter += ChildTrigger_OnChildTriggerEnter;
			childTrigger.OnChildTriggerExit += ChildTrigger_OnChildTriggerExit;
		}

		virtual protected void ChildTrigger_OnChildTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				if (target && (transform.position - target.transform.position).magnitude < (transform.position - other.transform.position).magnitude) return;

				target = other.transform;
				SetModeChase();
			}
		}

		virtual protected void ChildTrigger_OnChildTriggerExit(Collider other)
		{
			if (other.transform == target)
				target = null;

			SetModeNormal();
		}

		private void SetModeChase()
		{
			DoAction = DoActionChase;
		}

		private void DoActionChase()
		{
			transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
			Rotate(transform.position - target.position);
		}

		protected override void SetModeNormal()
		{
			base.SetModeNormal();

			randomPoint = UnityEngine.Random.insideUnitSphere * 50;
			randomPoint.y = 0;
		}

		protected override void DoActionNormal()
		{
			transform.position = Vector3.MoveTowards(transform.position, randomPoint, speed * Time.deltaTime);

			Rotate(transform.position - randomPoint);

			if (transform.position == randomPoint) SetModeNormal(); 
		}

		protected void Rotate(Vector3 forward)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(forward), rotationSpeed * Time.deltaTime);
		}
	}
}