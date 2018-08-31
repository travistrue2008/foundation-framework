/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TRUEStudios.Foundation.Variables {
	public abstract class BaseVariableDrawer : PropertyDrawer {
		#region Constants
		public const float FieldHeight = 16.0f;
		#endregion

		#region Abstract and Virtual Methods
		protected abstract void DrawConstantField (Rect contentRect, SerializedProperty constantProperty, GUIContent label);
		#endregion

		#region Overrides Methods
		public override void OnGUI (Rect rect, SerializedProperty property, GUIContent label) {
			var useConstantProperty = property.FindPropertyRelative("UseConstant");
			bool useConstant = useConstantProperty.boolValue;

			EditorGUI.BeginProperty(rect, label, property);
			DrawField(rect, property, label, useConstant);
			DrawMenuButton(rect, property, useConstant);
			DrawEvent(rect, property);
			property.serializedObject.ApplyModifiedProperties();
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
			var eventProperty = property.FindPropertyRelative("_onChange");
			return FieldHeight + EditorGUI.GetPropertyHeight(eventProperty);
		}
		#endregion

		#region Private Methods
		private void DrawField (Rect rect, SerializedProperty property, GUIContent label, bool useConstant) {
			var propertyRect = GetPropertyRect(rect); // setup the property size rect
			var constantValueProperty = property.FindPropertyRelative("_constantValue");
			var variableProperty = property.FindPropertyRelative("_variable");

			// either render the constant value, or a ScriptableObject reference
			if (useConstant) {
				DrawConstantField(propertyRect, constantValueProperty, label);
			} else {
				EditorGUI.PropertyField(propertyRect, variableProperty, label);
			}
		}

		private void DrawMenuButton (Rect rect, SerializedProperty property, bool useConstant) {
			var buttonPos = new Vector2(rect.position.x + (rect.size.x - FieldHeight), rect.position.y);
			var buttonRect = new Rect(buttonPos, Vector2.one * FieldHeight);

			// dropdown button
			if (EditorGUI.DropdownButton(buttonRect, new GUIContent(""), FocusType.Keyboard)) {
				GenericMenu menu = new GenericMenu();
				menu.AddItem(new GUIContent("Constant"), useConstant, () => SetProperty(property, true));
				menu.AddItem(new GUIContent("Variable"), !useConstant, () => SetProperty(property, false));
				menu.ShowAsContext();
			}
		}

		private void DrawEvent (Rect rect, SerializedProperty property) {
			var eventProperty = property.FindPropertyRelative("_onChange");

			var position = rect.position;

			var size = rect.size;
			size.y = EditorGUI.GetPropertyHeight(eventProperty);

			var eventRect = new Rect(position, size);
			eventRect.y += FieldHeight;

			EditorGUI.PropertyField(eventRect, eventProperty);
		}

		private void SetProperty (SerializedProperty property, bool useConstant) {
			property.FindPropertyRelative("UseConstant").boolValue = useConstant;
			property.serializedObject.ApplyModifiedProperties();
		}

		private Rect GetPropertyRect (Rect source) {
			// setup the property size rect
			var propertyRect = new Rect(source);
			var propertySize = propertyRect.size;
			propertySize.x -= FieldHeight + 2.0f; // make room for the dropdown button
			propertySize.y = FieldHeight;
			propertyRect.size = propertySize;
			return propertyRect;
		}
		#endregion
	}
}
