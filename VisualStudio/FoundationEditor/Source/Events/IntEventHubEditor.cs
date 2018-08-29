using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TRUEStudios.Events {
	[CustomEditor(typeof(IntEventHub))]
	public class IntEventHubEditor : EventHub1Editor<int> {
		#region Override Methods
		protected override int OnGetV0 (int value) {
			return EditorGUILayout.IntField("Value", value);
		}
		#endregion
	}
}
