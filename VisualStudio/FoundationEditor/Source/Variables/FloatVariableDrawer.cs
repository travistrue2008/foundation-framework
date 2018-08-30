/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TRUEStudios.Variables {
	[CustomPropertyDrawer(typeof(FloatReference))]
	public class FloatVariableDrawer : BaseVariableDrawer {
		#region Overrides
		protected override void DrawConstantField(Rect contentRect, SerializedProperty constantProperty, GUIContent label) {
			float prevValue = constantProperty.floatValue;
			float nextValue = EditorGUI.FloatField(contentRect, label, prevValue);
			if (prevValue != nextValue) {
				constantProperty.floatValue = nextValue;
			}
		}
		#endregion
	}
}
