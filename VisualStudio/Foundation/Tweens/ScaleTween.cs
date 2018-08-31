/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;

namespace TRUEStudios.Foundation.Tweens {
	public class ScaleTween : Tween<Vector3> {
		#region Methods
		#if UNITY_EDITOR
		private ScaleTween () {
			Begin = End = Vector3.one;
		}
		#endif

		protected override void PerformRelative () {
			Vector3 diff = _end - _begin;
			Begin = transform.localScale;
			End = Begin + diff;
		}

		public override void ApplyResult () {
			_result = ((_end - _begin) * DistributedValue) + _begin;
			transform.localScale = _result;
		}
		#endregion
	}
}
