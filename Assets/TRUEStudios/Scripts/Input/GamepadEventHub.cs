/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using TRUEStudios.Events;

namespace TRUEStudios.Input {
	[CreateAssetMenu(menuName = "TRUEStudios/Input/Event Hub (int, Button)", fileName = "New Event Hub (int, Button)")]
	public class GamepadEventHub : EventHub<int, Button> {}
}
