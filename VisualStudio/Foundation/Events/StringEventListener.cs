/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine.Events;

namespace TRUEStudios.Foundation.Events {
	[Serializable]
	public class StringEvent : UnityEvent<string> {}

	public class StringEventListener : EventListener<StringEvent, StringEventHub, string> {}
}
