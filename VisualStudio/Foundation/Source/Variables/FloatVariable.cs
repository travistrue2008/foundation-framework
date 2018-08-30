/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;

namespace TRUEStudios.Variables {
	[Serializable]
	public class FloatReference {
		public bool UseConstant = true;
		public float ConstantValue = 0.0f;
		public FloatVariable Variable;

		public float Value {
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

	[CreateAssetMenu(menuName = "TRUEStudios/Variables/Float", fileName = "New Variable (float)")]
	public class FloatVariable : ScriptableObject {
		public float Value;
	}
}
