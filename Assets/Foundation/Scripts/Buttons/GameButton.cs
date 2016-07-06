/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;


namespace Foundation.Framework
{
	[RequireComponent(typeof(Button))]
	public class GameButton : MonoBehaviour, IPointerClickHandler
	{
		#region Fields
		[SerializeField]
		private AudioClip _clickClip;

		private Button _button;
		#endregion


		#region Properties
		public Button AttachedButton { get { return _button; } }
		public AudioClip ClickClip { get { return _clickClip; } }
		#endregion


		#region Methods
		protected virtual void OnClick(PointerEventData e) { }

		protected virtual void Awake()
		{
			// get the Button component
			_button = GetComponent<Button>();
			if (_button == null)
				throw new Exception("No Button component attached to GameObject.");
		}

		public void OnPointerClick(PointerEventData e)
		{
			// make sure the button is interactible
			if (AttachedButton.interactable)
			{
				Services.Get<AudioService>().PlaySound(_clickClip);
				OnClick(e);
			}
		}
		#endregion
	}
}
