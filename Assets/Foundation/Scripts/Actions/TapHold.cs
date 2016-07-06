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
	[RequireComponent(typeof(Collider2D))]
	public class TapHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		#region
		[SerializeField]
		private UnityEvent _onDown = new UnityEvent();
		[SerializeField]
		private UnityEvent _onUp = new UnityEvent();
		#endregion


		#region Methods
		public void OnPointerDown(PointerEventData eventData)
		{
			_onDown.Invoke();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			_onUp.Invoke();
		}
		#endregion
	}
}
