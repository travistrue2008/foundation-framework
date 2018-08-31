﻿/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;

namespace TRUEStudios.Foundation.Actions {
	[RequireComponent(typeof(AudioSource))]
	public class SoundAction : MonoBehaviour {
		#region Fields
		public AudioClip defaultClip;

		private AudioSource _source;
		#endregion

		#region Methods
		private void Awake () {
			_source = GetComponent<AudioSource>();
		}

		public void Play () {
			Play(defaultClip);
		}

		public void Play (AudioClip clip) {
			_source.PlayOneShot(clip);
		}
		#endregion
	}
}
