/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEditor;
using System.Net;
using System.Collections;
using System.Collections.Generic;


namespace Foundation.Framework
{
	[CustomEditor(typeof(ColorTween))]
	[CanEditMultipleObjects]
	public class ColorTweenEditor : TweenEditor<ColorTween>
	{
		#region Methods
		protected override void OnEnable()
		{
			base.OnEnable();
			EnableSetButtons = false;
		}
		#endregion
	}
}
