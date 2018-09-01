/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TRUEStudios.Foundation.UI;

namespace TRUEStudios.Foundation.Actions {
	public class SceneAction : MonoBehaviour {
		#region Fields
		[SerializeField]
		private string _sceneName;
		[SerializeField]
		private Transition _transition;
		#endregion

		#region Methods
		public void GotoScene() {
			GotoScene(_sceneName);
		}

		public void GotoScene (string sceneName) {
			// spawn the transition if available
			if (_transition != null) {
				var obj = (GameObject)Instantiate(_transition.gameObject, Vector3.zero, Quaternion.identity);
				var transition = obj.GetComponent<Transition>();
				transition.SceneName = sceneName;
			} else {
				SceneManager.LoadScene(sceneName);
			}
		}
		#endregion
	}
}
