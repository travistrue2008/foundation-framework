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
	[CustomEditor(typeof(PositionTween))]
	[CanEditMultipleObjects]
	public class PositionTweenEditor : TweenEditor<PositionTween>
	{
		#region Methods
		protected override void OnEnable()
		{
			base.OnEnable();
			EnableSetButtons = true;
		}

		protected override void OnAdditionalFields()
		{
			Reference.TransformSpace = (CoordinateSpace)EditorGUILayout.EnumPopup("Coordinate Space", Reference.TransformSpace);
		}

		protected override void OnBeginField()
		{
			Reference.Begin = EditorGUILayout.Vector3Field("Begin", Reference.Begin);
		}

		protected override void OnEndField()
		{
			Reference.End = EditorGUILayout.Vector3Field("End", Reference.End);
		}

		protected override void OnBeginSet()
		{
			switch (Reference.TransformSpace)
			{
				case CoordinateSpace.Local:
					Reference.Begin = Reference.transform.localPosition;
					break;

				case CoordinateSpace.Global:
					Reference.Begin = Reference.transform.position;
					break;
			}
		}

		protected override void OnEndSet()
		{
			switch (Reference.TransformSpace)
			{
				case CoordinateSpace.Local:
					Reference.End = Reference.transform.localPosition;
					break;

				case CoordinateSpace.Global:
					Reference.End = Reference.transform.position;
					break;
			}
		}
		#endregion
	}
}
