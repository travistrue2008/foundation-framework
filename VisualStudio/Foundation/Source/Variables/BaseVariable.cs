/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;

namespace TRUEStudios.Variables {
	[Serializable]
	public class BaseReference<TVariable, TValue> where TVariable : BaseVariable<TValue> {
		public bool UseConstant = true;
		public TValue ConstantValue = default(TValue);
		public TVariable Variable;

		public TValue Value {
			set {
				if (UseConstant) {
					ConstantValue = value;
				} else {
					Variable.Value = value;
				}
			}

			get { return UseConstant ? ConstantValue : Variable.Value; }
		}
	}

	public class BaseVariable<T> : ScriptableObject {
		public T Value;
	}
}
