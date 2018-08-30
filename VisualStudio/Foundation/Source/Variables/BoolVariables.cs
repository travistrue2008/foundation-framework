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
	public class BoolReference : BaseReference<bool, BoolVariable, BoolEvent> { }

	[CreateAssetMenu(menuName = "TRUEStudios/Variables/Bool", fileName = "New Variable (bool)")]
	public class BoolVariable : BaseVariable<bool> { }
}
