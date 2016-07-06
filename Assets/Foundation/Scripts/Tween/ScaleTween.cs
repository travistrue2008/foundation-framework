/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using System.Collections;


namespace Foundation.Framework
{
	public class ScaleTween : Tween<Vector3>
	{
		#region Methods
#if UNITY_EDITOR
		private ScaleTween()
		{
			Begin = End = Vector3.one;
		}
#endif

		protected override void OnRelative()
		{
			base.OnRelative();
			if (_transform != null)
				_offset = _transform.localScale;
		}

		protected override void OnUpdate(float factor)
		{
			base.OnUpdate(factor);
			if (Relative)
				_result = _offset + (End - Begin) * factor;
			else
				_result = Begin + (End - Begin) * factor;

			// apply the transform
			if (_transform != null)
				_transform.localScale = _result;
		}
		#endregion
	}
}
