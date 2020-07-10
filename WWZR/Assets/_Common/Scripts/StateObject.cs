///-----------------------------------------------------------------
/// Author : Andy Bastel
/// Date : 11/11/2019 15:04
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.IsartDigital.Common {
	public class StateObject : MonoBehaviour {
        protected Action DoAction;

        virtual protected void Awake()
        {
        }
        virtual protected void Start () {
            Init();
		}

        virtual public void Init()
        {
            SetModeNormal();
        }

        virtual public void Passive()
        {
            SetModeVoid();
        }

        virtual protected void SetModeVoid()
        {
            DoAction = DoActionVoid;
        }

        virtual protected void DoActionVoid()
        {

        }
        virtual protected void SetModeNormal()
        {
            DoAction = DoActionNormal;
        }

        virtual protected void DoActionNormal()
        {
        }

        virtual protected void Update () {
            DoAction();
        }
	}
}