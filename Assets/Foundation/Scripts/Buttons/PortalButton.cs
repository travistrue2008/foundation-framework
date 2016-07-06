/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;


namespace Foundation.Framework
{
	public class PortalButton : GameButton
	{
		#region Fields
		[SerializeField]
		private float _duration = 0.5f;
		[SerializeField]
		private float _holdDuration = 0.0f;
		[SerializeField]
		private string _zoneTag;
		[SerializeField]
		private string _sceneName;
		[SerializeField]
		private GameObject _transitionPrefab;

		private bool _performingTransition = false;
		#endregion


		#region Properties
		public string ZoneTag { get { return _zoneTag; } }
		public string SceneName { get { return _sceneName; } }
		#endregion


		#region Methods
		private void OnDestroy()
		{
			// change the zone tag
			if (_performingTransition && Services.Get<SceneService>() != null)
				Services.Get<SceneService>().ZoneTag = ZoneTag;
		}

		protected override void OnClick(PointerEventData e)
		{
			// begin the transition, and log the event
			Services.Get<SceneService>().Transition<Transition>(_transitionPrefab, _sceneName, _duration, _holdDuration, false);
			_performingTransition = true;
		}
		#endregion
	}
}
