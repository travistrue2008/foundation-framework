using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TRUEStudios.Foundation.Events {
	[CustomEditor(typeof(StringEventHub))]
	public class StringEventHubEditor : EventHub1Editor<string> {
		#region Override Methods
		protected override string OnGetV0 (string value) {
			return EditorGUILayout.TextField("Value", value);
		}
		#endregion
	}
}
