/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
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

		public PopupStack Stack {
			get {
				// cache the stack if not already (necessary since Popups are meant to be instantiated)
				if (_stack == null) {
					_stack = transform.parent.parent.GetComponent<PopupStack>();
				}

				return _stack;
			}
		}
		#endregion

		#region Setup
		protected virtual void OnDestroy () {
			// signal the closed event
			if (_onClose != null) {
				_onClose.Invoke();
			}
		}

		public void Dismiss (bool clear = false) {
			// only pop the last popup off the stack if it's this particular popup
			if (Stack.CurrentPopup == this) {
				Stack.Pop(clear); // clear the stack if necessary
			} else {
				Debug.LogWarning("This popup isn't the active popup");
			}
		}
		#endregion
	}
}
