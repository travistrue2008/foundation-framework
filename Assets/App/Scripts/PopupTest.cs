using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TRUEStudios.State;

namespace TRUEStudios.State.Tests {
	public class PopupTest : MonoBehaviour {
		#region Fields
		[SerializeField]
		private Popup _testPopup;
		#endregion

		#region Methods
		private void Awake () {
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
				Services.Get<PopupService>().PushPopup<Popup>(_testPopup);
			}

			// instantly destroy popups
			for (int i = 0; i < numPopups; ++i) {
				Services.Get<PopupService>().PopPopup();
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
				Services.Get<PopupService>().PushPopup<Popup>(_testPopup);
				yield return null;
			}

			// destroy popups
			while (Services.Get<PopupService>().CurrentPopup != null) {
				Services.Get<PopupService>().PopPopup();
				yield return null;
			}
		}

		private IEnumerator ProcessFullDelayTest (int numPopups) {
			// create popups
			for (int i = 0; i < numPopups; ++i) {
				Services.Get<PopupService>().PushPopup<Popup>(_testPopup);
				while (Services.Get<PopupService>().IsTransitioning) {
					yield return null;
				}
			}

			// destroy popups
			while (Services.Get<PopupService>().CurrentPopup != null) {
				Services.Get<PopupService>().PopPopup();
				while (Services.Get<PopupService>().IsTransitioning) {
					yield return null;
				}
			}
		}
		#endregion
	}
}
