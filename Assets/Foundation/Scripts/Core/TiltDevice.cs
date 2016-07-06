/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;


namespace Foundation.Framework
{
	[Serializable]
	public class TiltEvent : UnityEvent<float> { }


	public class TiltDevice : MonoBehaviour
	{
		private enum Direction
		{
			Left,
			Right
		}


		#region Constants
		private const float LargeEpsilon = 0.01f;
		#endregion


		#region Fields
		[SerializeField]
		private Direction _direction;
		[SerializeField, Min(0.0f)]
		private float _angleThreshold = 0.0f;
		[SerializeField, Min(0.0f)]
		private float _maxAngle = 90.0f;

#if UNITY_EDITOR
		[SerializeField, Min(0.0f)]
		private float _degreesPerSecond = 45.0f;
#endif

		[SerializeField]
		private AudioClip _tiltMetClip;
		[SerializeField]
		private AudioClip _tiltUnmetClip;
		[SerializeField]
		private UnityEvent _onThresholdMet = new UnityEvent();
		[SerializeField]
		private UnityEvent _onThresholdUnmet = new UnityEvent();
		[SerializeField]
		private TiltEvent _onTiltChanged = new TiltEvent();

		private bool _thresholdMet = false;
		private float _angle = 0.0f;
		private AudioSource _source;
		#endregion


		#region Properties
		public float TiltFactor { get { return Mathf.Abs(_angle / _maxAngle); } }
		#endregion


		#region Methods
		private void OnDisable()
		{
			if (_source != null)
			{
				_source.Stop();
				_source = null;
			}
		}

		private void Update()
		{
			CheckInput();
			switch (_direction)
			{
				case Direction.Left:
					if (_angle >= _angleThreshold)
					{
						if (!_thresholdMet)
						{
							_source = Services.Get<AudioService>().PlaySound(_tiltMetClip, true);
							_onThresholdMet.Invoke();
							_thresholdMet = true;
						}
					}
					else {
						if (_thresholdMet)
						{
							Services.Get<AudioService>().PlaySound(_tiltUnmetClip);
							if (_source != null)
							{
								_source.Stop();
								_source = null;
							}
							_onThresholdUnmet.Invoke();
							_thresholdMet = false;
						}
					}
					break;

				case Direction.Right:
					if (_angle <= -_angleThreshold)
					{
						if (!_thresholdMet)
						{
							_source = Services.Get<AudioService>().PlaySound(_tiltMetClip, true);
							_onThresholdMet.Invoke();
							_thresholdMet = true;
						}
					}
					else {
						if (_thresholdMet)
						{
							Services.Get<AudioService>().PlaySound(_tiltUnmetClip);
							if (_source != null)
							{
								_source.Stop();
								_source = null;
							}
							_onThresholdUnmet.Invoke();
							_thresholdMet = false;
						}
					}
					break;
			}
			_onTiltChanged.Invoke(TiltFactor); // signal angle-changed event
		}

#if UNITY_EDITOR
		private void TiltByKey(KeyCode keyCode)
		{
			// handle based on direction
			switch (_direction)
			{
				case Direction.Left:
					// increase the angle if the target key is held down
					if (Input.GetKey(keyCode))
						_angle += _degreesPerSecond * Time.deltaTime;
					else
						_angle -= _degreesPerSecond * Time.deltaTime;
					_angle = Mathf.Clamp(_angle, 0.0f, _maxAngle);
					break;

				case Direction.Right:
					// increase the angle if the target key is held down
					if (Input.GetKey(keyCode))
						_angle -= _degreesPerSecond * Time.deltaTime;
					else
						_angle += _degreesPerSecond * Time.deltaTime;
					_angle = Mathf.Clamp(_angle, 0.0f, _maxAngle);
					break;
			}
		}

		private void CheckInput()
		{
			// handle based on direction
			switch (_direction)
			{
				case Direction.Left:
					TiltByKey(KeyCode.LeftArrow);
					break;

				case Direction.Right:
					TiltByKey(KeyCode.RightArrow);
					break;
			}
		}
#else
	private void CheckInput()
	{
		float currentAngle = -Input.acceleration.x * _maxAngle * 1.25f;
		_angle += (currentAngle - _angle) * Time.deltaTime * 4.0f;
	}
#endif
		#endregion
	}
}
