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
	[CustomPropertyDrawer(typeof(BoolReference))]
	public class BoolVariableDrawer : BaseVariableDrawer {
		#region Overrides
		protected override void DrawConstantField(Rect contentRect, SerializedProperty constantProperty, GUIContent label) {
			bool prevValue = constantProperty.boolValue;
			bool nextValue = EditorGUI.Toggle(contentRect, label, prevValue);
			if (prevValue != nextValue) {
				constantProperty.boolValue = nextValue;
			}
		}
		#endregion
	}
}
