/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Foundation.Framework
{
	public class PopupService : PrefabFactoryService<Popup>
	{
		#region Fields
		[SerializeField]
		private Blocker _mainBlocker;
		[SerializeField]
		private Blocker _popupBlocker;
		[SerializeField]
		private Transform _popupSpawn;

		private Coroutine _transitionRoutine;
		private List<Popup> _popupQueue = new List<Popup>();
		#endregion


		#region Properties
		public int QueueSize { get { return _popupQueue.Count; } }
		public Popup FirstPopup { get { return (QueueSize > 0) ? _popupQueue[0] : null; } }
		public Popup LastPopup { get { return (QueueSize > 0) ? _popupQueue[QueueSize - 1] : null; } }
		#endregion


		#region Load/Release Popup Profiles
		protected override void OnInitialize()
		{
			// make sure Inspector references are set
			if (_mainBlocker == null || _popupBlocker == null || _popupSpawn == null)
				throw new Exception("Check the Inspector for missing references.");
		}
		#endregion


		#region Queue Controls
		protected override void OnPushInstance(Popup popup)
		{
			RectTransform rectTransform = (RectTransform)popup.transform;
			rectTransform.position = Vector3.zero;
			rectTransform.localScale = Vector3.one;
			rectTransform.offsetMin = rectTransform.offsetMax = Vector2.zero;

			// reset the popup to the beginning of the transition tween
			if (popup.TransitionTween != null)
				popup.TransitionTween.ResetToBegin();

			// check if there is more than 1 popup already in the queue
			if (QueueSize > 1)
			{
				// instantly hide the 2nd-to-last popup
				Popup previousPopup = _popupQueue[QueueSize - 2];
				if (previousPopup.TransitionTween != null)
					previousPopup.TransitionTween.ResetToBegin();
				previousPopup.gameObject.SetActive(false);
			}
			_popupQueue.Add(popup); // push the newest popup into the queue

			// stop the current transition if it's active
			if (_transitionRoutine != null)
			{
				StopCoroutine(_transitionRoutine);
				_transitionRoutine = null;
				--_popupBlocker.Count;
			}
			_transitionRoutine = StartCoroutine(ProcessPush(popup)); // begin displaying the latest popup, and push it onto the stack
		}

		public T PushPopup<T>(string prefabName, bool explicitType = true)
			where T : Popup
		{
			return PushInstance<T>(prefabName, _popupSpawn, explicitType);
		}

		public T PushPopup<T>(GameObject prefab, bool explicitType = true)
			where T : Popup
		{
			return PushInstance<T>(prefab, _popupSpawn, explicitType);
		}

		public void PopPopup()
		{
			// don't process if there's no popups in the queue
			if (QueueSize == 0)
				return;

			// stop the current transition if it's active
			if (_transitionRoutine != null)
			{
				StopCoroutine(_transitionRoutine);
				_transitionRoutine = null;
			}
			_transitionRoutine = StartCoroutine(ProcessPop());
		}

		public void ClearQueue()
		{
			// destroy all popup GameObjects
			foreach (Popup popup in _popupQueue)
				Destroy(popup.gameObject);
			_popupQueue.Clear(); // release all dangling Popup references
		}
		#endregion


		#region Coroutines
		private IEnumerator ProcessPush(Popup popup)
		{
			//++_mainBlocker.Count;
			++_popupBlocker.Count;

			// check if there are currently any popups
			if (QueueSize > 1)
			{
				// hide the current popup
				Popup previousPopup = _popupQueue[QueueSize - 2];
				if (previousPopup.TransitionTween != null)
				{
					yield return previousPopup.TransitionTween.Play(false);
					previousPopup.gameObject.SetActive(false);
				}
			}

			// display the current popup
			if (LastPopup.TransitionTween != null)
				yield return LastPopup.TransitionTween.Play();

			--_popupBlocker.Count;
			_transitionRoutine = null;
		}

		private IEnumerator ProcessPop()
		{
			Popup currentPopup = LastPopup;
			++_popupBlocker.Count;

			// hide the current popup
			if (currentPopup.TransitionTween != null)
				yield return currentPopup.TransitionTween.Play(false);

			// remove the popup reference, and destroy the popup
			_popupQueue.Remove(LastPopup);
			Destroy(currentPopup.gameObject);

			// check if there are any popups left in the queue
			if (QueueSize > 0)
			{
				// activate, and transition the popup
				LastPopup.gameObject.SetActive(true);
				if (LastPopup.TransitionTween != null)
					yield return LastPopup.TransitionTween.Play();
			} //else
			  //--_mainBlocker.Count;
			--_popupBlocker.Count;
			_transitionRoutine = null;
		}
		#endregion


		#region Getters
		public Popup GetActivePopup()
		{
			return (QueueSize > 0) ? _popupQueue[QueueSize - 1].GetComponent<Popup>() : null;
		}

		public T GetActivePopup<T>()
			where T : Popup
		{
			return (QueueSize > 0) ? _popupQueue[QueueSize - 1].GetComponent<T>() : null;
		}
		#endregion
	}
}
