/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TRUEStudios.Foundation.Events;
using TRUEStudios.Foundation.Tweens;

namespace TRUEStudios.Foundation.UI {
	class Transition : MonoBehaviour {
		#region Fields
		[SerializeField]
		private string _sceneName;
		[SerializeField]
		private Tween _transitionTween;
		[SerializeField]
		private UnityEvent _onLoadStart = new UnityEvent();
		[SerializeField]
		private UnityEvent _onLoadFinished = new UnityEvent();
		[SerializeField]
		private FloatEvent _onProgress = new FloatEvent();
		#endregion

		#region Properties
		[SerializeField]
		public Tween TransitionTween { get { return _transitionTween; } }
		public UnityEvent OnLoadStart { get { return _onLoadStart; } }
		public UnityEvent OnLoadFinished { get { return _onLoadFinished; } }
		public FloatEvent OnProgress { get { return _onProgress; } }

		public string SceneName {
			set { _sceneName = value; }
			get { return _sceneName; }
		}
		#endregion

		#region Methods
		protected virtual IEnumerator Start () {
			// transition in
			if (_transitionTween != null) {
				yield return _transitionTween.Play();
			}

			// load the scene, then transition out
			yield return StartCoroutine(ProcessLoadScene(_sceneName));
			if (_transitionTween != null) {
				yield return _transitionTween.Play(false, true);
			}

			Destroy(gameObject);
		}

		private IEnumerator ProcessLoadScene(string name) {
			// load the scene, and wait for it to finish
			AsyncOperation op = SceneManager.LoadSceneAsync(name);
			_onLoadStart.Invoke();

			// wait for it to finish
			while (!op.isDone) {
				_onProgress.Invoke(op.progress);
				yield return null;
			}

			_onLoadFinished.Invoke();
			yield return null;
		}
		#endregion
	}
}
