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
using TRUEStudios.Events;

namespace TRUEStudios.Tweens {
	public abstract class Tween : MonoBehaviour {
		public enum AwakeTarget { None, Begin, End }
		public enum PlaybackMode { Once, Looping, Pingpong }
		public enum PlaybackState { Stopped, Playing }

		#region Fields
		[SerializeField]
		private AwakeTarget _awakeTarget = AwakeTarget.None;
		[SerializeField]
		private PlaybackMode _loopMode = PlaybackMode.Once;
		[SerializeField]
		private PlaybackState _state = PlaybackState.Stopped;
		[SerializeField]
		private bool _playingForward = true;
		[SerializeField]
		private int _numIterations = 0;
		[SerializeField]
		private float _duration = 1.0f;
		[SerializeField]
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
		private FloatEvent _onUpdate = new FloatEvent();
		[SerializeField]
		private FloatEvent _onProgress = new FloatEvent();

		private float _factor = 0.0f;
		private float _currentTime = 0.0f;
		private Coroutine _processRoutine;
		#endregion

		#region Properties
		public int IterationsLeft { private set; get; }
		public bool IsPlayingForward { get { return _playingForward; } }
		public UnityEvent OnPlay { get { return _onPlay; } }
		public UnityEvent OnFinish { get { return _onFinish; } }
		public UnityEvent OnIterate { get { return _onIterate; } }
		public FloatEvent OnUpdate { get { return _onUpdate; } }
		public FloatEvent OnProgress { get { return _onProgress; } }

		public float DistributedValue {
			get {
				return _distributionCurve.Evaluate(_factor);
			}
		}

		public Transform TargetTransform {
			get {
				return (_target != null) ? _target.transform : transform;
			}
		}

		public GameObject Target {
			set { _target = value; }
			get { return _target; }
		}
		
		public PlaybackMode LoopMode {
			set { _loopMode = value; }
			get { return _loopMode; }
		}

		public PlaybackState State {
			set { _state = value; }
			get { return _state; }
		}

		public int NumIterations {
			set { _numIterations = Mathf.Max(0, value); }
			get { return _numIterations; }
		}

		public float Duration {
			set { _duration = Mathf.Max(Mathf.Epsilon, value); }
			get { return _duration; }
		}

		public float Delay {
			set { _delay = Mathf.Max(0.0f, value); }
			get { return _delay; }
		}

		public float Factor {
			set {
				_factor = Mathf.Clamp01(value);
				_currentTime = Duration * _factor;
				UpdateTween();
			}

			get { return _factor; }
		}
		#endregion

		#region MonoBehaviour Hooks
		protected virtual void Awake () {
			// set the target to self if not set in the Inspector
			if (_target == null) {
				_target = gameObject;
			}

			// apply tween changes as desired
			switch (_awakeTarget) {
				case AwakeTarget.Begin:
					ResetToBegin();
					break;

				case AwakeTarget.End:
					ResetToEnd();
					break;
			}

			// start playing if set to play, but not already running
			if (_state == PlaybackState.Playing) {
				Play(_playingForward);
			}
		}

		protected virtual void OnDestroy () {
			Debug.Log($"{gameObject.name}:    Tween.OnDestroy()");
			InvalidateRoutine();
		}

		private void OnValidate () {
			NumIterations = _numIterations;
			Duration = _duration;
			Delay = _delay;
		}
		#endregion

		#region Abstract and Virtual Methods
		protected abstract void PerformRelative ();

		public virtual void ApplyResult () { }
		public virtual void Swap () { }
		#endregion

		#region Actions
		public void PlayForward (bool reset = false, bool relative = false) {
			Play(true, reset, relative);
		}

		public void PlayReverse (bool reset = false, bool relative = false) {
			Play(false, reset, relative);
		}

		public Coroutine Play (bool forward = true, bool reset = false, bool relative = false) {
			InvalidateRoutine(); // cancel any running coroutine

			// set playback direction, and check if the tween should be adjusted relatively
			_playingForward = forward;
			if (relative) {
				PerformRelative();
			}

			// handle reset logic
			if (reset) {
				if (_playingForward) {
					ResetToBegin();
				} else {
					ResetToEnd();
				}
			}

			// begin playing
			IterationsLeft = NumIterations;
			_processRoutine = StartCoroutine(Process());
			return _processRoutine;
		}

		public void Increment () {
			PerformIncrement();
		}

		public void ResetToBegin () {
			_currentTime = 0.0f;
			UpdateTween();
		}

		public void ResetToEnd () {
			_currentTime = Duration;
			UpdateTween();
		}

		public void Stop () {
			InvalidateRoutine();
			_state = PlaybackState.Stopped;
			Debug.Log($"{gameObject.name}:    Stop()");
		}
		#endregion

		#region Process Tweens
		protected void UpdateTween () {
			// update, evaluate and raise event
			_factor = _currentTime / Duration;
			_onUpdate.Invoke(DistributedValue);
			_onProgress.Invoke(_factor);
			ApplyResult();
		}

		private bool PerformIncrement () {
			bool finish = false;
			_currentTime += IsPlayingForward ? Time.deltaTime : -Time.deltaTime; // offset time based on playback direction

			// handle end of the loop
			switch (_loopMode) {
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

			UpdateTween();
			return finish;
		}

		private bool PerformIncrementOnce () {
			_currentTime = Mathf.Clamp(_currentTime, 0.0f, Duration);
			if (_playingForward) {
				return (_currentTime == Duration);
			} else {
				return (_currentTime == 0.0f) ;
			}
		}

		private bool PerformIncrementLooping () {
			bool finish = false;

			if (_playingForward) {
				if (_currentTime > Duration) {
					finish = Iterate();
					if (finish) {
						_currentTime = Duration;
					} else {
						_currentTime -= Duration;
					}
				}
			} else {
				if (_currentTime < 0.0f) {
					finish = Iterate();
					if (finish) {
						_currentTime = 0.0f;
					} else {
						_currentTime += Duration;
					}
				}
			}

			return finish;
		}

		private bool PerformIncrementPingPong () {
			bool finish = false;

			if (_playingForward) {
				if (_currentTime >= Duration) {
					finish = Iterate();
					if (finish) {
						_currentTime = Duration;
					} else {
						_currentTime = Duration - (_currentTime - Duration);
					}

					_playingForward = false;
				}
			} else {
				if (_currentTime <= 0.0f) {
					finish = Iterate();
					if (finish) {
						_currentTime = 0.0f;
					} else {
						_currentTime = -_currentTime;
					}

					_playingForward = true;
				}
			}

			return finish;
		}

		private bool Iterate () {
			_onIterate.Invoke();
			return (NumIterations > 0 && (--IterationsLeft == 0)); // loop indefinitely if numIterations is zero
		}

		private void InvalidateRoutine () {
			if (_processRoutine != null) {
				StopCoroutine(_processRoutine);
				_processRoutine = null;
			}
		}

		private IEnumerator Process () {
			float delayTime = 0.0f;

			// run the delay upon playing
			while (delayTime < _delay) {
				delayTime += Time.deltaTime;
				yield return null;
			}

			// start the tween
			_state = PlaybackState.Playing;
			_onPlay.Invoke();

			// process the update loop
			while (true) {
				bool result = PerformIncrement();
				Debug.Log($"{gameObject.name}:    Process(): {Factor}   Result: {result}");
				if (result) {
					break;
				}

				yield return null;
			}

			// stop everything
			Debug.Log($"{gameObject.name}:    Finished tween: {this.GetType()}");
			_processRoutine = null;
			_state = PlaybackState.Stopped;
			_onFinish.Invoke();
		}
		#endregion
	}

	public abstract class Tween<T> : Tween where T : new() {
		#region Fields
		[SerializeField]
		protected T _begin;
		[SerializeField]
		protected T _end;

		protected T _result; // must calculate with: [(_begin - _end) * distributedValue] in implementing class
		#endregion

		#region Properties
		public T Result { get { return _result; } }

		public T Begin {
			set {
				BeginWillBeSet(value);
				_begin = value;
			}

			get { return _begin; }
		}

		public T End {
			set {
				EndWillBeSet(value);
				_end = value;
			}

			get { return _end; }
		}

		#endregion

		#region Virtual Methods
		protected virtual void BeginWillBeSet (T to) { }
		protected virtual void EndWillBeSet (T to) { }
		#endregion

		#region Methods
		protected override void Awake () {
			base.Awake();
			OnUpdate.AddListener(delegate { ApplyResult(); });
		}

		public override void Swap () {
			T temp = Begin;
			Begin = End;
			End = temp;
		}
		#endregion
	}
}
