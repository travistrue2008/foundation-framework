﻿/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace TRUEStudios.Foundation.Core {
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver {
		[SerializeField]
		private List<TKey> _keys = new List<TKey>();

		[SerializeField]
		private List<TValue> _values = new List<TValue>();

		public void OnBeforeSerialize() {
			_keys.Clear();
			_values.Clear();

			// save the dictionary to lists
			foreach(KeyValuePair<TKey, TValue> pair in this) {
				_keys.Add(pair.Key);
				_values.Add(pair.Value);
			}
		}

		public void OnAfterDeserialize() {
			Clear();

			// make sure there is a 1:1 ratio of keys to values
			if(_keys.Count != _values.Count) {
				const string Format = "there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable.";
				throw new Exception(string.Format(Format, _keys.Count, _values.Count));
			}

			// load dictionary from lists
			for(int i = 0; i < _keys.Count; ++i) {
				Add(_keys[i], _values[i]);
			}
		}
	}
}
