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
	[CustomEditor(typeof(ScaleTween))]
	[CanEditMultipleObjects]
	public class ScaleTweenEditor : TweenEditor<ScaleTween>
	{
		#region Methods
		protected override void OnEnable()
		{
			base.OnEnable();
			EnableSetButtons = true;
		}

		protected override void OnBeginField()
		{
			base.OnBeginField();
			Reference.Begin = EditorGUILayout.Vector3Field("Begin", Reference.Begin);
		}

		protected override void OnEndField()
		{
			base.OnEndField();
			Reference.End = EditorGUILayout.Vector3Field("End", Reference.End);
		}

		protected override void OnBeginSet()
		{
			base.OnBeginSet();
			Reference.Begin = Reference.transform.localScale;
		}

		protected override void OnEndSet()
		{
			base.OnEndSet();
			Reference.End = Reference.transform.localScale;
		}
		#endregion
	}
}
