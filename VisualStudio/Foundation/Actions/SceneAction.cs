/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

namespace TRUEStudios.Foundation.Actions {
	public class SceneAction : MonoBehaviour {
		#region Fields
		[SerializeField]
		private string _sceneName;
		#endregion

		#region Methods
		public void GotoScene () {
			SceneManager.LoadScene(_sceneName);
		}
		#endregion
	}
}
