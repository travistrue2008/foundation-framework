/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TRUEStudios.Tweens {
	[CustomEditor(typeof(ColorTween)), CanEditMultipleObjects]
	public class ColorTweenEditor : TweenEditor<ColorTween> {
		#region Override Methods
		protected override void OnSetBegin (ColorTween target) {
			if (target.AttachedSpriteRenderer != null) {
				target.Begin = target.AttachedSpriteRenderer.color;
			}

			if (target.AttachedGraphic != null) {
				target.Begin = target.AttachedGraphic.color;
			}
		}

		protected override void OnSetEnd (ColorTween target) {
			if (target.AttachedSpriteRenderer != null) {
				target.End = target.AttachedSpriteRenderer.color;
			}

			if (target.AttachedGraphic != null) {
				target.End = target.AttachedGraphic.color;
			}
		}
		#endregion
	}
}
