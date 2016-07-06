/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;


namespace Foundation.Framework
{
	public class Blocker : MonoBehaviour
	{
		#region Fields
		private int _count = 0;
		private Image _image;
		#endregion


		#region Properties
		public int Count
		{
			set
			{
				_count = value;
				if (_image == null)
					_image = GetComponent<Image>();
				if (_image != null)
					_image.enabled = (_count > 0);
			}

			get { return _count; }
		}
		#endregion


		#region Methods
		private void Awake()
		{
			_image = GetComponent<Image>();
			if (_image == null)
				throw new Exception("No Image component attached to Blocker GameObject.");
			_image.enabled = false;
		}
		#endregion
	}
}
