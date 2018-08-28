/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour {
	#region Fields
	[SerializeField]
	private int _numInstances = 20;
	[SerializeField]
	private GameObject _prefab;

	private GameObject[] _instances;
	#endregion

	#region Properties
	public int NumInstances { get { return _instances.Length; } }
	#endregion

	#region MonoBehaviour Hooks
	private void Awake () {
		_instances = new GameObject[_numInstances];
		for (int i = 0; i < _numInstances; ++i) {
			_instances[i] = InstantiatePrefab(_prefab);
		}
	}
	#endregion

	#region Actions
	public GameObject Spawn (Transform target) {
		// iterate through each instance
		for (int i = 0; i < _numInstances; ++i) {
			// look for the first instance that's not disabled
			if (!_instances[i].activeSelf) {
				_instances[i].transform.SetParent(target);
				_instances[i].SetActive(true);
				return _instances[i];
			}
		}

		return null;
	}

	public void Recall (GameObject target) {
		bool found = false;

		// iterate through each instance
		foreach (var obj in _instances) {
			if (obj == target) {
				target.SetActive(false);
				target.transform.SetParent(transform);
				target.transform.localPosition = Vector3.zero;
				target.transform.localScale = Vector3.one;
				found = true;
				break;
			}
		}

		// notify if rogue GameObject
		if (!found) {
			Debug.LogWarning($"Unknown GameObject found: {target}");
		}
	}

	public void Reclaim () {
		// iterate through all references
		foreach (var obj in _instances) {
			if (!obj.activeSelf) {
				Recall(obj);
			}
		}
	}
	#endregion

	#region Private Methods
	private GameObject InstantiatePrefab (GameObject prefab) {
		// instantiate the prefab
		var obj = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
		obj.transform.localScale = Vector3.one;
		obj.SetActive(false);
		return obj;
	}
	#endregion
}
