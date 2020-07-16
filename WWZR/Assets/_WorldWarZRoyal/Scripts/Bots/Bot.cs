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
		[SerializeField] protected ChildTrigger3D body = null;

		[SerializeField] protected uint life = 5;
		[SerializeField] protected uint damage = 1;
		[SerializeField] protected float timeBetweenDirectionChange = 3f;
		/// <summary>
		/// This property represents the variance of the time direction changes.
		/// If you assign 1 to this property it'll influence timeBetweenDirectionChange property
		/// by -0,5 or +0,5.
		/// </summary>
		[SerializeField, Range(0f, 2f)] protected float varianceDirectionChange = 1f;

		protected Transform target = null;
		protected Vector3 randomPoint = Vector3.zero;
		protected float elapsedTimeBetweenDirectionChange = 0f;
		protected float actualTimeBetweenDirectionChange = 0;

		protected override void Start()
		{
			base.Start();

			childTrigger.OnChildTriggerEnter += ChildTrigger_OnChildTriggerEnter;
			childTrigger.OnChildTriggerExit += ChildTrigger_OnChildTriggerExit;

			//body.OnChildTriggerEnter += Body_OnChildTriggerEnter;
			//body.OnChildTriggerExit += Body_OnChildTriggerExit;
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

			SetModeMove();
		}

		virtual protected void Body_OnChildTriggerEnter(Collider other)
		{
			//if (other.CompareTag("Player"))
			//{
			//	if (target && (transform.position - target.transform.position).magnitude < (transform.position - other.transform.position).magnitude) return;

			//	target = other.transform;

			//	Debug.Log(target);

			//	SetModeChase();
			//}
		}

		virtual protected void Body_OnChildTriggerExit(Collider other)
		{
			//if (other.transform == target)
			//	target = null;

			//SetModeMove();
		}

		#region DoActions

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

			transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);
			Rotate(target.position - transform.position);
		}

		protected override void SetModeMove()
		{
			base.SetModeMove();

			actualTimeBetweenDirectionChange = timeBetweenDirectionChange + UnityEngine.Random.Range(-varianceDirectionChange / 2, varianceDirectionChange / 2);

			randomPoint = UnityEngine.Random.insideUnitSphere * limitRadius;
			randomPoint.y = 0;
		}

		protected override void DoActionMove()
		{
			transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);

			Rotate(randomPoint - transform.position);


			if (elapsedTimeBetweenDirectionChange >= actualTimeBetweenDirectionChange)
			{
				SetModeMove();
				elapsedTimeBetweenDirectionChange = 0;
				return;
			}

			elapsedTimeBetweenDirectionChange += Time.deltaTime;
		}
		#endregion

		protected void Rotate(Vector3 forward)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(forward), speedRotation * Time.deltaTime);
		}

		protected override void Init()
		{
			SetModeMove();
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

		private void OnCollisionEnter(Collision collision)
		{
			Debug.Log("Collision with " + collision.collider.tag);
			if(collision.collider.GetComponent<Mobile>()) Hit(collision.collider.GetComponent<Mobile>(), damage);
		}

		protected override void Destroy()
		{
			childTrigger.OnChildTriggerEnter -= ChildTrigger_OnChildTriggerEnter;
			childTrigger.OnChildTriggerExit -= ChildTrigger_OnChildTriggerExit;

			Destroy(gameObject);
		}
	}
}