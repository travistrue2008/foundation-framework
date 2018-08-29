using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TRUEStudios.Events {
	[CustomEditor(typeof(BoolEventHub))]
	public class BoolEventHubEditor : EventHub1Editor<bool> {
		#region Override Methods
		protected override bool OnGetV0 (bool value) {
			return EditorGUILayout.Toggle("Value", value);
		}
		#endregion
	}
}
