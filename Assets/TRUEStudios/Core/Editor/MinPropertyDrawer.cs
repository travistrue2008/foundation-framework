/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEditor;
using UnityEngine;

namespace TRUEStudios.Core {
	[CustomPropertyDrawer(typeof(MinAttribute))]
	public class MinPropertyDrawer : PropertyDrawer {
		#region Properties
		public MinAttribute Reference {
			get {
				return attribute as MinAttribute;
			}
		}
		#endregion

		#region Methods
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			// handle based on type
			EditorGUI.PropertyField(position, property, label);
			switch (property.propertyType)
			{
				case SerializedPropertyType.Integer:
					property.intValue = (int)Mathf.Max(Reference.min, property.intValue);
					break;

				case SerializedPropertyType.Float:
					property.floatValue = Mathf.Max(Reference.min, property.floatValue);
					break;

				default:
					EditorGUI.LabelField(position, label.text, "Use with type \"int\" or \"float\".");
					break;
			}
		}
		#endregion
	}
}
