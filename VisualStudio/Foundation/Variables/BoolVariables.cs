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
	public class BoolReference : BaseReference<bool, BoolVariable, BoolEvent> { }

	[CreateAssetMenu(menuName = "TRUEStudios.Foundation/Variables/Bool", fileName = "New Variable (bool)")]
	public class BoolVariable : BaseVariable<bool> { }
}
