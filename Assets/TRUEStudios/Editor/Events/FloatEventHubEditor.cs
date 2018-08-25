using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TRUEStudios.Events {
	[CustomEditor(typeof(FloatEventHub))]
	public class FloatEventHubEditor : EventHub1Editor<float> {
		#region Override Methods
		protected override float OnGetV0 (float value) {
			return EditorGUILayout.FloatField("Value", value);
		}
		#endregion
	}
}
