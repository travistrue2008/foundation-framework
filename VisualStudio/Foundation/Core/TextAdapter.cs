/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using TRUEStudios.Foundation.Events;

namespace TRUEStudios.Foundation.Core {
	public class TextAdapter : MonoBehaviour {
		#region Fields
		[SerializeField]
		private string _format = string.Empty;
		[SerializeField]
		private StringEvent _onSet = new StringEvent();

		private object _cache = null;
		#endregion

		#region Properties
		public StringEvent OnSet{ get { return _onSet; } }

		public string Format {
			set {
				_format = value;
				Send(_cache);
			}

			get { return _format; }
		}
		#endregion

		#region Actions
		public void Set (bool value) {
			Send(value);
		}

		public void Set(int value) {
			Send(value);
		}

		public void Set(float value) {
			Send(value);
		}

		public void Set(string value) {
			Send(value);
		}
		#endregion

		#region Private Methods
		private void Send (object value) {
			_cache = value;

			string result = (_cache != null) ? string.Format(_format, _cache) : string.Empty;
			_onSet.Invoke(result);
		}
		#endregion
	}
}
