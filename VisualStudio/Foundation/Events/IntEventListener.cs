/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine.Events;

namespace TRUEStudios.Foundation.Events {
	[Serializable]
	public class IntEvent : UnityEvent<int> {}

	public class IntEventListener : EventListener<IntEvent, IntEventHub, int> {}
}
