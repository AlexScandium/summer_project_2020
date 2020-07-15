///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 15/07/2020 10:41
///-----------------------------------------------------------------

using Com.WWZR.WorldWarZRoyal.MobileObjects;
using UnityEditor;
using UnityEngine;

namespace Com.WWZR.WorldWarZRoyal
{
    [CustomEditor(typeof(Player))]
	[CanEditMultipleObjects]
	public class PlayerEditor : Editor
	{
		private Player currentPlayer;

        private void OnEnable()
        {
            currentPlayer = target as Player;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(15);
            EditorGUILayout.LabelField("Debug Weapons",EditorStyles.boldLabel);

            if (GUILayout.Button("Equip Stick"))
            {
                currentPlayer.AddStick();
            }
            if (GUILayout.Button("Equip Revolver"))
            {
                currentPlayer.AddRevolver();
            }
            if (GUILayout.Button("Remove weapon"))
            {
                currentPlayer.RemoveWeapon();
            }
            GUILayout.Space(15);
        }
    }
}