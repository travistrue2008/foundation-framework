/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Foundation.Framework
{
	public class AudioService : Service
	{
		#region Constants
		public const int NumSources = 16;
		private const string SoundMuteKey = "MuteSound";
		private const string MusicMuteKey = "MuteMusic";
		#endregion


		#region Events
		public delegate void BoolDelegate(bool muted);
		public event BoolDelegate SoundMuteStateChanged;
		public event BoolDelegate MusicMuteStateChanged;
		#endregion


		#region Fields
		[SerializeField]
		private GameObject _sourceObj;

		private bool _soundMuted = false;
		private bool _musicMuted = false;
		private float _soundVolume = 1.0f;
		private float _musicVolume = 0.3f;
		private Coroutine _musicFadeRoutine;
		private AudioListener _listener;
		private AudioSource _musicSource;
		private AudioSource[] _sources = new AudioSource[NumSources];
		#endregion


		#region Properties
		public bool PlayingMusic { get { return (_musicSource != null) ? _musicSource.isPlaying : false; } }
		public AudioListener Listener { get { return _listener; } }

		public bool SoundMuted
		{
			set
			{
				// set the mute status, and save it to PlayerPrefs
				_soundMuted = value;
				PlayerPrefs.SetInt(SoundMuteKey, _soundMuted ? 1 : 0);
				PlayerPrefs.Save();

				// set each source's mute state
				foreach (AudioSource source in _sources)
					source.mute = _soundMuted;

				// raise mute event
				if (SoundMuteStateChanged != null)
					SoundMuteStateChanged(_soundMuted);
			}
			get { return _soundMuted; }
		}

		public bool MusicMuted
		{
			set
			{
				// set the mute status, and save it to PlayerPrefs
				_musicMuted = value;
				PlayerPrefs.SetInt(MusicMuteKey, _musicMuted ? 1 : 0);
				PlayerPrefs.Save();

				// set the music source's mute state
				if (_musicSource != null)
					_musicSource.mute = _musicMuted;

				// raise mute event
				if (MusicMuteStateChanged != null)
					MusicMuteStateChanged(_musicMuted);
			}
			get { return _musicMuted; }
		}

		public float SoundVolume
		{
			set
			{
				_soundVolume = Mathf.Clamp01(value);
				foreach (AudioSource source in _sources)
					source.volume = _soundVolume;
			}
			get { return _soundVolume; }
		}

		public float SoundMusic
		{
			set
			{
				_musicVolume = Mathf.Clamp01(value);
				if (_musicSource != null)
					_musicSource.volume = _musicVolume;
			}
			get { return _musicVolume; }
		}
		#endregion


		#region Methods
		protected override void OnInitialize()
		{
			// allocate AudioListener, and AudioSource components for the music and sound source references
			_listener = _sourceObj.AddComponent<AudioListener>();
			_musicSource = _sourceObj.AddComponent<AudioSource>();
			for (int i = 0; i < _sources.Length; ++i)
				_sources[i] = _sourceObj.AddComponent<AudioSource>();

			// get the mute properties' initial values from PlayerPrefs
			SoundMuted = Convert.ToBoolean(PlayerPrefs.GetInt(SoundMuteKey, 0));
			MusicMuted = Convert.ToBoolean(PlayerPrefs.GetInt(MusicMuteKey, 0));
		}

		public void PlayOneOffSound(AudioClip clip)
		{
			PlaySound(clip, false);
		}

		public AudioSource PlaySound(AudioClip clip, bool looping = false, float pitch = 1.0f)
		{
			// return early if muted
			if (SoundMuted)
				return null;

			// get an available AudioSource
			AudioSource source = GetAvailableSource();
			if (clip != null)
			{
				source.clip = clip;
				source.loop = looping;
				source.volume = _soundVolume;
				source.pitch = pitch;

				// play and begin a coroutine that checks when the sound is finished
				source.Play();
				StartCoroutine(ClearOnFinish(source));
			}
			return source;
		}

		public void StopSound(AudioClip clip)
		{
			foreach (AudioSource source in _sources)
			{
				// check for matching clip
				if (source.clip == clip)
				{
					// stop and clear reference
					source.Stop();
					source.clip = null;
				}
			}
		}

		public void StopMusic(float fadeDuration = 0.0f)
		{
			// make sure the music AudioSource is valid
			if (_musicSource != null)
			{
				// fade, or stop immediately
				if (fadeDuration > 0.0f)
					StartCoroutine(ProcessMusicFade(fadeDuration));
				else
					_musicSource.Stop();
			}
		}

		public void Stop()
		{
			// stop any fade transitions
			if (_musicFadeRoutine != null)
			{
				StopCoroutine(_musicFadeRoutine);
				_musicFadeRoutine = null;
			}

			// stop all sound and music sources
			foreach (AudioSource source in _sources)
				source.Stop();
			StopMusic();
		}

		public AudioSource FindSource(AudioClip clip)
		{
			// loop through the pool
			foreach (AudioSource source in _sources)
			{
				// check if the current source is using the clip
				if (source.clip == clip)
					return source;
			}
			return null;
		}

		private IEnumerator ClearOnFinish(AudioSource source)
		{
			// make sure the source is valid
			if (source != null)
			{
				// run an update loop until the sound is done playing
				while (source.isPlaying)
					yield return null;
				source.clip = null;
				yield break;
			}
		}

		public void PlayMusic(AudioClip clip, float pitch = 1.0f)
		{
			if (_musicSource != null)
			{
				_musicSource.clip = clip;
				_musicSource.loop = true;
				_musicSource.pitch = pitch;
				_musicSource.volume = _musicVolume;
				_musicSource.Play();
			}
		}

		private AudioSource GetAvailableSource()
		{
			// look for a source with no clip assigned to it in the pool
			foreach (AudioSource source in _sources)
			{
				if (source.clip == null)
					return source;
			}
			return null;
		}

		private IEnumerator ProcessMusicFade(float duration)
		{
			float currentTime = 0.0f;
			float startVolume = _musicVolume;

			// fade over time
			while (_musicSource != null && currentTime < duration)
			{
				currentTime = Mathf.Clamp(currentTime + Time.deltaTime, 0.0f, duration);
				_musicSource.volume = startVolume - startVolume * (currentTime / duration);
				yield return null;
			}

			StopMusic();
		}
		#endregion


		#region Menu Items
#if UNITY_EDITOR
		[MenuItem("Tools/Game/Audio/Toggle Sound", true)]
		public static bool CanToggleSound()
		{
			return EditorApplication.isPlaying;
		}

		[MenuItem("Tools/Game/Audio/Toggle Sound")]
		public static void ToggleSound()
		{
			Services.Get<AudioService>().SoundMuted ^= true;
		}

		[MenuItem("Tools/Game/Audio/Toggle Music", true)]
		public static bool CanToggleMusic()
		{
			return EditorApplication.isPlaying;
		}

		[MenuItem("Tools/Game/Audio/Toggle Music")]
		public static void ToggleMusic()
		{
			Services.Get<AudioService>().MusicMuted ^= true;
		}
#endif
		#endregion
	}
}
