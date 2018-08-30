﻿/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;
using TRUEStudios.Events;

namespace TRUEStudios.Variables {
	[Serializable]
	public class StringReference : BaseReference<string, StringVariable, StringEvent> { }

	[CreateAssetMenu(menuName = "TRUEStudios/Variables/String", fileName = "New Variable (string)")]
	public class StringVariable : BaseVariable<string> { }
}
