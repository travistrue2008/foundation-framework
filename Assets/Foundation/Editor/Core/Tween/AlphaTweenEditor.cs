/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Net;
using System.Collections;
using System.Collections.Generic;


namespace Foundation.Framework
{
	[CustomEditor(typeof(AlphaTween))]
	[CanEditMultipleObjects]
	public class AlphaTweenEditor : TweenEditor<AlphaTween>
	{
		#region Methods
		protected override void OnEnable()
		{
			base.OnEnable();
			EnableSetButtons = false;
		}

		protected override void OnAdditionalFields()
		{
			Reference.TargetGraphic = (Graphic)EditorGUILayout.ObjectField("Target Graphic", Reference.TargetGraphic, typeof(Graphic), true);
			Reference.TargetSpriteRenderer = (SpriteRenderer)EditorGUILayout.ObjectField("Target Sprite Renderer", Reference.TargetSpriteRenderer, typeof(SpriteRenderer), true);
		}

		protected override void OnBeginField()
		{
			base.OnBeginField();
			Reference.Begin = EditorGUILayout.Slider("Begin", Reference.Begin, 0.0f, 1.0f);
		}

		protected override void OnEndField()
		{
			base.OnEndField();
			Reference.End = EditorGUILayout.Slider("End", Reference.End, 0.0f, 1.0f);
		}
		#endregion
	}
}
