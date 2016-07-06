/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;


namespace Foundation.Framework
{
	public class MuteButton : GameButton
	{
		#region Fields
		[SerializeField]
		private Sprite _onSprite;
		[SerializeField]
		private Sprite _offSprite;

		private Image _image;
		#endregion


		#region Methods
		protected override void Awake()
		{
			base.Awake();
			_image = GetComponent<Image>();
			_image.sprite = Services.Get<AudioService>().SoundMuted ? _offSprite : _onSprite;
		}

		private void OnEnable()
		{
			Services.Get<AudioService>().SoundMuteStateChanged += HandleSoundMuteStateChanged;
		}

		private void OnDisable()
		{
			if (Services.Get<AudioService>() != null)
				Services.Get<AudioService>().SoundMuteStateChanged -= HandleSoundMuteStateChanged;
		}

		protected override void OnClick(PointerEventData e)
		{
			Services.Get<AudioService>().SoundMuted ^= true;
			Services.Get<AudioService>().MusicMuted ^= true;
			Services.Get<AudioService>().PlaySound(ClickClip);
		}

		private void HandleSoundMuteStateChanged(bool muted)
		{
			_image.sprite = muted ? _offSprite : _onSprite;
		}
		#endregion
	}
}
