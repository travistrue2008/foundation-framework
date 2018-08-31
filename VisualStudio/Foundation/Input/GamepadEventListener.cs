/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine.Events;
using TRUEStudios.Foundation.Events;

namespace TRUEStudios.Foundation.Input {
	[Serializable]
	public class GamepadEvent : UnityEvent<int, Button> {}

	public class GamepadEventListener : EventListener<GamepadEvent, GamepadEventHub, int, Button> {}
}
