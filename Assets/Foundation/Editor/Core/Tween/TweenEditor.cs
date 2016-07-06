/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


namespace Foundation.Framework
{
	[CustomEditor(typeof(Tween))]
	[CanEditMultipleObjects]
	public class TweenEditor<T> : Editor
	where T : Tween
	{
		#region Properties
		public bool EnableTargetField { protected set; get; }
		public bool EnableSetButtons { protected set; get; }
		public T Reference { get { return (T)serializedObject.targetObject; } }
		#endregion


		#region Methods
		protected virtual void OnEnable()
		{
			// get the target
			if (Reference == null)
				Debug.LogWarning("Reference is null");
		}

		public override void OnInspectorGUI()
		{
			// make sure the target is set
			if (Reference == null)
			{
				Debug.LogError("Reference not set");
				return;
			}
			OnAdditionalFields(); // display additional fields provided by subclass

			// display the relative (needs work), and target fields
			EditorGUILayout.BeginVertical();
			{
				Reference.SetOnAwake = (Tween.SetMode)EditorGUILayout.EnumPopup("Set Mode On Awake", Reference.SetOnAwake);
				Reference.ActivateOnPress = EditorGUILayout.Toggle("Activate On Press", Reference.ActivateOnPress);
				Reference.ActivateOnClick = EditorGUILayout.Toggle("Activate On Click", Reference.ActivateOnClick);
				Reference.Relative = EditorGUILayout.Toggle("Relative", Reference.Relative);
				if (EnableTargetField)
					Reference.Target = (GameObject)EditorGUILayout.ObjectField("Target", Reference.Target, typeof(GameObject), true);
			}
			EditorGUILayout.EndVertical();

			// display state
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("State", GUILayout.MinWidth(32.0f));
				Reference.State = (TweenPlaybackState)EditorGUILayout.EnumPopup(Reference.State);
				GUI.enabled = Reference.State == TweenPlaybackState.Playing ? true : false;
				EditorGUILayout.LabelField("Forward", GUILayout.MinWidth(32.0f));
				Reference.PlayingForward = EditorGUILayout.Toggle(Reference.PlayingForward);
				GUI.enabled = true;
			}
			EditorGUILayout.EndHorizontal();

			// display loop mode
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Loop Mode", GUILayout.MinWidth(32.0f));
				Reference.LoopMode = (TweenLoopMode)EditorGUILayout.EnumPopup(Reference.LoopMode);
				GUI.enabled = Reference.LoopMode == TweenLoopMode.Looping ? true : false;
				EditorGUILayout.LabelField("Iterations", GUILayout.MinWidth(32.0f));
				Reference.NumIterations = EditorGUILayout.IntField(Reference.NumIterations);
				GUI.enabled = true;
			}
			EditorGUILayout.EndHorizontal();

			// display duration and delay
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Duration", GUILayout.MinWidth(32.0f));
				Reference.Duration = EditorGUILayout.FloatField(Reference.Duration);
				EditorGUILayout.LabelField("Delay", GUILayout.MinWidth(32.0f));
				Reference.Delay = EditorGUILayout.FloatField(Reference.Delay);
			}
			EditorGUILayout.EndHorizontal();

			// display the animation curve, and interpolation
			Reference.Curve = EditorGUILayout.CurveField("Animation", Reference.Curve);
			float factor = EditorGUILayout.Slider("Interpolation", Reference.Factor, 0.0f, 1.0f);
			if (factor != Reference.Factor)
				Reference.Factor = factor; // only update Factor, if a change has occurred

			// setup the "begin" tween field
			EditorGUILayout.BeginHorizontal();
			{
				OnBeginField();
				if (EnableSetButtons && GUILayout.Button("Set", GUILayout.Width(48.0f)))
				{
					OnBeginSet();
					Reference.Factor = 0.0f;
				}
			}
			EditorGUILayout.EndHorizontal();

			// setup the "end" tween field
			EditorGUILayout.BeginHorizontal();
			{
				OnEndField();
				if (EnableSetButtons && GUILayout.Button("Set", GUILayout.Width(48.0f)))
				{
					OnEndSet();
					Reference.Factor = 1.0f;
				}
			}
			EditorGUILayout.EndHorizontal();

			// convenience buttons
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Reset To Begin"))
				Reference.ResetToBegin();
			if (GUILayout.Button("Reset To End"))
				Reference.ResetToEnd();
			if (GUILayout.Button("Swap"))
				Reference.Swap();
			EditorGUILayout.EndHorizontal();

			// display UnityEvent properties
			EditorGUILayout.BeginVertical();
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_onPlay"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_onFinish"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_onUpdate"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_onIterate"));
			}
			EditorGUILayout.EndVertical();

			// apply properties, and UI changes
			serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(Reference);
		}
		#endregion


		#region Virtual Methods
		protected virtual void OnAdditionalFields() { }
		protected virtual void OnBeginField() { }
		protected virtual void OnEndField() { }
		protected virtual void OnBeginSet() { }
		protected virtual void OnEndSet() { }
		#endregion
	}
}
