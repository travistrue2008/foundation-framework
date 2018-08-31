/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.Events;

namespace TRUEStudios.Foundation.Events {
	public abstract class EventListenerBase<T0, T1> : MonoBehaviour {
		public abstract void OnInvoke (T0 v0, T1 v1);
	}
	
	public abstract class EventListener<TUnityEvent, TEventHub, T0, T1> : EventListenerBase<T0, T1>
		where TUnityEvent : UnityEvent<T0, T1>, new()
		where TEventHub : EventHub<T0, T1> {

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

		public override void OnInvoke (T0 v0, T1 v1) {
			_onResponse.Invoke(v0, v1);
		}
		#endregion
	}
}
