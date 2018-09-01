/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TRUEStudios.Foundation.Core {
	public class SceneAction : MonoBehaviour {
		#region Fields
		[SerializeField]
		private string _sceneName;
		#endregion

		#region Methods
		public void GotoScene() {
			GotoScene(_sceneName);
		}

		public void GotoScene (string sceneName) {
			SceneManager.LoadScene(_sceneName);
		}
		#endregion
	}
}
