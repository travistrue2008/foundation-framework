/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;


namespace Foundation.Framework
{
	public class DragToResponder : MonoBehaviour
	{
		#region Fields
		[SerializeField]
		private UnityEvent _onResponse = new UnityEvent();
		#endregion


		#region Methods
		public void PerformTrigger()
		{
			_onResponse.Invoke();
		}
		#endregion
	}
}
