/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;
using TRUEStudios.Foundation.Events;

namespace TRUEStudios.Foundation.Variables {
	[Serializable]
	public class FloatReference : BaseReference<float, FloatVariable, FloatEvent> {}

	[CreateAssetMenu(menuName = "TRUEStudios/Foundation/Variables/Float", fileName = "New Variable (float)")]
	public class FloatVariable : BaseVariable<float> {}
}
