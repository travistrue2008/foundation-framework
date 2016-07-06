/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Foundation.Framework
{
	public class Popup : MonoBehaviour
	{
		#region Events
		public delegate void VoidDelegate();
		public event VoidDelegate PopupClosed;
		#endregion


		#region Fields
		[SerializeField]
		private AudioClip _openClip;
		[SerializeField]
		private Tween _transitionTween;
		#endregion


		#region Properties
		public Tween TransitionTween { get { return _transitionTween; } }
		#endregion


		#region Setup
		protected virtual void Awake()
		{
			Services.Get<AudioService>().PlaySound(_openClip);
			if (_transitionTween == null)
				_transitionTween = GetComponent<Tween>();
		}

		protected virtual void OnDestroy()
		{
			// signal the closed event
			if (PopupClosed != null)
				PopupClosed();
		}

		public void Dismiss()
		{
			Services.Get<PopupService>().PopPopup();
		}
		#endregion
	}
}
