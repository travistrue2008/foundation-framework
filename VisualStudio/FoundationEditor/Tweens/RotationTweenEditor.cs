﻿/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TRUEStudios.Foundation.Tweens {
	[CustomEditor(typeof(RotationTween)), CanEditMultipleObjects]
	public class RotationTweenEditor : TweenEditor<RotationTween> {
		#region Fields
		private SerializedProperty _xModeProperty;
		private SerializedProperty _yModeProperty;
		private SerializedProperty _zModeProperty;
		#endregion

		#region Override Methods
		protected override void OnSetBegin (RotationTween target) {
			target.Begin = target.transform.localRotation.eulerAngles;
		}

		protected override void OnSetEnd (RotationTween target) {
			target.End = target.transform.localRotation.eulerAngles;
		}

		protected override void DrawAdditionalFields () {
			EditorGUILayout.BeginVertical();
			EditorGUILayout.PropertyField(_xModeProperty);
			EditorGUILayout.PropertyField(_yModeProperty);
			EditorGUILayout.PropertyField(_zModeProperty);
			EditorGUILayout.EndVertical();
		}
		#endregion

		#region Methods
		protected override void OnEnable () {
			base.OnEnable();
			_xModeProperty = serializedObject.FindProperty("_xMode");
			_yModeProperty = serializedObject.FindProperty("_yMode");
			_zModeProperty = serializedObject.FindProperty("_zMode");
		}
		#endregion
	}
}
