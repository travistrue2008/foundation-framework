﻿/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using TRUEStudios.Foundation.Core;

namespace TRUEStudios.Foundation.Core {
	public class PrefabProfile {
		public Type ComponentType { private set; get; }
		public GameObject Prefab { private set; get; }

		public PrefabProfile(Type componentType, GameObject prefab) {
			ComponentType = componentType;
			Prefab = prefab;
		}
	}

	public class PrefabFactory<TBase> : MonoBehaviour where TBase : MonoBehaviour {
		#region Fields
		[SerializeField]
		private string _prefabDirectory;

		private Dictionary<string, PrefabProfile> _profiles = new Dictionary<string, PrefabProfile>();
		#endregion

		#region Virtual Methods
		protected virtual void OnInstantiate (TBase component) { }
		#endregion

		#region Load/Release prefab profiles
		public T LoadPrefab<T> (Enum prefabIdentifier) where T : TBase {
			return LoadPrefab<T>(prefabIdentifier.ToString());
		}

		public T LoadPrefab<T> (string prefabName) where T : TBase {
			// check if the profile already exists
			PrefabProfile profile = null;
			if (_profiles.TryGetValue(prefabName, out profile)) {
				return profile.Prefab.GetComponent<T>();
			}

			// attempt to load the prefab
			GameObject prefab = (GameObject)Resources.Load("Prefabs/" + _prefabDirectory + "/" + prefabName);
			if (prefab != null) {
				// check if the GameObject has the TBase, base-type component of specified type attached to it
				T component = prefab.GetComponent<T>();
				if (component != null) {
					profile = new PrefabProfile(typeof(T), prefab);
					_profiles[prefabName] = profile;
					Debug.Log("Loaded prefab of type: " + component.GetType().ToString());
					return component;
				} else {
					throw new Exception("Couldn't find " + typeof(TBase).ToString() + " component of explicit type '" + typeof(T).ToString() + "' attached to '" + prefabName + "'.");
				}
			}

			// attempted to load a prefab that doesn't exist
			throw new Exception("Unable to load prefab: " + prefabName);
		}
		
		public void ReleasePrefab (string prefabName) {
			PrefabProfile profile = null;
			if (_profiles.TryGetValue(prefabName, out profile)) {
				_profiles.Remove(prefabName);
			}
		}

		protected T Instantiate<T> (string prefabName, Transform spawnTransform) where T : TBase {
			PrefabProfile profile = null;

			// attempt to get the prefab profile
			if (!_profiles.TryGetValue(prefabName, out profile)) {
				// load the prefab, and attempt again
				LoadPrefab<T>(prefabName);
				if (!_profiles.TryGetValue(prefabName, out profile)) {
					// report the error, and return
					Debug.LogError("Trying to instantiate " + typeof(T).ToString() + " with a prefab that doesn't exist: " + prefabName);
					return null;
				}
			}

			return Instantiate<T>(profile.Prefab, spawnTransform);
		}

		protected T Instantiate<T> (GameObject prefab, Transform spawnTransform) where T : TBase {
			T component = null;

			// make sure a prefab is available
			if (prefab == null) {
				return null;
			}

			// instantiate the prefab
			GameObject obj = (GameObject)Instantiate(prefab);
			if (obj != null) {
				// get the component
				component = obj.GetComponent<T>();
				if (component != null) {
					// setup the component's transform
					obj.transform.SetParent(spawnTransform);
					OnInstantiate(component);
				} else {
					Destroy(obj);
					throw new Exception("No popup of type '" + typeof(T).ToString() + "' attached.");
				}
			} else {
				throw new Exception("Unable to load popup prefab.");
			}

			return component;
		}

		public PrefabProfile GetPrefabProfileByName<T> (string prefabName) where T : TBase {
			// check if the profile already exists
			PrefabProfile profile = null;
			_profiles.TryGetValue(prefabName, out profile);
			return profile;
		}
		#endregion
	}
}
