/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.UI;


namespace TRUEStudios.Tweens
{
	public class ColorTween : Tween<Color>
	{
		#region Fields
		[SerializeField]
		private SpriteRenderer _spriteRenderer;
		[SerializeField]
		private Graphic _graphic;
		#endregion

		#region Properties
		public SpriteRenderer spriteRenderer {
			set { _spriteRenderer = value; }
			get { return _spriteRenderer; }
		}

		public Graphic graphic {
			set { _graphic = value; }
			get { return _graphic; }
		}
		#endregion

		#region Methods
#if UNITY_EDITOR
		private ColorTween()
		{
			begin = end = Color.white;
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

		public override void ApplyResult()
		{
			_result = ((_end - _begin) * distributedValue) + _begin;

			// apply the result to the references
			if (_spriteRenderer != null)
				_spriteRenderer.color = _result;
			if (_graphic != null)
				_graphic.color = _result;
		}
		#endregion
	}
}
