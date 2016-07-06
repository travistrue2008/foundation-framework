/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Foundation.Framework
{
	public interface ISceneController
	{
		string BackSceneName { get; }
		bool PopFromStack();
	}


	[Serializable]
	public class SceneServiceEvent : UnityEvent<string> { }


	public class SceneService : PrefabFactoryService<Transition>
	{
		#region Fields
		[SerializeField, Min(0.1f)]
		private float _defaultDuration = 1.0f;
		[SerializeField, Min(0.0f)]
		private float _defaultHoldDuration = 1.0f;
		[SerializeField]
		private Blocker _blocker;
		[SerializeField]
		private Transition _defaultTransitionPrefab;
		[SerializeField]
		private Transform _transitionSpawn;
		[SerializeField]
		private SceneServiceEvent _onTransitionStarted = new SceneServiceEvent();
		[SerializeField]
		private SceneServiceEvent _onSceneWillChange = new SceneServiceEvent();
		[SerializeField]
		private SceneServiceEvent _onSceneDidChange = new SceneServiceEvent();
		[SerializeField]
		private UnityEvent _onTransitionEnded = new UnityEvent();

		private Transition _transition;
		private ISceneController _sceneController;
		#endregion


		#region Properties
		public string ZoneTag { set; get; }
		public string LastSceneName { private set; get; }

		public bool IsTransitioning { get { return _transition != null; } }
		public Transition CurrentTransition { get { return _transition; } }
		public SceneServiceEvent OnTransitionStarted { get { return _onTransitionStarted; } }
		public SceneServiceEvent OnSceneWillChange { get { return _onSceneWillChange; } }
		public SceneServiceEvent OnSceneDidChange { get { return _onSceneDidChange; } }
		public UnityEvent OnTransitionEnded { get { return _onTransitionEnded; } }

		public ISceneController SceneController
		{
			set { _sceneController = value; }
			get { return _sceneController; }
		}
		#endregion


		#region MonoBehaviour/Overridden Methods
		private void Update()
		{
			// check if the "Escape" button is pressed (this is the Back button for Android)
			if (Input.GetKeyDown(KeyCode.Escape))
				GoBack();
		}

		private IEnumerator ProcessLoadScene(string sceneName, float holdDuration)
		{
			// block all input, and raise started event
			++_blocker.Count;
			_onTransitionStarted.Invoke(sceneName);

			// play the transition tween forward, and wait
			if (_transition != null && _transition.Tween != null)
				yield return _transition.Tween.Play();
			_onSceneWillChange.Invoke(sceneName);
			LastSceneName = Application.loadedLevelName; // cache the last scene's name

			// wait for the transition to finish, then load the level asynchronously
			AsyncOperation loadOperation = Application.LoadLevelAsync(sceneName);
			if (loadOperation != null)
			{
				// wait for the loading operation to complete
				while (!loadOperation.isDone)
				{
					if (_transition != null)
						_transition.UpdateLoadFrame(loadOperation.progress);
					yield return null; // wait for the level to finish loading
				}
				yield return Services.Release(); // attempt to flag unused assets, and release them from GC
			}
			else
				throw new Exception("Unable to load level: " + sceneName);

			// yield for the hold duration
			if (holdDuration > 0.0f)
				yield return new WaitForSeconds(holdDuration);
			_onSceneDidChange.Invoke(LastSceneName); // invoke the event that the scene has changed

			// play the transition tween backwards, and wait
			if (_transition != null && _transition.Tween != null)
				yield return _transition.Tween.Play(false);
			_onTransitionEnded.Invoke(); // invoke ended event

			// destroy the transition, and unblock all input
			if (_transition != null)
			{
				Destroy(_transition.gameObject);
				_transition = null;
			}
			--_blocker.Count;
		}
		#endregion


		#region Actions
		public void GoBack()
		{
			// don't process if performing a transition between scenes
			if (IsTransitioning)
				return;

			// check if there are any popups in the queue
			if (Services.Get<PopupService>() != null && Services.Get<PopupService>().QueueSize > 0)
			{
				// dismiss the current popup from the queue if one isn't already transitioning
				if (Services.Get<PopupService>().LastPopup.TransitionTween.State == TweenPlaybackState.Stopped)
					Services.Get<PopupService>().PopPopup();
				return;
			}

			// check if there is a valid ISceneController set
			if (SceneController != null)
			{
				// attempt to pop from the stack
				if (!SceneController.PopFromStack())
				{
					Transition<Transition>(_defaultTransitionPrefab.gameObject, SceneController.BackSceneName, _defaultDuration, _defaultHoldDuration); // if false, use the default transition
					SceneController = null; // unset the scene, so don't expect persistent GameObjects with ISceneController-derivitive components to still be the target controller after a scene change
				}
			}
		}

		public T Transition<T>(string prefabName, string sceneName, float duration = 1.0f, float holdDuration = 0.25f, bool explicitType = true)
			where T : Transition
		{
			// don't transition if already transitioning
			if (IsTransitioning)
			{
				Debug.LogWarning("Can't transition while already in a transition.");
				return null;
			}

			// instantiate the transition
			T transition = PushInstance<T>(prefabName, _transitionSpawn, explicitType);
			return Transition<T>(transition, sceneName, duration, holdDuration);
		}

		public T Transition<T>(GameObject prefab, string sceneName, float duration = 1.0f, float holdDuration = 0.25f, bool explicitType = true)
			where T : Transition
		{
			// don't transition if already transitioning
			if (IsTransitioning)
			{
				Debug.LogWarning("Can't transition while already in a transition.");
				return null;
			}

			// instantiate the transition
			T transition = PushInstance<T>(prefab, _transitionSpawn, explicitType);
			return Transition<T>(transition, sceneName, duration, holdDuration);
		}

		private T Transition<T>(T transition, string sceneName, float duration = 1.0f, float holdDuration = 0.25f)
			where T : Transition
		{
			// don't process if there is no transition
			if (transition != null)
			{
				// set the transition member, and its duration
				_transition = transition;
				_transition.Tween.Duration = duration;

				// normalize the local location and scale
				_transition.transform.localPosition = Vector3.zero;
				_transition.transform.localScale = Vector3.one;

				// set the transition margins
				RectTransform rectTransform = _transition.GetComponent<RectTransform>();
				if (rectTransform != null)
				{
					rectTransform.offsetMin = Vector2.zero;
					rectTransform.offsetMax = Vector2.one;
				}
			}

			// begin the transition, and return the transition
			StartCoroutine(ProcessLoadScene(sceneName, holdDuration));
			return transition; // return the specifically-typed transition
		}
		#endregion
	}
}
