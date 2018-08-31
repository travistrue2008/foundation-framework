using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TRUEStudios.Foundation.Core;
using TRUEStudios.Foundation.UI;

namespace TRUEStudios.State.Tests {
	public class PopupTest : MonoBehaviour {
		#region Fields
		[SerializeField]
		private Popup _testPopup;
		[SerializeField]
		private PopupStack _popupStack;
		#endregion

		#region Methods
		private IEnumerator Start () {
			RunInstantTest(10);
			yield return RunFrameDelayTest(10);
			yield return RunFullDelayTest(10);
			yield return RunPopClearTest(10);
		}
		#endregion

		#region Test Methods
		private void RunInstantTest (int numPopups) {
			// make sure there are popups
			if (numPopups < 1) {
				throw new Exception("numPopups must be at least 1.");
			}

			// instantly create popups
			for (int i = 0; i < numPopups; ++i) {
				_popupStack.Push<Popup>(_testPopup);
			}

			// instantly destroy popups
			for (int i = 0; i < numPopups; ++i) {
				_popupStack.Pop();
			}
		}

		private Coroutine RunFrameDelayTest (int numPopups) {
			// make sure there are popups
			if (numPopups > 0) {
				return StartCoroutine(ProcessFrameDelayTest(numPopups));
			} else {
				throw new Exception("numPopups must be at least 1.");
			}
		}

		private Coroutine RunFullDelayTest (int numPopups) {
			// make sure there are popups
			if (numPopups > 0) {
				return StartCoroutine(ProcessFullDelayTest(numPopups));
			} else {
				throw new Exception("numPopups must be at least 1.");
			}
		}

		private Coroutine RunPopClearTest (int numPopups) {
			// make sure there are popups
			if (numPopups > 0) {
				return StartCoroutine(ProcessPopClearTest(numPopups));
			} else {
				throw new Exception("numPopups must be at least 1.");
			}
		}
		#endregion

		#region Coroutines
		private IEnumerator ProcessFrameDelayTest (int numPopups) {
			// create popups
			for (int i = 0; i < numPopups; ++i) {
				_popupStack.Push<Popup>(_testPopup);
				yield return null;
			}

			// destroy popups
			while (_popupStack.CurrentPopup != null) {
				_popupStack.Pop();
				yield return null;
			}
		}

		private IEnumerator ProcessFullDelayTest (int numPopups) {
			// create popups
			for (int i = 0; i < numPopups; ++i) {
				_popupStack.Push<Popup>(_testPopup);
				while (_popupStack.IsTransitioning) {
					yield return null;
				}
			}

			// destroy popups
			while (_popupStack.CurrentPopup != null) {
				_popupStack.Pop();
				while (_popupStack.IsTransitioning) {
					yield return null;
				}
			}
		}

		private IEnumerator ProcessPopClearTest (int numPopups) {
			// create popups
			for (int i = 0; i < numPopups; ++i) {
				_popupStack.Push<Popup>(_testPopup);
				while (_popupStack.IsTransitioning) {
					yield return null;
				}
			}

			// pop the current popup, and clear the stack
			_popupStack.CurrentPopup.Dismiss(true);
		}
		#endregion
	}
}
