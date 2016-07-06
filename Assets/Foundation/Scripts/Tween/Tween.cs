/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Foundation.Framework
{
	public enum TweenPlaybackState
	{
		Stopped,
		Playing
	}

	public enum TweenLoopMode
	{
		Once,
		Looping,
		Pingpong
	}

	public enum CoordinateSpace
	{
		Local,
		Global
	}


	[Serializable]
	public class TweenEvent : UnityEvent<Tween> { }


	public class Tween : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
	{
		public enum SetMode
		{
			None,
			Begin,
			End
		}


		#region Fields
		[SerializeField]
		private SetMode _setOnAwake;
		[SerializeField]
		private bool _activateOnPress = false;
		[SerializeField]
		private bool _activateOnClick = false;
		[SerializeField]
		private bool _relative = false;
		[SerializeField]
		private bool _playForward = true;
		[SerializeField]
		private int _numIterations = 0;
		[SerializeField]
		private float _duration = 1.0f;
		[SerializeField]
		private float _delay = 0.0f;
		[SerializeField]
		private TweenPlaybackState _state = TweenPlaybackState.Stopped;
		[SerializeField]
		private TweenLoopMode _loopMode = TweenLoopMode.Once;
		[SerializeField]
		private AnimationCurve _animationCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
		[SerializeField]
		private GameObject _target;
		[SerializeField]
		private TweenEvent _onPlay = new TweenEvent();
		[SerializeField]
		private TweenEvent _onFinish = new TweenEvent();
		[SerializeField]
		private TweenEvent _onUpdate = new TweenEvent();
		[SerializeField]
		private TweenEvent _onIterate = new TweenEvent();

		private float _factor = 0.0f;
		private float _currentTime = 0.0f;
		private Coroutine _processRoutine;
		#endregion


		#region Properties
		public int Iterations { private set; get; }
		public bool ActivateOnPress { set { _activateOnPress = value; } get { return _activateOnPress; } }
		public bool ActivateOnClick { set { _activateOnClick = value; } get { return _activateOnClick; } }
		public bool Relative { set { _relative = value; } get { return _relative; } }
		public bool PlayingForward { set { _playForward = value; } get { return _playForward; } }
		public AnimationCurve Curve { set { _animationCurve = value; } get { return _animationCurve; } }
		public GameObject Target { set { _target = value; } get { return _target; } }
		public TweenEvent TweenPlayed { get { return _onPlay; } }
		public TweenEvent TweenFinished { get { return _onFinish; } }
		public TweenEvent TweenUpdated { get { return _onUpdate; } }
		public TweenEvent TweenIterated { get { return _onIterate; } }

		public SetMode SetOnAwake
		{
			set { _setOnAwake = value; }
			get { return _setOnAwake; }
		}

		public TweenPlaybackState State
		{
			set
			{
				if (_state != value)
					_state = value;
			}

			get { return _state; }
		}

		public int NumIterations
		{
			set
			{
				_numIterations = value;
				if (_numIterations < 0)
					_numIterations = 0;
			}

			get { return _numIterations; }
		}

		public float Duration
		{
			set
			{
				_duration = value;
				if (_duration < 0.0f)
					_duration = 0.0f;
			}

			get { return _duration; }
		}

		public float Delay
		{
			set
			{
				_delay = value;
				if (_delay < 0.0f)
					_delay = 0.0f;
			}
			get { return _delay; }
		}

		public float Factor
		{
			set
			{
				_factor = Mathf.Clamp01(value);
				_currentTime = Duration * _factor;
				UpdateTween();
			}
			get { return _factor; }
		}

		public TweenLoopMode LoopMode
		{
			set { _loopMode = value; }
			get { return _loopMode; }
		}
		#endregion


		#region Setup
		protected virtual void Awake()
		{
			if (_target == null)
				_target = gameObject;
			if (State == TweenPlaybackState.Playing)
				Play(_playForward); // start playing if set to play, but not already running
		}

		protected virtual void OnDestroy()
		{
			if (_processRoutine != null)
			{
				StopCoroutine(_processRoutine);
				_processRoutine = null;
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (_activateOnClick)
				Play(true, true);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (_activateOnPress)
				Play();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (_activateOnPress)
				Play(false);
		}
		#endregion


		#region Actions
		public void PlayForward(bool reset = false)
		{
			Play(true, reset);
		}

		public void PlayReverse(bool reset = false)
		{
			Play(false, reset);
		}

		public Coroutine Play(bool forward = true, bool reset = false)
		{
			// cancel any running coroutine
			if (_processRoutine != null)
			{
				StopCoroutine(_processRoutine);
				_processRoutine = null;
			}

			// set relative, and forward playing
			OnRelative();
			_playForward = forward;

			// handle reset logic
			if (reset)
			{
				if (_playForward)
					ResetToBegin();
				else
					ResetToEnd();
			}

			// begin playing
			OnPlay();
			Iterations = NumIterations;
			_processRoutine = StartCoroutine(Process());
			return _processRoutine;
		}

		public void Increment() // UnityEvent-friendly incrementation
		{
			PerformIncrement();
		}

		public void ResetToBegin()
		{
			_currentTime = 0.0f;
			UpdateTween();
		}

		public void ResetToEnd()
		{
			_currentTime = Duration;
			UpdateTween();
		}

		public void Stop()
		{
			if (_processRoutine != null)
			{
				StopCoroutine(_processRoutine);
				_processRoutine = null;
			}
			_state = TweenPlaybackState.Stopped;
		}

		public virtual void Swap()
		{
		}
		#endregion


		#region Process Tweens
		private void UpdateTween()
		{
			// update, evaluate and raise event
			_factor = _currentTime / Duration;
			OnUpdate(_animationCurve.Evaluate(_factor)); // update internal logic
			_onUpdate.Invoke(this);
		}

		public bool PerformIncrement()
		{
			bool finish = false;

			// offset time based on current playback state
			if (_playForward)
				_currentTime += Time.deltaTime;
			else
				_currentTime -= Time.deltaTime;

			// handle end of the loop
			switch (_loopMode)
			{
				case TweenLoopMode.Once:
					_currentTime = Mathf.Clamp(_currentTime, 0.0f, Duration);
					if (_playForward)
					{
						if (_currentTime == Duration)
							finish = true;
					}
					else {
						if (_currentTime == 0.0f)
							finish = true;
					}
					break;

				case TweenLoopMode.Looping:
					if (_playForward)
					{
						if (_currentTime > Duration)
						{
							finish = Iterate();
							if (finish)
								_currentTime = Duration;
							else
								_currentTime -= Duration;
						}
					}
					else {
						if (_currentTime < 0.0f)
						{
							finish = Iterate();
							if (finish)
								_currentTime = 0.0f;
							else
								_currentTime += Duration;
						}
					}
					break;

				case TweenLoopMode.Pingpong:
					if (_playForward)
					{
						if (_currentTime >= Duration)
						{
							finish = Iterate();
							if (finish)
								_currentTime = Duration;
							else
								_currentTime = Duration - (_currentTime - Duration);
							_playForward = false;
						}
					}
					else {
						if (_currentTime <= 0.0f)
						{
							finish = Iterate();
							if (finish)
								_currentTime = 0.0f;
							else
								_currentTime = -_currentTime;
							_playForward = true;
						}
					}
					break;
			}

			UpdateTween();
			return finish;
		}

		private bool Iterate()
		{
			bool result = false;

			// if the iteration count isn't zero, then it's not an indefinite loop
			if (NumIterations > 0)
			{
				// decrement, and break once zero is reached
				--Iterations;
				if (Iterations == 0)
					result = true;
			}

			// raise the event
			_onIterate.Invoke(this);
			return result;
		}

		private IEnumerator Process()
		{
			float delayTime = 0.0f;

			// run the delay upon playing
			while (delayTime < _delay)
			{
				delayTime += Time.deltaTime;
				yield return null;
			}

			// start the tween
			_state = TweenPlaybackState.Playing;
			_onPlay.Invoke(this);

			// process the update loop
			while (true)
			{
				// auto-increment based on elapsed time between frames, and break when deemed "finished"
				if (PerformIncrement()) break;
				yield return null;
			}
			_state = TweenPlaybackState.Stopped; // set state to "Stopped"

			// signal event
			OnFinish();
			_onFinish.Invoke(this);
		}
		#endregion


		#region Virtual Members
		protected virtual void OnPlay() { }
		protected virtual void OnFinish() { }
		protected virtual void OnRelative() { }
		protected virtual void OnUpdate(float factor) { }
		#endregion
	}


	public abstract class Tween<T> : Tween
		where T : new()
	{
		#region Fields
		[SerializeField]
		protected T _begin;
		[SerializeField]
		protected T _end;

		protected T _result;
		protected T _offset;
		protected Transform _transform;
		#endregion


		#region Properties
		public T Result { get { return _result; } }

		public T Begin
		{
			set
			{
				OnBeginWillChange(value);
				_begin = value;
			}

			get { return _begin; }
		}

		public T End
		{
			set
			{
				OnEndWillChange(value);
				_end = value;
			}

			get { return _end; }
		}
		#endregion


		#region Virtual Methods
		protected virtual void OnBeginWillChange(T value) { }
		protected virtual void OnEndWillChange(T value) { }
		#endregion


		#region Methods
		protected override void Awake()
		{
			base.Awake();
			if (Target != null)
			{
				_transform = Target.transform;
				OnRelative();
			}
			else {
				Debug.LogWarning("Target is null");
				_transform = transform;
			}

			// apply tween changes as desired
			switch (SetOnAwake)
			{
				case SetMode.None: break;

				case SetMode.Begin:
					ResetToBegin();
					break;

				case SetMode.End:
					ResetToEnd();
					break;
			}
		}

		public override void Swap()
		{
			T temp = Begin;
			Begin = End;
			End = temp;
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (Target != null)
				_transform = Target.transform;
			else
				_transform = transform;
		}
#endif
		#endregion
	}
}
