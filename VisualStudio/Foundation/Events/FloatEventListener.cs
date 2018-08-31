﻿/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine.Events;

namespace TRUEStudios.Events {
	[Serializable]
	public class FloatEvent : UnityEvent<float> {}

	public class FloatEventListener : EventListener<FloatEvent, FloatEventHub, float> {}
}
