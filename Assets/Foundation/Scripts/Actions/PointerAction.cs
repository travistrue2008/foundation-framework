/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;


namespace Foundation.Framework
{
	[Serializable]
	public class PointerActionEvent : UnityEvent<Vector2> { }


	public class PointerAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
	{
		#region Fields
		[SerializeField]
		PointerActionEvent _onDown = new PointerActionEvent();
		[SerializeField]
		PointerActionEvent _onUp = new PointerActionEvent();
		[SerializeField]
		PointerActionEvent _onClick = new PointerActionEvent();

		private Vector2 _downPos = Vector2.zero;
		#endregion


		#region Methods
		public void OnPointerDown(PointerEventData eventData)
		{
			_downPos = Input.mousePosition;
			_onDown.Invoke(Input.mousePosition);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			Vector2 diff = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - _downPos;
			if(diff.sqrMagnitude < 0.001f)
				_onClick.Invoke(Input.mousePosition);
			_onUp.Invoke(Input.mousePosition);
		}

		public void OnPointerClick(PointerEventData e)
		{
			_onClick.Invoke(e.position);
		}
		#endregion
	}
}
