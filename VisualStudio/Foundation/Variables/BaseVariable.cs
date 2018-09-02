/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TRUEStudios.Foundation.Variables {
	[Serializable]
	public abstract class BaseReference<TValue, TVariable, TUnityEvent>
		where TVariable : BaseVariable<TValue>
		where TUnityEvent : UnityEvent<TValue>, new() {
		#region Fields
		public bool UseConstant = true;

		[SerializeField]
		public TValue _constantValue = default(TValue);
		[SerializeField]
		private TVariable _variable;
		[SerializeField]
		private TUnityEvent _onChange = new TUnityEvent();
		#endregion

		#region Properties
		public TUnityEvent OnChange { get { return _onChange; } }

		public TValue Value {
			set {
				bool changed = false;

				if (UseConstant) {
					changed = EqualityComparer<TValue>.Default.Equals(_constantValue, value);
					_constantValue = value;
				} else {
					changed = EqualityComparer<TValue>.Default.Equals(_variable.Value, value);
					_variable.Value = value;
				}

				if (changed) {
					_onChange.Invoke(value);
				}
			}

			get { return UseConstant ? _constantValue : _variable.Value; }
		}
		#endregion
	}

	public class BaseVariable<T> : ScriptableObject {
		#region
		[SerializeField]
		private bool _resetOnDestroy;
		[SerializeField]
		private T _initialValue;
		[SerializeField]
		private T _value;
		#endregion

		#region Properties
		public bool ResetOnDestroy {
			set { _resetOnDestroy = value; }
			get { return _resetOnDestroy; }
		}

		public T InitialValue {
			set { _initialValue = value; }
			get { return _initialValue; }
		}

		public T Value {
			set { _value = value; }
			get { return _value; }
		}
		#endregion

		#region ScriptableObject Hooks
		private void OnEnable () {
			_value = _initialValue;
		}
		#endregion
	}
}
