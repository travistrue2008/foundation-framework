/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;
using UnityEngine.Events;
using TRUEStudios.Foundation.Core;
using TRUEStudios.Foundation.Tweens;

namespace TRUEStudios.Foundation.UI {
	public class Popup : MonoBehaviour {
		#region Fields
		[SerializeField]
		private Tween _transitionTween;
		[SerializeField]
		private GameObject _firstResponder;
		[SerializeField]
		private UnityEvent _onClose = new UnityEvent();

		private PopupStack _stack;
		#endregion

		#region Properties
		public Tween TransitionTween { get { return _transitionTween; } }
		public GameObject FirstResponder { get { return _firstResponder; } }
		public UnityEvent OnClose { get { return _onClose; } }
		#endregion

		#region Setup
		protected virtual void Start () {
			// get the parent PopupStack
			_stack = transform.parent.parent.GetComponent<PopupStack>();
			if (_stack == null) {
				Debug.LogWarning("Popup was spawned, but not managed by the PopupStack");
			}
		}

		protected virtual void OnDestroy () {
			// signal the closed event
			if (_onClose != null) {
				_onClose.Invoke();
			}
		}

		public void Dismiss (bool clear = false) {
			// only pop the last popup off the stack if it's this particular popup
			if (_stack.CurrentPopup == this) {
				_stack.Pop(clear); // clear the stack if necessary
			} else {
				Debug.LogWarning("This popup isn't the active popup");
			}
		}
		#endregion
	}
}
