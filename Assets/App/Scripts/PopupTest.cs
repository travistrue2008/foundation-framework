using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TRUEStudios.Core;
using TRUEStudios.UI;

namespace TRUEStudios.State.Tests {
	public class PopupTest : MonoBehaviour {
		#region Fields
		[SerializeField]
		private Popup _testPopup;
		[SerializeField]
		private PopupStack _popupStack;
		#endregion

		#region Methods
		private void Start () {
			RunInstantTest(10);
			RunFrameDelayTest(10);
			RunFullDelayTest(10);
		}
		#endregion

		#region Test Methods
		public void RunInstantTest (int numPopups) {
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

		public void RunFrameDelayTest (int numPopups) {
			// make sure there are popups
			if (numPopups > 0) {
				StartCoroutine(ProcessFrameDelayTest(numPopups));
			} else {
				throw new Exception("numPopups must be at least 1.");
			}
		}

		public void RunFullDelayTest (int numPopups) {
			// make sure there are popups
			if (numPopups > 0) {
				StartCoroutine(ProcessFullDelayTest(numPopups));
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
		#endregion
	}
}
