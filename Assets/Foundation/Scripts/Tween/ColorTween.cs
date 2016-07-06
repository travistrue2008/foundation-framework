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
	public class ColorTween : Tween<Color>
	{
		#region Fields
		[SerializeField]
		private SpriteRenderer _spriteRenderer;
		[SerializeField]
		private Graphic _graphic;
		#endregion


		#region Methods
#if UNITY_EDITOR
		private ColorTween()
		{
			Begin = End = Color.white;
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

			// update the references
			if (_spriteRenderer != null)
				_spriteRenderer.color = _result;
			if (_graphic != null)
				_graphic.color = _result;
		}
		#endregion
	}
}
