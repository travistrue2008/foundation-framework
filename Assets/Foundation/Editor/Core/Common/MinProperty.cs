﻿/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEditor;
using UnityEngine;


namespace Foundation.Framework
{
	[CustomPropertyDrawer(typeof(MinAttribute))]
	public class MinProperty : PropertyDrawer
	{
		#region
		public MinAttribute Target { get { return attribute as MinAttribute; } }
		#endregion


		#region Methods
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// handle based on type
			switch (property.propertyType)
			{
				case SerializedPropertyType.Integer:
					// cache the old value, and draw the property field
					int iValue = property.intValue;
					EditorGUI.PropertyField(position, property, label);

					// bound the property field's intValue by the attribute's minimum
					if (iValue != property.intValue)
						property.intValue = (int)Mathf.Max(Target.minimum, property.intValue);
					break;

				case SerializedPropertyType.Float:
					// cache the old value, and draw the property field
					float fValue = property.floatValue;
					EditorGUI.PropertyField(position, property, label);

					// bound the property field's floatValue by the attribute's minimum
					if (fValue != property.floatValue)
						property.floatValue = Mathf.Max(Target.minimum, property.floatValue);
					break;

				default:
					EditorGUI.LabelField(position, label.text, "Use with type \"int\" or \"float\".");
					break;
			}
		}
		#endregion
	}
}
