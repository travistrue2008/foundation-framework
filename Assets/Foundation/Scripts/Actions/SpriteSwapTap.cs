/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;


namespace Foundation.Framework
{
	public class SpriteSwapTap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		#region Fields
		[SerializeField]
		private Sprite _tappedSprite;
		[SerializeField]
		private AudioClip _swapClip;
		[SerializeField]
		private AudioClip _restoreClip;

		private Sprite _originalSprite;
		private SpriteRenderer _spriteRenderer;
		#endregion


		#region Methods
		private void Awake()
		{
			// get the SpriteRenderer component
			_spriteRenderer = GetComponent<SpriteRenderer>();
			if (_spriteRenderer != null)
				_originalSprite = _spriteRenderer.sprite;
			else
				throw new Exception("Couldn't find SpriteRenderer.");
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			Services.Get<AudioService>().PlaySound(_swapClip);
			_spriteRenderer.sprite = _tappedSprite;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			Services.Get<AudioService>().PlaySound(_restoreClip);
			_spriteRenderer.sprite = _originalSprite;
		}
		#endregion
	}
}
