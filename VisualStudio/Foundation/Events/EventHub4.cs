/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace TRUEStudios.Foundation.Events {
	public abstract class EventHub<T0, T1, T2, T3> : ScriptableObject {
		#region Fields
		private List<EventListenerBase<T0, T1, T2, T3>> _listeners = new List<EventListenerBase<T0, T1, T2, T3>>();
		#endregion

		#region Methods
		public void RegisterListener (EventListenerBase<T0, T1, T2, T3> listener) {
			if (_listeners.IndexOf(listener) == -1) {
				_listeners.Add(listener);
			}
		}

		public void UnregisterListener (EventListenerBase<T0, T1, T2, T3> listener) {
			if (_listeners.IndexOf(listener) != -1) {
				_listeners.Remove(listener);
			}
		}

		public void Raise (T0 v0, T1 v1, T2 v2, T3 v3) {
			for (int i = _listeners.Count - 1; i >= 0; --i) {
				_listeners[i].OnInvoke(v0, v1, v2, v3);
			}
		}
		#endregion
	}
}
