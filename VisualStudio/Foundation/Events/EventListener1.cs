/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.Events;

namespace TRUEStudios.Foundation.Events {
	public abstract class EventListenerBase<T0> : MonoBehaviour {
		public abstract void OnInvoke (T0 v0);
	}
	
	public abstract class EventListener<TUnityEvent, TEventHub, T0> : EventListenerBase<T0>
		where TUnityEvent : UnityEvent<T0>, new()
		where TEventHub : EventHub<T0> {

		#region Fields
		[SerializeField]
		private TEventHub _hub;
		[SerializeField]
		private TUnityEvent _onResponse = new TUnityEvent();
		#endregion

		#region Properties
		public TEventHub Hub { get { return _hub; } }
		public TUnityEvent OnResponse { get { return _onResponse; } }
		#endregion

		#region Methods
		private void OnEnable () {
			_hub.RegisterListener(this);
		}

		private void OnDisable () {
			_hub.UnregisterListener(this);
		}

		public override void OnInvoke (T0 v0) {
			_onResponse.Invoke(v0);
		}
		#endregion
	}
}
