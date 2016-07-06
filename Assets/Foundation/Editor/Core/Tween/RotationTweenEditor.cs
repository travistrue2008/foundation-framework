/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEditor;
using System.Net;
using System.Collections;
using System.Collections.Generic;


namespace Foundation.Framework
{
	[CustomEditor(typeof(RotationTween))]
	[CanEditMultipleObjects]
	public class RotationTweenEditor : TweenEditor<RotationTween>
	{
		#region Methods
		protected override void OnEnable()
		{
			base.OnEnable();
			EnableSetButtons = true;
		}

		protected override void OnAdditionalFields()
		{
			base.OnAdditionalFields();
			EditorGUILayout.BeginVertical();
			Reference.RotationModeX = (RotationTweenMode)EditorGUILayout.EnumPopup("X Rotation Mode", Reference.RotationModeX);
			Reference.RotationModeY = (RotationTweenMode)EditorGUILayout.EnumPopup("Y Rotation Mode", Reference.RotationModeY);
			Reference.RotationModeZ = (RotationTweenMode)EditorGUILayout.EnumPopup("Z Rotation Mode", Reference.RotationModeZ);
			EditorGUILayout.EndVertical();
		}

		protected override void OnBeginField()
		{
			base.OnBeginField();
			Reference.Begin = EditorGUILayout.Vector3Field("Begin", Reference.Begin);
		}

		protected override void OnEndField()
		{
			base.OnEndField();
			Reference.End = EditorGUILayout.Vector3Field("End", Reference.End);
		}

		protected override void OnBeginSet()
		{
			base.OnBeginSet();
			Reference.Begin = Reference.transform.rotation.eulerAngles;
		}

		protected override void OnEndSet()
		{
			base.OnEndSet();
			Reference.End = Reference.transform.rotation.eulerAngles;
		}
		#endregion
	}
}
