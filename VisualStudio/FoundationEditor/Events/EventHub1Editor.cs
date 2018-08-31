using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TRUEStudios.Foundation.Events {
	public abstract class EventHub1Editor<T0> : Editor {
		#region Fields
		private T0 _v0 = default(T0);
		#endregion

		#region Properties
		public EventHub<T0> Target {
			get { return (EventHub<T0>)target; }
		}
		#endregion

		#region Virtual Methods
		protected abstract T0 OnGetV0 (T0 value);
		#endregion

		#region Override Methods
		public override void OnInspectorGUI () {
			base.OnInspectorGUI();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Label");
			_v0 = OnGetV0(_v0);

			// raise the event if pressed
			if (GUILayout.Button("Raise")) {
				Target.Raise(_v0);
			}
		}
		#endregion
	}
}
