/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace TRUEStudios.Tweens {
	public class ColorTween : Tween<Color> {
		#region Fields
		[SerializeField]
		private SpriteRenderer _spriteRenderer;
		[SerializeField]
		private Graphic _graphic;
		#endregion

		#region Properties
		public SpriteRenderer AttachedSpriteRenderer {
			set { _spriteRenderer = value; }
			get { return _spriteRenderer; }
		}

		public Graphic AttachedGraphic {
			set { _graphic = value; }
			get { return _graphic; }
		}
		#endregion

		#region Methods
		#if UNITY_EDITOR
		private ColorTween() {
			Begin = End = Color.white;
		}
		#endif

		protected override void Awake () {
			base.Awake();
			if (_spriteRenderer == null) {
				_spriteRenderer = GetComponent<SpriteRenderer>();
			}

			if (_graphic == null) {
				_graphic = GetComponent<Graphic>();
			}
		}

		public override void ApplyResult () {
			_result = ((_end - _begin) * DistributedValue) + _begin;

			// apply the result to the references
			if (_spriteRenderer != null) {
				_spriteRenderer.color = _result;
			}
			
			if (_graphic != null) {
				_graphic.color = _result;
			}
		}

		protected override void PerformRelative () {
			Color diff = _end - _begin;

			if (_spriteRenderer != null) {
				Begin = _spriteRenderer.color;
			}

			if (_graphic != null) {
				Begin = _graphic.color;
			}

			End = Begin + diff;
		}
		#endregion
	}
}
