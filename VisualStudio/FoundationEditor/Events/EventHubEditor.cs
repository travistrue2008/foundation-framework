using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TRUEStudios.Foundation.Events {
	[CustomEditor(typeof(EventHub), true)]
	public class EventHubEditor : Editor {
		#region Properties
		public EventHub Target {
			get { return (EventHub)target; }
		}
		#endregion

		#region Override Methods
		public override void OnInspectorGUI () {
			base.OnInspectorGUI();

			// raise the event if pressed
			if (GUILayout.Button("Raise")) {
				Target.Raise();
			}
		}
		#endregion
	}
}
