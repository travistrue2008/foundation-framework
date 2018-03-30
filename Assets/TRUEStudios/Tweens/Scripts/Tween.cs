/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

using TRUEStudios.Core;


namespace TRUEStudios.Tweens
{
	[Serializable]
	public class TweenEvent : UnityEvent<float> { }

	public class Tween : MonoBehaviour
	{
		public enum AwakeTarget { Begin, End }
		public enum PlaybackMode { Once, Looping, Pingpong }
		public enum PlaybackState { Stopped, Playing }

		#region Fields
		[SerializeField]
		private AwakeTarget _awakeTarget = AwakeTarget.Begin;
		[SerializeField]
		private PlaybackMode _loopMode = PlaybackMode.Once;
		[SerializeField]
		private PlaybackState _state = PlaybackState.Stopped;
		[SerializeField]
		private bool _playForward = true;
		[SerializeField, Min(0)]
		private int _numIterations = 0;
		[SerializeField, Min(0.01f)]
		private float _duration = 1.0f;
		[SerializeField, Min(0.0f)]
		private float _delay = 0.0f;
		[SerializeField]
		private GameObject _target;
		[SerializeField]
		private AnimationCurve _distributionCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
		[SerializeField]
		private UnityEvent _onPlay = new UnityEvent();
		[SerializeField]
		private UnityEvent _onFinish = new UnityEvent();
		[SerializeField]
		private UnityEvent _onIterate = new UnityEvent();
		[SerializeField]
		private TweenEvent _onUpdate = new TweenEvent();
		[SerializeField]
		private TweenEvent _onProgress = new TweenEvent();

		private float _factor = 0.0f;
		private float _currentTime = 0.0f;
		private Coroutine _processRoutine;
		#endregion

		#region Properties
		public int iterationsLeft { private set; get; }
		public bool playForward { get { return _playForward; } }
		public UnityEvent onPlay { get { return _onPlay; } }
		public UnityEvent onFinish { get { return _onFinish; } }
		public UnityEvent onIterate { get { return _onIterate; } }
		public TweenEvent onUpdate { get { return _onUpdate; } }
		public TweenEvent onProgress { get { return _onProgress; } }

		public float distributedValue {
			get {
				return _distributionCurve.Evaluate(_factor);
			}
		}

		public Transform targetTransform {
			get {
				return (_target != null) ? _target.transform : transform;
			}
		}

		public GameObject target {
			set { _target = value; }
			get { return _target; }
		}
		
		public PlaybackMode loopMode {
			set { _loopMode = value; }
			get { return _loopMode; }
		}

		public PlaybackState state {
			set { _state = value; }
			get { return _state; }
		}

		public int numIterations {
			set { _numIterations = Mathf.Max(0, value); }
			get { return _numIterations; }
		}

		public float duration {
			set { _duration = Mathf.Max(Mathf.Epsilon, value); }
			get { return _duration; }
		}

		public float delay {
			set { _delay = Mathf.Max(0.0f, value); }
			get { return _delay; }
		}

		public float factor {
			set {
				_factor = Mathf.Clamp01(value);
				_currentTime = duration * _factor;
				UpdateTween();
			}

			get { return _factor; }
		}
		#endregion

		#region MonoBehaviour Hooks
		protected virtual void Awake()
		{
			// set the target to self if not set in the Inspector
			if (_target == null)
				_target = gameObject;

			// apply tween changes as desired
			switch (_awakeTarget)
			{
				case AwakeTarget.Begin:
					ResetToBegin();
					break;

				case AwakeTarget.End:
					ResetToEnd();
					break;
			}

			// start playing if set to play, but not already running
			if (_state == PlaybackState.Playing)
				Play(_playForward);
		}

		protected virtual void OnDestroy()
		{
			InvalidateRoutine();
		}
		#endregion

		#region Virtual Methods
		public virtual void ApplyResult() { }
		public virtual void Swap() { }
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
			InvalidateRoutine();

			// handle reset logic
			_playForward = forward;
			if (reset)
			{
				if (_playForward)
					ResetToBegin();
				else
					ResetToEnd();
			}

			// begin playing
			iterationsLeft = numIterations;
			_processRoutine = StartCoroutine(Process());
			return _processRoutine;
		}

		public void Increment()
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
			_currentTime = duration;
			UpdateTween();
		}

		public void Stop()
		{
			InvalidateRoutine();
			_state = PlaybackState.Stopped;
		}
		#endregion

		#region Process Tweens
		protected void UpdateTween()
		{
			// update, evaluate and raise event
			_factor = _currentTime / duration;
			_onUpdate.Invoke(distributedValue);
			_onProgress.Invoke(_factor);
			ApplyResult();
		}

		private bool PerformIncrement()
		{
			bool finish = false;
			_currentTime += playForward ? Time.deltaTime : -Time.deltaTime; // offset time based on playback direction

			// handle end of the loop
			switch (_loopMode)
			{
				case PlaybackMode.Once:
					finish = PerformIncrementOnce();
					break;

				case PlaybackMode.Looping:
					finish = PerformIncrementLooping();
					break;

				case PlaybackMode.Pingpong:
					finish = PerformIncrementPingPong();
					break;
			}

			// finish up
			UpdateTween();
			return finish;
		}

		private bool PerformIncrementOnce()
		{
			_currentTime = Mathf.Clamp(_currentTime, 0.0f, duration);
			if (_playForward)
				return (_currentTime == duration);
			else
				return (_currentTime == 0.0f) ;
		}

		private bool PerformIncrementLooping()
		{
			bool finish = false;

			if (_playForward)
			{
				if (_currentTime > duration)
				{
					finish = Iterate();
					if (finish)
						_currentTime = duration;
					else
						_currentTime -= duration;
				}
			} else {
				if (_currentTime < 0.0f)
				{
					finish = Iterate();
					if (finish)
						_currentTime = 0.0f;
					else
						_currentTime += duration;
				}
			}

			return finish;
		}

		private bool PerformIncrementPingPong()
		{
			bool finish = false;

			if (_playForward)
			{
				if (_currentTime >= duration)
				{
					finish = Iterate();
					if (finish)
						_currentTime = duration;
					else
						_currentTime = duration - (_currentTime - duration);
					_playForward = false;
				}
			} else {
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

			return finish;
		}

		private bool Iterate()
		{
			_onIterate.Invoke();
			return (numIterations > 0 && (--iterationsLeft == 0)); // loop indefinitely if numIterations is zero
		}

		private void InvalidateRoutine()
		{
			if (_processRoutine != null)
			{
				StopCoroutine(_processRoutine);
				_processRoutine = null;
			}
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
			_state = PlaybackState.Playing;
			_onPlay.Invoke();

			// process the update loop
			while (true)
			{
				// auto-increment based on elapsed time between frames, and break when deemed "finished"
				if (PerformIncrement()) break;
				yield return null;
			}

			// stop everything
			_processRoutine = null;
			_state = PlaybackState.Stopped;
			_onFinish.Invoke();
		}
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

		protected T _result; // must calculate with: [(_begin - _end) * distributedValue] in implementing class
		#endregion

		#region Properties
		public T result { get { return _result; } }

		public T begin {
			set {
				BeginWillBeSet(value);
				_begin = value;
			}

			get { return _begin; }
		}

		public T end {
			set {
				EndWillBeSet(value);
				_end = value;
			}

			get { return _end; }
		}

		#endregion

		#region Virtual Methods
		protected virtual void BeginWillBeSet(T to) { }
		protected virtual void EndWillBeSet(T to) { }
		#endregion

		#region Methods
		protected override void Awake()
		{
			base.Awake();
			onUpdate.AddListener(delegate { ApplyResult(); });
		}

		public override void Swap()
		{
			T temp = begin;
			begin = end;
			end = temp;
		}
		#endregion
	}
}
