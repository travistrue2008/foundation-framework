/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Foundation.Framework
{
	[RequireComponent(typeof(PositionTween))]
	public class DragAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		#region Constants
		private const float NoMoveThreshold = 0.05f;
		#endregion


		#region Fields
		[SerializeField]
		private bool _returnOnRelease;
		[SerializeField]
		private AudioClip _dragClip;
		[SerializeField]
		private UnityEvent _onDragMove = new UnityEvent();

		protected Collider2D _collider;

		private bool _lastFrameMoved = false;
		private bool _moved = false;
		private float _noMoveTime = NoMoveThreshold;
		private Vector3 _lastPos;
		private Vector3 _currentPos;
		private Vector3 _grabOffset;
		private Vector3 _initialPosition;
		private AudioSource _dragSource;
		private PositionTween _returnTween;
		#endregion


		#region Properties
		public bool IsMoving { get { return _moved; } }
		#endregion
		
		
		#region Virtual Methods
		protected virtual void OnRelease() { }
		protected virtual void OnDragBeganMoving() { }
		protected virtual void OnDragStoppedMoving() { }
		protected virtual void OnDrag() { }
		#endregion


		#region Methods
		protected virtual void Awake()
		{
			// set the initial position, and set the collider
			_initialPosition = transform.localPosition;
			_collider = GetComponent<Collider2D>();
			if(_collider == null)
				throw new Exception("No Collider2D component attached to GameObject.");
			
			// setup the return tween
			_returnTween = GetComponent<PositionTween>();
			_returnTween.End = _initialPosition;
			_lastPos = transform.position;
		}

		protected virtual void OnDestroy()
		{
			StopSound();
		}

		protected virtual void OnDisable()
		{
			StopSound();
		}

		protected virtual void FixedUpdate()
		{
			if(_lastPos != _currentPos)
			{
				_noMoveTime = 0.0f;
				_lastFrameMoved = _moved;
				_moved = true;

				// signal movement started
				if(!_lastFrameMoved)
					OnDragBeganMoving();
			} else {
				// increment the no-moving time
				if(_noMoveTime < NoMoveThreshold)
				{
					_lastFrameMoved = _moved;
					_noMoveTime = Mathf.Clamp(_noMoveTime + Time.deltaTime, 0.0f, NoMoveThreshold);
					if(_noMoveTime == NoMoveThreshold)
					{
						OnDragStoppedMoving();
						_moved = false;
					} else
						_moved = true;
				}
			}
			_lastPos = _currentPos;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			// only process if the collider is still active, and not pressing on a UI element
			if(_collider.enabled)
			{
				_grabOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; // get the grab's offset
				_dragSource = Services.Get<AudioService>().PlaySound(_dragClip, true);
			}
		}
		
		public void OnPointerUp(PointerEventData eventData)
		{
			StopSound();

			// only process if the collider is still active
			if(_collider.enabled)
			{
				// get the current position on the screen, and return if on release
				transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _grabOffset;
				if(_returnOnRelease && _collider.enabled)
					ResetPosition(false);
				OnRelease();
			}
		}
		
		public void OnDrag(PointerEventData eventData)
		{
			// only process if the collider is still active
			if(_collider.enabled)
			{
				// get last and current positions
				_lastPos = _currentPos;
				_currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _grabOffset;
				transform.position = _currentPos;

				OnDrag();
				_onDragMove.Invoke();
			}
		}
		
		public void ResetPosition(bool instant)
		{
			// instantly move back
			if(instant)
				transform.position = _initialPosition;
			else {
				// return back to position using a tween
				if(_returnTween != null && _returnTween.State == TweenPlaybackState.Stopped)
				{
					_returnTween.Begin = transform.localPosition;
					_returnTween.Play(true, true);
				}
			}
		}

		private void StopSound()
		{
			// stop any drag sources
			if(_dragSource != null)
			{
				_dragSource.Stop();
				_dragSource = null;
			}
		}
		#endregion
	}
}
