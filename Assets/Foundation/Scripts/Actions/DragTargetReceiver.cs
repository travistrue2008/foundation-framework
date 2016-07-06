/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using System.Collections;


namespace Foundation.Framework.Actions
{
	[RequireComponent(typeof(Collider2D))]
	public class DragTargetReceiver : MonoBehaviour
	{
		#region Methods
		private void OnCollisionEnter2D(Collision2D collision)
		{
			Debug.Log(collision.gameObject.name + " entered collision on receiver: " + name);
		}
		#endregion
	}
}
