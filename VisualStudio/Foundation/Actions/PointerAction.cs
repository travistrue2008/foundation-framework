/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TRUEStudios.Foundation.Actions {
	[Serializable]
	public class PointerActionEvent : UnityEvent<Vector2> { }

	public class PointerAction : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler {
		#region Fields
		[SerializeField]
		private PointerActionEvent _onUp = new PointerActionEvent();
		[SerializeField]
		private PointerActionEvent _onDown = new PointerActionEvent();
		[SerializeField]
		private PointerActionEvent _onClick = new PointerActionEvent();

		private Vector2 _downPos = Vector2.zero;
		#endregion

		#region Properties
		public PointerActionEvent OnUp { get { return _onUp; } }
		public PointerActionEvent OnDown { get { return _onDown; } }
		public PointerActionEvent OnClick { get { return _onClick; } }
		#endregion

		#region Methods
		public void OnPointerUp (PointerEventData eventData) {
			Vector2 diff = eventData.position - _downPos;
			if (diff.sqrMagnitude > Mathf.Epsilon) {
				_onUp.Invoke(UnityEngine.Input.mousePosition);
			} else {
				_onClick.Invoke(UnityEngine.Input.mousePosition);
			}
		}

		public void OnPointerDown (PointerEventData eventData) {
			_downPos = UnityEngine.Input.mousePosition;
			_onDown.Invoke(UnityEngine.Input.mousePosition);
		}

		public void OnPointerClick (PointerEventData e) {
			_onClick.Invoke(e.position);
		}
		#endregion
	}
}
