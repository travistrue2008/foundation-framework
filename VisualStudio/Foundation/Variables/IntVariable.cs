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
	public class IntReference : BaseReference<int, IntVariable, IntEvent> { }

	[CreateAssetMenu(menuName = "TRUEStudios.Foundation/Variables/Int", fileName = "New Variable (int)")]
	public class IntVariable : BaseVariable<int> { }
}
