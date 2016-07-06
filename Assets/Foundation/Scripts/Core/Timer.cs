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
	public class TimerEvent : UnityEvent<float> { }


	public class Timer : MonoBehaviour
	{
		#region Fields
		[SerializeField]
		private bool _playOnAwake;
		[SerializeField]
		private bool _looping;
		[SerializeField]
		private float _duration = 1.0f;
		[SerializeField]
		private UnityEvent _onPlay = new UnityEvent();
		[SerializeField]
		private UnityEvent _onFinish = new UnityEvent();
		[SerializeField]
		private UnityEvent _onIterate = new UnityEvent();
		[SerializeField]
		private TimerEvent _onUpdate = new TimerEvent();
		[SerializeField]
		private TimerEvent _onProgress = new TimerEvent();

		private float _currentTime = 0.0f;
		private Coroutine _timerRoutine;
		#endregion


		#region Properties
		public bool IsPlaying { get { return _timerRoutine != null; } }
		public UnityEvent OnPlay { get { return _onPlay; } }
		public UnityEvent OnFinish { get { return _onFinish; } }
		public UnityEvent OnIterate { get { return _onIterate; } }
		public TimerEvent OnUpdate { get { return _onUpdate; } }
		public TimerEvent OnProgress { get { return _onProgress; } }

		public bool IsLooping
		{
			set { _looping = value; }
			get { return _looping; }
		}

		public float Duration
		{
			set { _duration = value; }
			get { return _duration; }
		}

		public float Progress
		{
			set
			{
				float progress = Mathf.Clamp01(value);
				_currentTime = progress / Duration;
				_onUpdate.Invoke(_currentTime);
				_onProgress.Invoke(progress);
			}

			get { return _currentTime / Duration; }
		}
		#endregion


		#region Private Methods
		private void Awake()
		{
			if (_playOnAwake)
				Play();
		}

		private IEnumerator ProcessTimer(float delay)
		{
			// process the delay
			if (delay > 0.0f)
				yield return new WaitForSeconds(delay);

			// update the current time
			_onPlay.Invoke(); // signal play event
			while (IsLooping || _currentTime < Duration)
			{
				IncrementProgress(); // TODO: increment forward/backwards playback, and convert Tween over to use it
				yield return null;
			}
			_timerRoutine = null;
		}
		#endregion


		#region Actions
		public void Play(float delay = 0.0f)
		{
			// start the coroutine if not already
			if (_timerRoutine == null)
				_timerRoutine = StartCoroutine(ProcessTimer(delay));
		}

		public void IncrementProgress()
		{
			// increment progress
			if (_currentTime < Duration)
				_currentTime += Time.deltaTime;
			else
				return;

			// update and bound _currentTime
			if (_currentTime >= Duration)
			{
				// check if looping
				if (IsLooping)
				{
					// decrement the duration and signal iteration
					_currentTime -= Duration;
					_onIterate.Invoke();
				}
				else
					_currentTime = Duration; // bound to duration
			}

			// signal changed event
			_onUpdate.Invoke(_currentTime);
			_onProgress.Invoke(Progress);

			// check if finished
			if (Progress == 1.0f)
				_onFinish.Invoke();
		}

		public void Pause()
		{
			// stop the currently-playing coroutine
			if (_timerRoutine != null)
			{
				StopCoroutine(_timerRoutine);
				_timerRoutine = null;
			}
		}

		public void Stop()
		{
			// stop and reset the 
			if (_timerRoutine != null)
			{
				StopCoroutine(_timerRoutine);
				_timerRoutine = null;
			}
			Progress = 0.0f;
		}

		public void Reset()
		{
			Progress = 0.0f;
		}
		#endregion
	}
}
