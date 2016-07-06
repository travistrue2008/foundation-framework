/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using System;
using System.Collections;


namespace Foundation.Framework
{
	public class SpawnPrefab : MonoBehaviour
	{
		#region Methods
		public void Spawn(string _prefabName)
		{
			// attempt to load the prefab, and instantiate it
			GameObject prefab = Resources.Load(_prefabName) as GameObject;
			if (prefab != null)
				Spawn(prefab);
		}

		public void Spawn(GameObject _prefab)
		{
			// check if the prefab isn't valid
			if (_prefab == null)
				throw new Exception("Invalid _prefab: " + _prefab.name);

			// attempt to instantiate the prefab as a GameObject
			GameObject obj = Instantiate(_prefab, transform.position, transform.rotation) as GameObject;
			if (obj != null)
			{
				obj.transform.SetParent(transform);
				obj.transform.position = transform.position;
			}
			else
				throw new Exception("Couldn't spawn prefab as a GameObject: " + _prefab.name);
		}
		#endregion
	}
}
