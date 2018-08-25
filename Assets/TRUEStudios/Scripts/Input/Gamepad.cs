/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TRUEStudios.Events;

namespace TRUEStudios.Input {
	public enum Button : int {
		A               = 1,
		B               = 2,
		X               = 4,
		Y               = 8,
		Up              = 16,
		Down            = 32,
		Left            = 64,
		Right           = 128,
		Back            = 256,
		Start           = 512,
		LeftBumper      = 1024,
		RightBumper     = 2048,
		LeftThumbstick  = 4096,
		RightThumbstick = 8192,
	}

	[CreateAssetMenu(menuName = "TRUEStudios/Input/Gamepad", fileName = "New Gamepad")]
	public class Gamepad : ScriptableObject {
		private readonly Button[] Buttons = (Button[])Enum.GetValues(typeof(Button));

		#region Fields
		[SerializeField]
		private int _channel = 0;
		[SerializeField]
		private string _aKey = "button_a";
		[SerializeField]
		private string _bKey = "button_b";
		[SerializeField]
		private string _xKey = "button_x";
		[SerializeField]
		private string _yKey = "button_y";
		[SerializeField]
		private string _backKey = "button_back";
		[SerializeField]
		private string _startKey = "button_start";
		[SerializeField]
		private string _leftBumperKey = "bumper_left";
		[SerializeField]
		private string _rightBumperKey = "bumper_right";
		[SerializeField]
		private string _leftThumbstickKey = "thumbstick_left";
		[SerializeField]
		private string _rightThumbstickKey = "thumbstick_right";
		[SerializeField]
		private string _leftTriggerKey = "trigger_left";
		[SerializeField]
		private string _rightTriggerKey = "trigger_right";
		[SerializeField]
		private string _dpadHorizontalKey = "dpad_horizontal";
		[SerializeField]
		private string _dpadVerticalKey = "dpad_vertical";
		[SerializeField]
		private string _leftXKey = "left_x";
		[SerializeField]
		private string _leftYKey = "left_y";
		[SerializeField]
		private string _rightXKey = "right_x";
		[SerializeField]
		private string _rightYKey = "right_y";
		[SerializeField]
		private GamepadEvent _onButtonPressed = new GamepadEvent();
		[SerializeField]
		private GamepadEvent _onButtonReleased = new GamepadEvent();

		private int _upPad = 0;
		private int _downPad = 0;
		private int _lastUpPad = 0;
		private int _lastDownPad = 0;
		private float _leftAngle = 0.0f;
		private float _rightAngle = 0.0f;
		private string _debugText = string.Empty;
		private Vector2 _leftAxis = Vector2.zero;
		private Vector2 _rightAxis = Vector2.zero;
		private GameObject _target;
		#endregion

		#region Properties
		public float LeftAngle { get { return _leftAngle; } }
		public float RightAngle { get { return _rightAngle; } }
		public string DebugText { get { return _debugText; } }
		public GamepadEvent OnButtonPressed { get { return _onButtonPressed; } }
		public GamepadEvent OnButtonReleased { get { return _onButtonReleased; } }
		public bool A { get { return UnityEngine.Input.GetButton(_aKey); } }
		public bool B { get { return UnityEngine.Input.GetButton(_bKey); } }
		public bool X { get { return UnityEngine.Input.GetButton(_xKey); } }
		public bool Y { get { return UnityEngine.Input.GetButton(_yKey); } }
		public bool Up { get { return UnityEngine.Input.GetAxis(_dpadVerticalKey) > 0.5f; } }
		public bool Down { get { return UnityEngine.Input.GetAxis(_dpadVerticalKey) < -0.5f; } }
		public bool Left { get { return UnityEngine.Input.GetAxis(_dpadHorizontalKey) < -0.5f; } }
		public bool Right { get { return UnityEngine.Input.GetAxis(_dpadHorizontalKey) > 0.5f; } }
		public bool Back { get { return UnityEngine.Input.GetButton(_backKey); } }
		public bool Start { get { return UnityEngine.Input.GetButton(_startKey); } }
		public bool LeftBumper { get { return UnityEngine.Input.GetButton(_leftBumperKey); } }
		public bool RightBumper { get { return UnityEngine.Input.GetButton(_rightBumperKey); } }
		public bool LeftThumbstick { get { return UnityEngine.Input.GetButton(_leftThumbstickKey); } }
		public bool RightThumbstick { get { return UnityEngine.Input.GetButton(_rightThumbstickKey); } }
		public float LeftTrigger { get { return UnityEngine.Input.GetAxis(_leftTriggerKey); } }
		public float RightTrigger { get { return UnityEngine.Input.GetAxis(_rightTriggerKey); } }

		public Vector2 LeftAxis {
			get {
				_leftAxis.Set(UnityEngine.Input.GetAxis(_leftXKey), UnityEngine.Input.GetAxis(_leftYKey));
				return _leftAxis;
			}
		}

		public Vector2 RightAxis {
			get {
				_rightAxis.Set(UnityEngine.Input.GetAxis(_rightXKey), UnityEngine.Input.GetAxis(_rightYKey));
				return _rightAxis;
			}
		}
		#endregion

		#region Actions
		public void Update () {
			ProcessAngles();
			ProcessButtonPresses();
			ProcessButtonReleases();
			UpdateLastPad();
			PrintDebug();
		}
		#endregion

		#region Private Methods
		private void ProcessAngles () {
			_leftAngle = Mathf.Atan2(LeftAxis.y, LeftAxis.x);
			_rightAngle = Mathf.Atan2(RightAxis.y, RightAxis.x);
		}

		private void ProcessButtonPresses () {
			int mask = 0;
			_downPad = 0;
			_downPad |= A ? (int)Button.A : 0;
			_downPad |= B ? (int)Button.B : 0;
			_downPad |= X ? (int)Button.X : 0;
			_downPad |= Y ? (int)Button.Y : 0;
			_downPad |= Up ? (int)Button.Up : 0;
			_downPad |= Down ? (int)Button.Down : 0;
			_downPad |= Left ? (int)Button.Left : 0;
			_downPad |= Right ? (int)Button.Right : 0;
			_downPad |= Back ? (int)Button.Back : 0;
			_downPad |= Start ? (int)Button.Start : 0;
			_downPad |= LeftBumper ? (int)Button.LeftBumper : 0;
			_downPad |= RightBumper ? (int)Button.RightBumper : 0;
			_downPad |= LeftThumbstick ? (int)Button.LeftThumbstick : 0;
			_downPad |= RightThumbstick ? (int)Button.RightThumbstick : 0;
			
			// check if the gamepad's state has changed
			for (int i = 0; i < Buttons.Length; ++i) {
				mask = 1 << i;
				if ((_lastDownPad & mask) == 0 && (_downPad & mask) == mask) {
					_onButtonPressed.Invoke(_channel, Buttons[i]);
				}
			}
		}

		private void ProcessButtonReleases () {
			int mask = 0;
			_upPad = 0;
			_upPad |= A ? 0 : (int)Button.A;
			_upPad |= B ? 0 : (int)Button.B;
			_upPad |= X ? 0 : (int)Button.X;
			_upPad |= Y ? 0 : (int)Button.Y;
			_upPad |= Up ? 0 : (int)Button.Up;
			_upPad |= Down ? 0 : (int)Button.Down;
			_upPad |= Left ? 0 : (int)Button.Left;
			_upPad |= Right ? 0 : (int)Button.Right;
			_upPad |= Back ? 0 : (int)Button.Back;
			_upPad |= Start ? 0 : (int)Button.Start;
			_upPad |= LeftBumper ? 0 : (int)Button.LeftBumper;
			_upPad |= RightBumper ? 0 : (int)Button.RightBumper;
			_upPad |= LeftThumbstick ? 0 : (int)Button.LeftThumbstick;
			_upPad |= RightThumbstick ? 0 : (int)Button.RightThumbstick;

			// check if the gamepad's state has changed
			for (int i = 0; i < Buttons.Length; ++i) {
				mask = 1 << i;
				if ((_lastUpPad & mask) == 0 && (_upPad & mask) == mask) {
					_onButtonReleased.Invoke(_channel, Buttons[i]);
				}
			}
		}

		private void UpdateLastPad () {
			// update last up/down states
			_lastUpPad = _upPad;
			_lastDownPad = _downPad;
		}

		private void PrintDebug () {
			_debugText = $@"LEGEND:
Triggers: ({LeftTrigger}, {RightTrigger})
Left Axis: {LeftAxis}
Right Axis: {RightAxis}
Up: {Up}
Down: {Down}
Left: {Left}
Right: {Right}
A: {A}
B: {B}
X: {X}
Y: {Y}
Back: {Back}
Start: {Start}
LeftBumper: {LeftBumper}
RightBumper: {RightBumper}
LeftThumbstick: {LeftThumbstick}
RightThumbstick: {RightThumbstick}
			";
		}
		#endregion
	}
}
