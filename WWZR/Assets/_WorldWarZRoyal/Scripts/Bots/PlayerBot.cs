///-----------------------------------------------------------------
/// Author : Andy BASTEL
/// Date : 28/06/2020 16:09
///-----------------------------------------------------------------

using System;
using Com.DefaultCompany.ExperimentLab.ExperimentLab.IA;
using UnityEngine;

namespace Com.AndyBastel.ExperimentLab.ExperimentLab.IA {
	public class PlayerBot : Bot {

		protected override void ChildTrigger_OnChildTriggerEnter(Collider other)
		{
			if (other.CompareTag(Bot.TAG))
			{
				if (target && (transform.position - target.transform.position).magnitude < (transform.position - other.transform.position).magnitude) return;

				target = other.transform;
				SetModeEscape();
			}
		}

		private void SetModeEscape()
		{
			DoAction = DoActionEscape;
		}

		private void DoActionEscape()
		{
			if (target == null)
			{
				SetModeMove();
				return;
			}

			Vector3 direction = -(target.position - transform.position).normalized * speed * Time.deltaTime;
			direction.y = 0;
			transform.position += direction;

			Rotate(direction);
		}
	}
}