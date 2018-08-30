/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TRUEStudios.Tweens;

namespace TRUEStudios.Actions {
	[RequireComponent(typeof(PositionTween))]
	public class DragAction : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
		#region Constants
		private const float NoMoveThreshold = 0.05f;
		#endregion

		#region Fields
		[SerializeField]
		private bool _returnOnRelease;
		[SerializeField]
		private UnityEvent _onMove = new UnityEvent();
		[SerializeField]
		private UnityEvent _onRelease = new UnityEvent();
		[SerializeField]
		private UnityEvent _onBeganMoving = new UnityEvent();
		[SerializeField]
		private UnityEvent _onStoppedMoving = new UnityEvent();

		protected Collider2D _collider;

		private bool _lastFrameMoved = false;
		private bool _moved = false;
		private float _noMoveTime = NoMoveThreshold;
		private Vector3 _lastPosition;
		private Vector3 _currentPosition;
		private Vector3 _grabOffset;
		private Vector3 _homePosition;
		private PositionTween _homeTween;
		#endregion

		#region Properties
		public bool IsMoving { get { return _moved; } }
		public Vector3 HomePosition { get { return _homePosition; } }
		public Vector3 CurrentPosition { get { return _currentPosition; } }
		public Vector3 GrabOffset { get { return _grabOffset; } }
		private UnityEvent OnMove { get { return _onMove; } }
		private UnityEvent OnRelease { get { return _onRelease; } }
		private UnityEvent OnBeganMoving { get { return _onBeganMoving; } }
		private UnityEvent OnStoppedMoving { get { return _onStoppedMoving; } }
		#endregion

		#region Methods
		protected virtual void Awake () {
			// set the initial position, and set the collider
			_homePosition = transform.localPosition;
			_collider = GetComponent<Collider2D>();
			if (_collider == null) {
				throw new Exception("No Collider2D component attached to GameObject.");
			}

			// setup the return tween
			_homeTween = GetComponent<PositionTween>();
			_homeTween.End = _homePosition;
			_lastPosition = transform.position;
		}

		protected virtual void FixedUpdate () {
			// make sure a change has occurred
			if (_lastPosition != _currentPosition) {
				_noMoveTime = 0.0f;
				_lastFrameMoved = _moved;
				_moved = true;

				// signal movement started
				if (!_lastFrameMoved) {
					_onBeganMoving.Invoke();
				}
			} else {
				// increment the no-moving time
				if (_noMoveTime < NoMoveThreshold) {
					_lastFrameMoved = _moved;
					_noMoveTime = Mathf.Clamp(_noMoveTime + Time.deltaTime, 0.0f, NoMoveThreshold);
					if (_noMoveTime == NoMoveThreshold) {
						_onStoppedMoving.Invoke();
						_moved = false;
					} else {
						_moved = true;
					}
				}
			}

			_lastPosition = _currentPosition;
		}

		public void OnBeginDrag (PointerEventData eventData) {
			// only process if the collider is still active, and not pressing on a UI element
			if (_collider.enabled) {
				_grabOffset = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition) - transform.position; // get the grab's offset
			}
		}

		public void OnEndDrag (PointerEventData eventData) {
			// only process if the collider is still active
			if (_collider.enabled) {
				// get the current position on the screen, and return if on release
				transform.position = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition) - _grabOffset;
				if (_returnOnRelease) {
					ResetPosition(false);
				}

				_onRelease.Invoke();
			}
		}
		
		public void OnDrag (PointerEventData eventData) {
			// only process if the collider is still active
			if (_collider.enabled) {
				// get last and current positions
				_lastPosition = _currentPosition;
				_currentPosition = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition) - _grabOffset;
				transform.position = _currentPosition;
				_onMove.Invoke();
			}
		}
		
		public void ResetPosition (bool instant) {
			// check if move back over time
			if (!instant) {
				// return back to position using a tween
				if (_homeTween != null) {
					_homeTween.Begin = transform.localPosition;
					_homeTween.Play(true, true);
				}
			} else {
				transform.position = _homePosition;
			}
		}
		#endregion
	}
}
