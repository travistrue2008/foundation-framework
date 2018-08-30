/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;

namespace TRUEStudios.Variables {
	[Serializable]
	public class BoolReference : BaseReference<BoolVariable, bool> { }

	[CreateAssetMenu(menuName = "TRUEStudios/Variables/Bool", fileName = "New Variable (bool)")]
	public class BoolVariable : BaseVariable<bool> { }
}
