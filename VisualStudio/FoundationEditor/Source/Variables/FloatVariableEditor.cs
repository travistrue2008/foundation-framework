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
	public class FloatVariableEditor : PropertyDrawer {
		#region Methods
		public override void OnGUI (Rect rect, SerializedProperty property, GUIContent label) {
			int id = GUIUtility.GetControlID(FocusType.Passive);
			var useConstantProperty = property.FindPropertyRelative("UseConstant");
			bool useConstant = useConstantProperty.boolValue;

			EditorGUI.BeginProperty(rect, label, property);
			OnField(rect, property, label, useConstant);
			OnMenuButton(rect, property, useConstant);
			property.serializedObject.ApplyModifiedProperties();
			EditorGUI.EndProperty();
		}

		private void OnField (Rect rect, SerializedProperty property, GUIContent label, bool useConstant) {
			var propertyRect = GetPropertyRect(rect); // setup the property size rect
			var constantValueProperty = property.FindPropertyRelative("ConstantValue");
			var variableProperty = property.FindPropertyRelative("Variable");

			// either render the constant value, or a ScriptableObject reference
			if (useConstant) {
				float prevValue = constantValueProperty.floatValue;
				float nextValue = EditorGUI.FloatField(propertyRect, label, prevValue);
				if (prevValue != nextValue) {
					constantValueProperty.floatValue = nextValue;
				}
			} else {
				EditorGUI.PropertyField(propertyRect, variableProperty);
			}
		}

		private void OnMenuButton (Rect rect, SerializedProperty property, bool useConstant) {
			float height = GetPropertyHeight(property, null);
			var buttonPos = new Vector2(rect.position.x + (rect.size.x - height), rect.position.y);
			var buttonRect = new Rect(buttonPos, Vector2.one * height);

			// dropdown button
			if (EditorGUI.DropdownButton(buttonRect, new GUIContent(""), FocusType.Keyboard)) {
				GenericMenu menu = new GenericMenu();
				menu.AddItem(new GUIContent("Constant"), useConstant, () => SetProperty(property, true));
				menu.AddItem(new GUIContent("Variable"), !useConstant, () => SetProperty(property, false));
				menu.ShowAsContext();
			}
		}

		private void SetProperty (SerializedProperty property, bool useConstant) {
			property.FindPropertyRelative("UseConstant").boolValue = useConstant;
			property.serializedObject.ApplyModifiedProperties();
		}

		private Rect GetPropertyRect (Rect source) {
			// setup the property size rect
			var propertyRect = new Rect(source);
			var propertySize = propertyRect.size;
			propertySize.x -= 18.0f; // make room for the dropdown button
			propertyRect.size = propertySize;
			return propertyRect;
		}
		#endregion
	}
}
