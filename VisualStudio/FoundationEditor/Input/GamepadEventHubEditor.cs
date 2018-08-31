using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TRUEStudios.Events;

namespace TRUEStudios.Input {
	[CustomEditor(typeof(GamepadEventHub))]
	public class GamepadEventHubEditor : EventHub2Editor<int, Button> {
		#region Override Methods
		protected override int OnGetV0 (int value) {
			return EditorGUILayout.IntField("Channel", value);
		}

		protected override Button OnGetV1 (Button value) {
			return (Button)EditorGUILayout.EnumPopup("Button", value);
		}
		#endregion
	}
}
