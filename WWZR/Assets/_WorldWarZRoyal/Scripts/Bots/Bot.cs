///-----------------------------------------------------------------
/// Author : Andy BASTEL
/// Date : 26/06/2020 22:51
///-----------------------------------------------------------------

using System;
using Com.AndyBastel.ExperimentLab.Common;
using Com.IsartDigital.Common;
using Com.WWZR.WorldWarZRoyal.MobileObjects;
using UnityEngine;

namespace Com.DefaultCompany.ExperimentLab.ExperimentLab.IA {
	public class Bot : Mobile {
		public const string TAG = "Bot";

		public static float limitRadius;

		/// <summary>
		/// This is the detection area
		/// </summary>
		[SerializeField] protected ChildTrigger3D childTrigger = null;

		[SerializeField] protected uint life = 5;
		[SerializeField] protected uint damage = 1;

		protected Transform target = null;
		protected Vector3 randomPoint = Vector3.zero;

		private void Start()
		{
			Init();

			childTrigger.OnChildTriggerEnter += ChildTrigger_OnChildTriggerEnter;
			childTrigger.OnChildTriggerExit += ChildTrigger_OnChildTriggerExit;
		}

		virtual protected void ChildTrigger_OnChildTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				if (target && (transform.position - target.transform.position).magnitude < (transform.position - other.transform.position).magnitude) return;

				target = other.transform;

				Debug.Log(target);

				SetModeChase();
			}
		}

		virtual protected void ChildTrigger_OnChildTriggerExit(Collider other)
		{
			if (other.transform == target)
				target = null;

			SetModeMove();
		}

		private void SetModeChase()
		{
			DoAction = DoActionChase;
		}

		private void DoActionChase()
		{
			if (target == null)
			{
				SetModeMove();
				return;
			}
				//Debug.Log("if condition");

			//Debug.Log("Inside DoActionChase " + target);

			Vector3 targetOnPlanePos = target.position;
			targetOnPlanePos.y = transform.position.y;
			transform.position = Vector3.MoveTowards(transform.position, targetOnPlanePos, speed * Time.deltaTime);
			Rotate(transform.position - target.position);
		}

		protected override void SetModeMove()
		{
			base.SetModeMove();

			randomPoint = UnityEngine.Random.insideUnitSphere * limitRadius;
			randomPoint.y = 0;
		}

		protected override void DoActionMove()
		{
			transform.position = Vector3.MoveTowards(transform.position, randomPoint, speed * Time.deltaTime);

			Rotate(transform.position - randomPoint);

			if (transform.position == randomPoint) SetModeMove(); 
		}

		protected void Rotate(Vector3 forward)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(forward), speedRotation * Time.deltaTime);
		}

		protected override void Init()
		{
			SetModeMove();
		}

		virtual protected void OnTriggerEnter(Collider other)
		{

			if(other.GetComponent<Mobile>()) Hit(other.GetComponent<Mobile>(), damage);
		}



		public override void Hit(Mobile mobile, uint damage)
		{
			mobile.Hurt(damage);
		}

		public override void Hurt(uint damage)
		{
			life = (uint)Mathf.Clamp(life - damage, 0, life);

			if (life == 0) Die();
		}
		protected override void Die()
		{
			//Debug.Log("BAAAAHHHHH");
			Destroy();
		}

		protected override void Destroy()
		{
			childTrigger.OnChildTriggerEnter -= ChildTrigger_OnChildTriggerEnter;
			childTrigger.OnChildTriggerExit -= ChildTrigger_OnChildTriggerExit;

			Destroy(gameObject);
		}
	}
}