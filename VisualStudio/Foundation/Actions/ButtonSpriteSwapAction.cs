/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TRUEStudios.Foundation.Actions {
	[RequireComponent(typeof(Image))]
	public class ButtonSpriteSwapAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
		#region Fields
		[SerializeField]
		private bool _useNativeSize = true;
		[SerializeField]
		private Sprite _normalSprite;
		[SerializeField]
		private Sprite _hoverSprite;
		[SerializeField]
		private Sprite _downSprite;

		private bool _entered = false;
		private bool _down = false;
		private Image _image;
		#endregion

		#region Methods
		private void Awake () {
			// get the Image component, and make sure _normalSprite is assigned
			_image = GetComponent<Image>();
			if (_normalSprite == null) {
				throw new Exception("_normalSprite not assigned in the Inspector.");
			}
		}

		private void ApplySprite () {
			// set the sprite, and reset its size
			_image.sprite = _entered ? (_down ? _downSprite : _hoverSprite) : _normalSprite;
			if (_useNativeSize) {
				_image.SetNativeSize();
			}
		}

		public void OnPointerEnter (PointerEventData eventData) {
			_entered = true;
			ApplySprite();
		}

		public void OnPointerExit (PointerEventData eventData) {
			_entered = false;
			ApplySprite();
		}

		public void OnPointerDown (PointerEventData eventData) {
			_down = true;
			ApplySprite();
		}

		public void OnPointerUp (PointerEventData eventData) {
			_down = false;
			ApplySprite();
		}
		#endregion
	}
}
