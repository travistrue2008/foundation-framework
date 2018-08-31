/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TRUEStudios.Foundation.Core;

namespace TRUEStudios.Foundation.Tweens {
	[CustomEditor(typeof(PositionTween)), CanEditMultipleObjects]
	public class PositionTweenEditor : TweenEditor<PositionTween> {
		#region Fields
		private SerializedProperty _coordinateSpaceProperty;
		#endregion

		#region Override Methods
		protected override void OnSetBegin (PositionTween target) {
			switch (target.TransformSpace) {
				case CoordinateSpace.Local:
				target.Begin = target.transform.localPosition;
				break;

				case CoordinateSpace.Global:
				target.Begin = target.transform.position;
				break;
			}
		}

		protected override void OnSetEnd (PositionTween target) {
			switch (target.TransformSpace) {
				case CoordinateSpace.Local:
				target.End = target.transform.localPosition;
				break;

				case CoordinateSpace.Global:
				target.End = target.transform.position;
				break;
			}
		}

		protected override void DrawAdditionalFields () {
			EditorGUILayout.PropertyField(_coordinateSpaceProperty);
		}
		#endregion

		#region Methods
		protected override void OnEnable () {
			base.OnEnable();
			_coordinateSpaceProperty = serializedObject.FindProperty("_transformSpace");
		}
		#endregion
	}
}
