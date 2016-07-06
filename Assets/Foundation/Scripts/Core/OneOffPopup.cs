/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using System;
using System.Collections;


namespace Foundation.Framework
{
	public class OneOffPopup : MonoBehaviour
	{
		#region Fields
		[SerializeField]
		private bool _attemptGC;
		[SerializeField]
		private string _popupPrefabName;
		#endregion


		#region Properties
		public string PopupPrefabName { set { _popupPrefabName = value; } get { return _popupPrefabName; } }
		#endregion


		#region Methods
		public void ShowPopup()
		{
			// load and push the popup specified by prefab
			Popup popup = Services.Get<PopupService>().PushPopup<Popup>(_popupPrefabName, false);
			if (popup != null)
				popup.PopupClosed += HandlePopupClosed;
			else
				throw new Exception("Couldn't push popup with prefab: " + _popupPrefabName);
		}

		private void HandlePopupClosed()
		{
			// release the prefab, and attempt to clean up memory
			Services.Get<PopupService>().ReleasePrefab(_popupPrefabName);
			if (_attemptGC)
				Services.Release();
		}
		#endregion
	}
}
