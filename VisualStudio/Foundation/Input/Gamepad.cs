/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TRUEStudios.Foundation.Events;

namespace TRUEStudios.Foundation.Input {
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

	[CreateAssetMenu(menuName = "TRUEStudios/Foundation/Input/Gamepad", fileName = "New Gamepad")]
	public class Gamepad : ScriptableObject {
		#region Constants
		private const string AKey = "button_a";
		private const string BKey = "button_b";
		private const string XKey = "button_x";
		private const string YKey = "button_y";
		private const string BackKey = "button_back";
		private const string StartKey = "button_start";
		private const string LeftBumperKey = "bumper_left";
		private const string RightBumperKey = "bumper_right";
		private const string LeftThumbstickKey = "thumbstick_left";
		private const string RightThumbstickKey = "thumbstick_right";
		private const string LeftTriggerKey = "trigger_left";
		private const string RightTriggerKey = "trigger_right";
		private const string DpadHorizontalKey = "dpad_horizontal";
		private const string DpadVerticalKey = "dpad_vertical";
		private const string LeftXKey = "left_x";
		private const string LeftYKey = "left_y";
		private const string RightXKey = "right_x";
		private const string RightYKey = "right_y";

		private readonly Button[] Buttons = (Button[])Enum.GetValues(typeof(Button));
		#endregion

		#region Fields
		[SerializeField]
		private int _channel = 1;
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

		#if UNITY_OSX || UNITY_EDITOR_OSX
		[NonSerialized]
		private bool _leftTriggerTouched = false;
		[NonSerialized]
		private bool _rightTriggerTouched = false;
		#endif
		
		#endregion

		#region Properties
		public float LeftAngle { get { return _leftAngle; } }
		public float RightAngle { get { return _rightAngle; } }
		public string DebugText { get { return _debugText; } }
		public GamepadEvent OnButtonPressed { get { return _onButtonPressed; } }
		public GamepadEvent OnButtonReleased { get { return _onButtonReleased; } }

		public bool A {
			get {
				string key = GetFullKey(AKey);
				return UnityEngine.Input.GetButton(key);
			}
		}
		
		public bool B {
			get {
				string key = GetFullKey(BKey);
				return UnityEngine.Input.GetButton(key);
			}
		}
		
		public bool X {
			get {
				string key = GetFullKey(XKey);
				return UnityEngine.Input.GetButton(key);
			}
		}
		
		public bool Y {
			get {
				string key = GetFullKey(YKey);
				return UnityEngine.Input.GetButton(key);
			}
		}
		
		public bool Up {
			get {
				string key = GetFullKey(DpadVerticalKey);
				return UnityEngine.Input.GetAxis(key) > 0.5f;
			}
		}
		
		public bool Down {
			get {
				string key = GetFullKey(DpadVerticalKey);
				return UnityEngine.Input.GetAxis(key) < -0.5f;
			}
		}
		
		public bool Left {
			get {
				string key = GetFullKey(DpadHorizontalKey);
				return UnityEngine.Input.GetAxis(key) < -0.5f;
			}
		}
		
		public bool Right {
			get {
				string key = GetFullKey(DpadHorizontalKey);
				return UnityEngine.Input.GetAxis(key) > 0.5f;
			}
		}
		
		public bool Back {
			get {
				string key = GetFullKey(BackKey);
				return UnityEngine.Input.GetButton(key);
			}
		}
		
		public bool Start {
			get {
				string key = GetFullKey(StartKey);
				return UnityEngine.Input.GetButton(key);
			}
		}
		
		public bool LeftBumper {
			get {
				string key = GetFullKey(LeftBumperKey);
				return UnityEngine.Input.GetButton(key);
			}
		}
		
		public bool RightBumper {
			get {
				string key = GetFullKey(RightBumperKey);
				return UnityEngine.Input.GetButton(key);
			}
		}
		
		public bool LeftThumbstick {
			get {
				string key = GetFullKey(LeftThumbstickKey);
				return UnityEngine.Input.GetButton(key);
			}
		}
		
		public bool RightThumbstick {
			get {
				string key = GetFullKey(RightThumbstickKey);
				return UnityEngine.Input.GetButton(key);
			}
		}
		
		public float LeftTrigger {
			get {
				string key = GetFullKey(LeftTriggerKey);

				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (UnityEngine.Input.GetAxis(key) != 0.0f) {
					_leftTriggerTouched = true;
				}
				
				return _leftTriggerTouched ? ((UnityEngine.Input.GetAxis(key) + 1.0f) * 0.5f) : 0.0f;
				#else
				return UnityEngine.Input.GetAxis(key);
				#endif
			}
		}
		
		public float RightTrigger {
			get {
				string key = GetFullKey(RightTriggerKey);
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (UnityEngine.Input.GetAxis(key) != 0.0f) {
					_rightTriggerTouched = true;
				}

				return _rightTriggerTouched ? ((UnityEngine.Input.GetAxis(key) + 1.0f) * 0.5f) : 0.0f;
				#else
				return UnityEngine.Input.GetAxis(key);
				#endif
			}
		}

		public Vector2 LeftAxis {
			get {
				string xKey = GetFullKey(LeftXKey);
				string yKey = GetFullKey(LeftYKey);
				_leftAxis.Set(UnityEngine.Input.GetAxis(xKey), UnityEngine.Input.GetAxis(yKey));
				return _leftAxis;
			}
		}

		public Vector2 RightAxis {
			get {
				string xKey = GetFullKey(RightXKey);
				string yKey = GetFullKey(RightYKey);
				_rightAxis.Set(UnityEngine.Input.GetAxis(xKey), UnityEngine.Input.GetAxis(yKey));
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

		private string GetFullKey (string root) {
			#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
			string platform = "mac";
			#else
			string platform = "uni";
			#endif

			return $"{_channel}_{platform}_{root}";
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
