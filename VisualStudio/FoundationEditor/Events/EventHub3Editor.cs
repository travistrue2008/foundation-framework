using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TRUEStudios.Events {
	public abstract class EventHub3Editor<T0, T1, T2> : Editor {
		#region Fields
		private T0 _v0 = default(T0);
		private T1 _v1 = default(T1);
		private T2 _v2 = default(T2);
		#endregion

		#region Properties
		public EventHub<T0, T1, T2> Target {
			get { return (EventHub<T0, T1, T2>)target; }
		}
		#endregion

		#region Virtual Methods
		protected abstract T0 OnGetV0 (T0 value);
		protected abstract T1 OnGetV1 (T1 value);
		protected abstract T2 OnGetV2 (T2 value);
		#endregion

		#region Override Methods
		public override void OnInspectorGUI () {
			base.OnInspectorGUI();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Label");
			_v0 = OnGetV0(_v0);
			_v1 = OnGetV1(_v1);
			_v2 = OnGetV2(_v2);

			// raise the event if pressed
			if (GUILayout.Button("Raise")) {
				Target.Raise(_v0, _v1, _v2);
			}
		}
		#endregion
	}
}
