/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TRUEStudios.Variables {
	[Serializable]
	public abstract class BaseReference<TValue, TVariable, TUnityEvent>
		where TVariable : BaseVariable<TValue>
		where TUnityEvent : UnityEvent<TValue>, new() {
		public bool UseConstant = true;
		public TValue ConstantValue = default(TValue);
		public TVariable Variable;
		public TUnityEvent OnChange = new TUnityEvent();

		public TValue Value {
			set {
				bool changed = false;

				if (UseConstant) {
					changed = EqualityComparer<TValue>.Default.Equals(ConstantValue, value);
					ConstantValue = value;
				} else {
					changed = EqualityComparer<TValue>.Default.Equals(Variable.Value, value);
					Variable.Value = value;
				}

				if (changed) {
					OnChange.Invoke(value);
				}
			}

			get { return UseConstant ? ConstantValue : Variable.Value; }
		}
	}

	public class BaseVariable<T> : ScriptableObject {
		public T Value;
	}
}
