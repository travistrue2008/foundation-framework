/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;

namespace TRUEStudios.Variables {
	[Serializable]
	public class IntReference : BaseReference<IntVariable, int> { }

	[CreateAssetMenu(menuName = "TRUEStudios/Variables/Int", fileName = "New Variable (int)")]
	public class IntVariable : BaseVariable<int> { }
}
