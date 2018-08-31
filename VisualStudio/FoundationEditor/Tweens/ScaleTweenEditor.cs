/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TRUEStudios.Foundation.Tweens {
	[CustomEditor(typeof(ScaleTween)), CanEditMultipleObjects]
	public class ScaleTweenEditor : TweenEditor<ScaleTween> {
		#region Override Methods
		protected override void OnSetBegin (ScaleTween target) {
			target.Begin = target.transform.localScale;
		}

		protected override void OnSetEnd (ScaleTween target) {
			target.End = target.transform.localScale;
		}
		#endregion
	}
}
