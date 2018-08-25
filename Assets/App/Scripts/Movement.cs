using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TRUEStudios.Input;

public class Movement : MonoBehaviour {
	#region Constants
	public const float Speed = 0.1f;
	#endregion

	#region Fields
	[SerializeField]
	private Gamepad _gamepad;
	[SerializeField]
	private UnityEvent _onCollide = new UnityEvent();

	private Vector2 _position = Vector3.zero;
	#endregion

	#region Properties
	public UnityEvent OnCollide { get { return _onCollide; } }
	#endregion

	#region MonoBehaviour Hooks
	private void Update () {
		_position.x += (_gamepad.LeftAxis.x * Speed);
		_position.y += (_gamepad.LeftAxis.y * Speed);
		transform.localPosition = _position;
	}

	private void OnCollisionEnter (Collision other) {
		_onCollide.Invoke();
	}
	#endregion
}
