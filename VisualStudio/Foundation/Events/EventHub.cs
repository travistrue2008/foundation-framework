/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace TRUEStudios.Foundation.Events {
	[CreateAssetMenu(menuName = "TRUEStudios.Foundation/Events/Hub", fileName = "New Event Hub")]
	public class EventHub : ScriptableObject {
		#region Fields
		private List<EventListener> _listeners = new List<EventListener>();
		#endregion

		#region Methods
		public void RegisterListener (EventListener listener) {
			if (_listeners.IndexOf(listener) == -1) {
				_listeners.Add(listener);
			}
		}

		public void UnregisterListener (EventListener listener) {
			if (_listeners.IndexOf(listener) != -1) {
				_listeners.Remove(listener);
			}
		}

		public void Raise () {
			for (int i = _listeners.Count - 1; i >= 0; --i) {
				_listeners[i].OnInvoke();
			}
		}
		#endregion
	}
}
