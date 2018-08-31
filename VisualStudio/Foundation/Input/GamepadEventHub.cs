/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using TRUEStudios.Foundation.Events;

namespace TRUEStudios.Foundation.Input {
	[CreateAssetMenu(menuName = "TRUEStudios.Foundation/Input/Event Hub (int, Button)", fileName = "New Event Hub (int, Button)")]
	public class GamepadEventHub : EventHub<int, Button> {}
}
