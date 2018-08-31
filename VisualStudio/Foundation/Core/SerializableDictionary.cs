/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace TRUEStudios.Foundation.Core {
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver {
		[SerializeField]
		private List<TKey> keys = new List<TKey>();

		[SerializeField]
		private List<TValue> values = new List<TValue>();

		public void OnBeforeSerialize() {
			keys.Clear();
			values.Clear();

			// save the dictionary to lists
			foreach(KeyValuePair<TKey, TValue> pair in this) {
				keys.Add(pair.Key);
				values.Add(pair.Value);
			}
		}

		public void OnAfterDeserialize() {
			Clear();

			// make sure there is a 1:1 ratio of keys to values
			if(keys.Count != values.Count) {
				throw new Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));
			}

			// load dictionary from lists
			for(int i = 0; i < keys.Count; ++i) {
				Add(keys[i], values[i]);
			}
		}
	}
}
