/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using System.Collections;


namespace Foundation.Framework
{
	public class PositionTween : Tween<Vector3>
	{
		#region Fields
		[SerializeField]
		CoordinateSpace _space = CoordinateSpace.Local;
		#endregion


		#region Properties
		public CoordinateSpace TransformSpace { set { _space = value; } get { return _space; } }
		#endregion


		#region Methods
		protected override void OnRelative()
		{
			base.OnRelative();
			if (_transform != null)
			{
				switch (TransformSpace)
				{
					case CoordinateSpace.Local:
						_offset = _transform.localPosition;
						break;

					case CoordinateSpace.Global:
						_offset = _transform.position;
						break;
				}
			}
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
			{
				switch (TransformSpace)
				{
					case CoordinateSpace.Local:
						_transform.localPosition = _result;
						break;

					case CoordinateSpace.Global:
						_transform.position = _result;
						break;
				}
			}
		}
		#endregion
	}
}
