/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;

namespace TRUEStudios.Core {
	public class SpawnPrefab : MonoBehaviour {
		#region Fields
		public GameObject defaultPrefab;
		#endregion

		#region Methods
		public void Spawn () {
			Spawn(defaultPrefab);
		}

		public void Spawn (string _prefabName) {
			// attempt to load the prefab, and instantiate it
			var prefab = (GameObject)Resources.Load(_prefabName);
			if (prefab != null) {
				Spawn(prefab);
			}
		}

		public void Spawn (GameObject prefab) {
			// check if the prefab isn't valid
			if (prefab == null) {
				throw new Exception("No prefab reference set");
			}

			// attempt to instantiate the prefab as a GameObject
			var obj = (GameObject)Instantiate(prefab, transform.position, transform.rotation, transform);
			if (obj != null) {
				obj.transform.localScale = Vector3.one;
			} else {
				throw new Exception("Unable to spawn prefab as a GameObject: " + prefab.name);
			}
		}
		#endregion
	}
}
