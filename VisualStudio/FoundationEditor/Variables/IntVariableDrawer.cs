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
	[CustomPropertyDrawer(typeof(IntReference))]
	public class IntVariableDrawer : BaseVariableDrawer {
		#region Overrides
		protected override void DrawConstantField(Rect contentRect, SerializedProperty constantProperty, GUIContent label) {
			int prevValue = constantProperty.intValue;
			int nextValue = EditorGUI.IntField(contentRect, label, prevValue);
			if (prevValue != nextValue) {
				constantProperty.intValue = nextValue;
			}
		}
		#endregion
	}
}
