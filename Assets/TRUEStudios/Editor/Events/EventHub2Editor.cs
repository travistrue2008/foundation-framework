using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TRUEStudios.Events {
	public abstract class EventHub2Editor<T0, T1> : Editor {
		#region Fields
		private T0 _v0 = default(T0);
		private T1 _v1 = default(T1);
		#endregion

		#region Properties
		public EventHub<T0, T1> Target {
			get { return (EventHub<T0, T1>)target; }
		}
		#endregion

		#region Virtual Methods
		protected abstract T0 OnGetV0 (T0 value);
		protected abstract T1 OnGetV1 (T1 value);
		#endregion

		#region Override Methods
		public override void OnInspectorGUI () {
			base.OnInspectorGUI();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Label");
			_v0 = OnGetV0(_v0);
			_v1 = OnGetV1(_v1);

			// raise the event if pressed
			if (GUILayout.Button("Raise")) {
				Target.Raise(_v0, _v1);
			}
		}
		#endregion
	}
}
