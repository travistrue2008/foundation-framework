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
		public enum PlaybackMode { Once, Looping, Pingpong }

		#region Fields
		[SerializeField]
		private PlaybackMode _loopMode = PlaybackMode.Once;
		[SerializeField]
		private bool _isPlaying = false;
		[SerializeField]
		private bool _isForward = true;
		[SerializeField]
		private int _numIterations = 0;
		[SerializeField]
		private float _duration = 1.0f;
		[SerializeField]
		private float _delay = 0.0f;
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
		public bool IsPlaying { get { return _isPlaying; } }
		public bool IsForward { get { return _isForward; } }
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
		
		public PlaybackMode LoopMode {
			set { _loopMode = value; }
			get { return _loopMode; }
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
		protected virtual void OnEnable () {
			// resume playing the coroutine
			if (_isPlaying) {
				Play(_isForward);
			}
		}

		protected virtual void OnDisable () {
			InvalidateRoutine(); // stop the current coroutine since it will be canceled
		}

		private void OnValidate () {
			NumIterations = _numIterations;
			Duration = _duration;
			Delay = _delay;

			// play/pause depending on if it's active
			if (Application.isPlaying && isActiveAndEnabled) {
				if (_isPlaying) {
					Play(_isForward);
				} else {
					Pause();
				}
			}
		}
		#endregion

		#region Abstract and Virtual Methods
		protected abstract void PerformRelative ();

		public virtual void ApplyResult () { }
		public virtual void Swap () { }
		#endregion

		#region Actions
		// UnityEvent compatibility
		public void PlayForward (bool reset = false) {
			Play(true, reset, false);
		}

		// UnityEvent compatibility
		public void PlayForwardRelative(bool reset = false) {
			Play(true, reset, true);
		}

		// UnityEvent compatibility
		public void PlayReverse (bool reset = false) {
			Play(false, reset, false);
		}

		// UnityEvent compatibility
		public void PlayReverseRelative(bool reset = false) {
			Play(false, reset, true);
		}

		public Coroutine Play (bool forward = true, bool reset = false, bool relative = false) {
			if (_isPlaying && _processRoutine != null) { return null; } // don't process if already playing

			// set playback direction, and check if the tween should be adjusted relatively
			_isForward = forward;
			if (relative) {
				PerformRelative();
			}

			// handle reset logic
			if (reset) {
				if (_isForward) {
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

		public void Pause() {
			InvalidateRoutine();
			_isPlaying = false;
		}

		public void Stop () {
			InvalidateRoutine();
			Factor = 0.0f;
			_isPlaying = false;
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
			_currentTime += _isForward ? Time.deltaTime : -Time.deltaTime; // offset time based on playback direction

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
			if (_isForward) {
				return (_currentTime == Duration);
			} else {
				return (_currentTime == 0.0f) ;
			}
		}

		private bool PerformIncrementLooping () {
			bool finish = false;

			if (_isForward) {
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

			if (_isForward) {
				if (_currentTime >= Duration) {
					finish = Iterate();
					if (finish) {
						_currentTime = Duration;
					} else {
						_currentTime = Duration - (_currentTime - Duration);
					}

					_isForward = false;
				}
			} else {
				if (_currentTime <= 0.0f) {
					finish = Iterate();
					if (finish) {
						_currentTime = 0.0f;
					} else {
						_currentTime = -_currentTime;
					}

					_isForward = true;
				}
			}

			return finish;
		}

		private bool Iterate () {
			_onIterate.Invoke();
			return (NumIterations > 0 && (--IterationsLeft == 0)); // loop indefinitely if numIterations is zero
		}

		private void InvalidateRoutine () {
			if (_processRoutine == null) { return; }

			StopCoroutine(_processRoutine);
			_processRoutine = null;
		}

		private IEnumerator Process () {
			float delayTime = 0.0f;

			// check for a delay
			if (_delay > 0.0f) {
				ApplyResult();

				// run the delay upon playing
				while (delayTime < _delay) {
					delayTime += Time.deltaTime;
					yield return null;
				}
			}

			// start the tween
			_isPlaying = true;
			_onPlay.Invoke();

			// process the update loop
			while (!PerformIncrement()) {
				yield return null;
			}
			_isPlaying = false;

			// stop everything
			InvalidateRoutine();
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
		public override void Swap () {
			T temp = Begin;
			Begin = End;
			End = temp;
		}
		#endregion
	}
}
