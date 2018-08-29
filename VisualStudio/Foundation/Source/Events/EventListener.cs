/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.Events;

namespace TRUEStudios.Events {
	public class EventListener : MonoBehaviour {
		#region Fields
		[SerializeField]
		private EventHub _hub;
		[SerializeField]
		private UnityEvent _onResponse;
		#endregion

		#region Properties
		public EventHub Hub { get { return _hub; } }
		public UnityEvent OnResponse { get { return _onResponse; } }
		#endregion

		#region Methods
		private void OnEnable () {
			_hub.RegisterListener(this);
		}

		private void OnDisable () {
			_hub.UnregisterListener(this);
		}

		public void OnInvoke () {
			_onResponse.Invoke();
		}
		#endregion
	}
}
