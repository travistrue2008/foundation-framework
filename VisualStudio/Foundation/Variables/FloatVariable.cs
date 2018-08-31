/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;
using TRUEStudios.Events;

namespace TRUEStudios.Variables {
	[Serializable]
	public class FloatReference : BaseReference<float, FloatVariable, FloatEvent> {}

	[CreateAssetMenu(menuName = "TRUEStudios/Variables/Float", fileName = "New Variable (float)")]
	public class FloatVariable : BaseVariable<float> {}
}
