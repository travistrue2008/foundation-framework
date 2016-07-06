/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using System.Collections;


namespace Foundation.Framework
{
	public enum RotationTweenMode
	{
		Nearest,
		Farthest,
		Clockwise,
		CounterClockwise
	}


	public class RotationTween : Tween<Vector3>
	{
		private enum Axis { X, Y, Z }


		#region Fields
		[SerializeField]
		private RotationTweenMode _xRotationMode = RotationTweenMode.Nearest;
		[SerializeField]
		private RotationTweenMode _yRotationMode = RotationTweenMode.Nearest;
		[SerializeField]
		private RotationTweenMode _zRotationMode = RotationTweenMode.Nearest;

		private Vector3 _diff = Vector3.zero;
		private Vector3 _fixedBegin = Vector3.zero;
		private Vector3 _fixedEnd = Vector3.zero;
		#endregion


		#region Properties
		public RotationTweenMode RotationModeX
		{
			set
			{
				_xRotationMode = value;
				_diff.x = GetRotationAngle(_xRotationMode, _fixedEnd.x - _fixedBegin.x);
			}
			get { return _xRotationMode; }
		}

		public RotationTweenMode RotationModeY
		{
			set
			{
				_yRotationMode = value;
				_diff.y = GetRotationAngle(_yRotationMode, _fixedEnd.y - _fixedBegin.y);
			}
			get { return _yRotationMode; }
		}

		public RotationTweenMode RotationModeZ
		{
			set
			{
				_zRotationMode = value;
				_diff.z = GetRotationAngle(_zRotationMode, _fixedEnd.z - _fixedBegin.z);
			}
			get { return _zRotationMode; }
		}

		protected override void OnBeginWillChange(Vector3 value)
		{
			_fixedBegin = BoundAngle(value);
			FindDifference();
		}

		protected override void OnEndWillChange(Vector3 value)
		{
			_fixedEnd = BoundAngle(value);
			FindDifference();
		}

		public Vector3 FixedBegin { get { return _fixedBegin; } }
		public Vector3 FixedEnd { get { return _fixedEnd; } }
		#endregion


		#region Methods
		protected override void OnRelative()
		{
			base.OnRelative();
			Begin = Begin;  // force overridden property to update
			End = End;      // force overridden property to update
			FindDifference();

			if (_transform != null)
				_offset = _transform.position;
		}

		protected override void OnUpdate(float factor)
		{
			base.OnUpdate(factor);
			if (Relative)
				_result = _offset + _diff * factor;
			else
				_result = Begin + _diff * factor;

			// apply the transform
			if (_transform != null)
				_transform.localRotation = Quaternion.Euler(_result);
		}

		private Vector3 BoundAngle(Vector3 eulerAngles)
		{
			// wrap all angles to a domain of 0 - 360 degrees
			while (eulerAngles.x < 0.0f) eulerAngles.x = (180.0f + eulerAngles.x) + 180.0f;
			while (eulerAngles.y < 0.0f) eulerAngles.y = (180.0f + eulerAngles.y) + 180.0f;
			while (eulerAngles.z < 0.0f) eulerAngles.z = (180.0f + eulerAngles.z) + 180.0f;
			while (eulerAngles.x >= 360.0f) eulerAngles.x -= 360.0f;
			while (eulerAngles.y >= 360.0f) eulerAngles.y -= 360.0f;
			while (eulerAngles.z >= 360.0f) eulerAngles.z -= 360.0f;
			return eulerAngles;
		}

		private void FindDifference()
		{
			_diff = _fixedEnd - _fixedBegin;
			_diff.x = GetRotationAngle(RotationModeX, _diff.x);
			_diff.y = GetRotationAngle(RotationModeY, _diff.y);
			_diff.z = GetRotationAngle(RotationModeZ, _diff.z);
		}

		private float GetRotationAngle(RotationTweenMode mode, float angle)
		{
			// set the component based on rotation mode
			switch (mode)
			{
				case RotationTweenMode.Nearest:
					if (Mathf.Abs(angle) > 180.0f)
					{
						if (angle > 0.0f)
							angle = (360.0f - angle) * -1.0f;
						else
							angle = 360.0f - Mathf.Abs(angle);
					}
					break;

				case RotationTweenMode.Farthest:
					if (Mathf.Abs(angle) < 180.0f)
					{
						if (angle > 0.0f)
							angle = (360.0f - angle) * -1.0f;
						else
							angle = 360.0f - Mathf.Abs(angle);
					}
					break;

				case RotationTweenMode.Clockwise:
					if (angle > 0.0f)
						angle = (360.0f - angle) * -1.0f;
					break;

				case RotationTweenMode.CounterClockwise:
					if (angle < 0.0f)
						angle = 360.0f - Mathf.Abs(angle);
					break;
			}
			return angle;
		}
		#endregion
	}
}
