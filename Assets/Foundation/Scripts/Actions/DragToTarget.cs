/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Foundation.Framework
{
	[RequireComponent(typeof(PositionTween))]
	public class DragToTarget : DragAction
	{
		#region Fields
		[SerializeField]
		private bool _triggerOnEnter;
		[SerializeField]
		private bool _checkOnUpdate;
		[SerializeField]
		private bool _lockPositionOnFinish;
		[SerializeField]
		private bool _hideTargetOnTrigger;
		[SerializeField]
		private AudioClip _targetClip;
		[SerializeField]
		private Collider2D[] _targets;
		[SerializeField]
		private UnityEvent _onTrigger = new UnityEvent();
		[SerializeField]
		private UnityEvent _onFinish = new UnityEvent();

		private bool _dragging;
		private int _targetIndex = 0;
		#endregion


		#region Methods
		protected override void OnRelease()
		{
			PerformBoundsCheck();
		}

		protected override void OnDrag()
		{
			// weird syntax, I know... just meant to convey value-scope
			if(_triggerOnEnter)
				PerformBoundsCheck();
		}

		private void Update()
		{
			if(_checkOnUpdate)
				PerformBoundsCheck();
		}

		private void PerformBoundsCheck()
		{
			// check if the current target is valid
			if(_targets != null && _targetIndex < _targets.Length && _targets[_targetIndex] != null)
			{
				// check if the colliders intersect
				if(_targets[_targetIndex].bounds.Intersects(_collider.bounds))
				{
					// hide the target GameObject if flagged
					if(_hideTargetOnTrigger)
						_targets[_targetIndex].gameObject.SetActive(false);

					// trigger response
					DragToResponder responder = _targets[_targetIndex].GetComponent<DragToResponder>();
					if(responder != null)
						responder.PerformTrigger();

					// increment _targetIndex and signal event
					Services.Get<AudioService>().PlaySound(_targetClip);
					_onTrigger.Invoke();
					++_targetIndex;

					// check if the end of the list has been reached
					if(_targetIndex == _targets.Length)
					{
						// snap the transform to the ending 
						if(_lockPositionOnFinish)
							transform.position = _targets[_targetIndex-1].transform.position;

						_collider.enabled = false;
						_onFinish.Invoke();
					}
				}
			}
		}

		public void Reset()
		{
			_targetIndex = 0;
			_collider.enabled = true;
		}
		#endregion
	}
}
