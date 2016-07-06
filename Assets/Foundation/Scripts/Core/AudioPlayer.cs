/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using System.Collections;


namespace Foundation.Framework
{
	public class AudioPlayer : MonoBehaviour
	{
		private enum ClipType
		{
			Sound,
			Music
		}

		private enum StopPolicy
		{
			DoNotStop,
			StopOnSceneChange,
			StopOnSpecificSceneChange,
			StopOnDestroy
		}


		#region Constants
		private float FadeDuration = 0.5f;
		#endregion


		#region Field
		[SerializeField]
		private bool _loopSound = false;
		[SerializeField]
		private bool _overrideMusic = true;
		[SerializeField]
		private bool _playOnAwake = true;
		[SerializeField]
		private ClipType _type;
		[SerializeField]
		private StopPolicy _stopPolicy;
		[SerializeField]
		private string _stopSceneName;
		[SerializeField]
		private AudioClip _clip;
		#endregion


		#region Properties
		public AudioClip Clip { set { _clip = value; } get { return _clip; } }
		public AudioSource Source { get { return Services.Get<AudioService>().FindSource(_clip); } }
		#endregion


		#region Methods
		private void Awake()
		{
			if (_playOnAwake)
				Play();
		}

		private void OnDestroy()
		{
			if (_stopPolicy == StopPolicy.StopOnDestroy && Services.Get<AudioService>() != null)
				Stop();
		}

		private void OnEnable()
		{
			Services.Get<SceneService>().OnTransitionStarted.AddListener(HandleSceneChange);
		}

		private void OnDisable()
		{
			if (Services.Get<SceneService>() != null)
				Services.Get<SceneService>().OnTransitionStarted.RemoveListener(HandleSceneChange);
		}

		private void HandleSceneChange(string sceneName)
		{
			// stop audio based on policy
			switch (_stopPolicy)
			{
				case StopPolicy.DoNotStop:
					break; // do nothing

				case StopPolicy.StopOnSceneChange:
					Stop();
					break; // always stop

				case StopPolicy.StopOnSpecificSceneChange:
					if (sceneName == _stopSceneName)
						Stop();
					break;
			}
		}

		public void Play()
		{
			// play as either a sound or music
			switch (_type)
			{
				case ClipType.Sound:
					Services.Get<AudioService>().PlaySound(_clip, _loopSound);
					break;

				case ClipType.Music:
					// check if already playing music
					if (Services.Get<AudioService>().PlayingMusic)
					{
						// check if it's ok to override current music being played
						if (_overrideMusic)
							Services.Get<AudioService>().PlayMusic(_clip);
					}
					else
						Services.Get<AudioService>().PlayMusic(_clip); // play the music
					break;
			}
		}

		public void Stop()
		{
			// play as either a sound or music
			switch (_type)
			{
				case ClipType.Sound:
					Services.Get<AudioService>().StopSound(_clip);
					break;

				case ClipType.Music:
					Services.Get<AudioService>().StopMusic(FadeDuration);
					break;
			}
		}
		#endregion
	}
}
