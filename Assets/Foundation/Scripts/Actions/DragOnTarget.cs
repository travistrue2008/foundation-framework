/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;


namespace Foundation.Framework
{
	[RequireComponent(typeof(PositionTween))]
	public class DragOnTarget : DragAction
	{
		#region Fields
		[SerializeField]
		private AudioClip _targetEnterClip;
		[SerializeField]
		private AudioClip _targetLeaveClip;
		[SerializeField]
		private AudioClip _targetClip;
		[SerializeField]
		private AudioClip _releaseClip;
		[SerializeField]
		private Collider2D _targetCollider;
		[SerializeField]
		private UnityEvent _onDragEnter = new UnityEvent();
		[SerializeField]
		private UnityEvent _onDragLeave = new UnityEvent();
		[SerializeField]
		private UnityEvent _onDragInTarget = new UnityEvent();
		[SerializeField]
		private UnityEvent _onDragInTargetStarted = new UnityEvent();
		[SerializeField]
		private UnityEvent _onDragInTargetEnded = new UnityEvent();

		private bool _intersected = false;
		#endregion


		#region Methods
		protected override void OnDragBeganMoving()
		{
			if(PerformBoundsCheck())
				_onDragInTargetStarted.Invoke();
		}

		protected override void OnDragStoppedMoving()
		{
			if(PerformBoundsCheck())
				_onDragInTargetEnded.Invoke();
		}

		protected override void OnDrag()
		{
			// check if in bounds
			if(PerformBoundsCheck())
				_onDragInTarget.Invoke();
		}

		protected override void OnRelease()
		{
			if(_intersected)
			{
				Services.Get<AudioService>().PlaySound(_releaseClip);
				_onDragLeave.Invoke();

				if(IsMoving)
					_onDragInTargetEnded.Invoke();
			}
			_intersected = false;
		}

		private bool PerformBoundsCheck()
		{
			bool status = false;

			// check if the current target is valid, and check if the colliders intersect
			if(_targetCollider != null && _targetCollider.bounds.Intersects(_collider.bounds))
			{
				// check if just entering the drag target
				if(!_intersected)
				{
					Services.Get<AudioService>().PlaySound(_targetEnterClip);
					_onDragEnter.Invoke();

					if(IsMoving)
						_onDragInTargetStarted.Invoke();
				}

				_intersected = true;
				status = true;
			} else {
				// check if leaving
				if(_intersected)
				{
					Services.Get<AudioService>().PlaySound(_targetLeaveClip);
					_onDragInTargetEnded.Invoke();
					_onDragLeave.Invoke();
				}
				_intersected = false;
			}
			return status;
		}
		#endregion
	}
}
