using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TRUEStudios.Input;
using TMPro;

public class GamepadTest : MonoBehaviour {
	#region Fields
	[SerializeField]
	private Gamepad _gamepad;

	private TextMeshProUGUI _label;
	#endregion

	#region MonoBehaviour Hooks
	private void Awake () {
		_label = GetComponent<TextMeshProUGUI>();
	}

	private void Update () {
		_label.text = _gamepad.DebugText;
	}
	#endregion

	#region Actions
	public void PrintPressedButton (int channel, Button button) {
		Debug.Log($"PRESSED: Gamepad[{channel}] for button: {button}");
	}

	public void PrintReleasedButton (int channel, Button button) {
		Debug.Log($"RELEASED: Gamepad[{channel}] for button: {button}");
	}
	#endregion
}
