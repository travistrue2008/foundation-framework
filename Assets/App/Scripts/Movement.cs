using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TRUEStudios.Foundation.Input;

public class Movement : MonoBehaviour {
	#region Fields
	[SerializeField]
	public float _speed = 10.0f;
	[SerializeField]
	private Gamepad _gamepad;
	[SerializeField]
	private UnityEvent _onCollide = new UnityEvent();

	private Vector3 _velocity = Vector3.zero;
	private Rigidbody _rigidBody;
	#endregion

	#region Properties
	public UnityEvent OnCollide { get { return _onCollide; } }
	#endregion

	#region MonoBehaviour Hooks
	private void Awake () {
		_rigidBody = GetComponent<Rigidbody>();
	}

	private void Update () {
		_velocity.Set(_gamepad.LeftAxis.x * _speed, 0.0f, _gamepad.LeftAxis.y * _speed);
		_rigidBody.velocity = _velocity;
	}

	private void OnCollisionEnter (Collision other) {
		_onCollide.Invoke();
	}
	#endregion
}
