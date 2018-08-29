/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TRUEStudios.Input {
	public class GamepadController : MonoBehaviour {
		#region Fields
		[SerializeField]
		private Gamepad[] _gamepads;

		private GameObject _target;
		private EventSystem _eventSystem;
		#endregion

		#region Private Methods
		private void Awake () {
			_eventSystem = GetComponent<EventSystem>();
			_target = _eventSystem.firstSelectedGameObject;
		}

		private void Update () {
			for (int i = 0; i < _gamepads.Length; ++i) {
				_gamepads[i].Update();
			}
		}

		private void SyncEventSystem () {
			// check if the target GameObject is out-of-sync with the EventSystem's selected GameObject
			if (_eventSystem && _target != _eventSystem.currentSelectedGameObject) {
				// check if no GameObject is currently selected
				if (_eventSystem.currentSelectedGameObject == null) {
					_eventSystem.SetSelectedGameObject(_target); // re-select the target GameObject
				} else {
					_target = _eventSystem.currentSelectedGameObject; // update the target GameObject
				}
			}
		}
		#endregion
	}
}
