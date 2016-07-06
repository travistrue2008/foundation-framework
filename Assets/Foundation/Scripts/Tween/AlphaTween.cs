/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace Foundation.Framework
{
	public class AlphaTween : Tween<float>
	{
		#region Fields
		[SerializeField]
		private SpriteRenderer _spriteRenderer;
		[SerializeField]
		private Graphic _graphic;

		private Color _cachedColor;
		#endregion


		#region Properties
		public SpriteRenderer TargetSpriteRenderer
		{
			set { _spriteRenderer = value; }
			get { return _spriteRenderer; }
		}

		public Graphic TargetGraphic
		{
			set { _graphic = value; }
			get { return _graphic; }
		}
		#endregion


		#region Methods
#if UNITY_EDITOR
		private AlphaTween()
		{
			Begin = End = 1.0f;
		}
#endif

		protected override void Awake()
		{
			base.Awake();
			if (_spriteRenderer == null)
				_spriteRenderer = GetComponent<SpriteRenderer>();
			if (_graphic == null)
				_graphic = GetComponent<Graphic>();
		}

		protected override void OnUpdate(float factor)
		{
			if (Relative)
				_result = _offset + (End - Begin) * factor;
			else
				_result = Begin + (End - Begin) * factor;

			// check if any of the references are valid
			if (_spriteRenderer != null || _graphic != null)
			{
				// update the SpriteRenderer
				if (_spriteRenderer != null)
				{
					_cachedColor = _spriteRenderer.color;
					_cachedColor.a = _result;
					_spriteRenderer.color = _cachedColor;
				}

				// update the Graphic
				if (_graphic != null)
				{
					_cachedColor = _graphic.color;
					_cachedColor.a = _result;
					_graphic.color = _cachedColor;
				}
			}
		}
		#endregion
	}
}
