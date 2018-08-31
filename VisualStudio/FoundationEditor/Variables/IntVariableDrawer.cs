/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TRUEStudios.Foundation.Variables {
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
