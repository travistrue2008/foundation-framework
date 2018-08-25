using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TRUEStudios.Events;

public class IntEventTest : MonoBehaviour {
	#region Fields
	[SerializeField]
	private IntEvent _onSignaled = new IntEvent();

	private float _lastTime = 0.0f;
	#endregion

	#region MonoBehaviour Hooks
	private void Update () {
		int seconds = Mathf.FloorToInt(Time.timeSinceLevelLoad);
		if (seconds > Mathf.FloorToInt(_lastTime)) {
			_onSignaled.Invoke(seconds);
		}

		_lastTime = Time.timeSinceLevelLoad;
	}
	#endregion
}
