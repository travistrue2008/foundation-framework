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
	[CustomPropertyDrawer(typeof(StringReference))]
	public class StringVariableDrawer : BaseVariableDrawer {
		#region Overrides
		protected override void DrawConstantField(Rect contentRect, SerializedProperty constantProperty, GUIContent label) {
			string prevValue = constantProperty.stringValue;
			string nextValue = EditorGUI.TextField(contentRect, label, prevValue);
			if (prevValue != nextValue) {
				constantProperty.stringValue = nextValue;
			}
		}
		#endregion
	}
}
