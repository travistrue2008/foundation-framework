/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TRUEStudios.Foundation.Tweens {
	[CustomEditor(typeof(Tween)), CanEditMultipleObjects]
	public abstract class TweenEditor<T> : Editor where T : Tween {
		#region Fields
		private SerializedProperty _isPlayingProperty;
		private SerializedProperty _isForwardProperty;

		private SerializedProperty _loopModeProperty;
		private SerializedProperty _numIterationsProperty;

		private SerializedProperty _durationProperty;
		private SerializedProperty _delayProperty;

		private SerializedProperty _distributionCurveProperty;
		private SerializedProperty _beginProperty;
		private SerializedProperty _endProperty;

		private SerializedProperty _onPlayProperty;
		private SerializedProperty _onFinishProperty;
		private SerializedProperty _onIterateProperty;
		private SerializedProperty _onUpdateProperty;
		#endregion

		#region Properties
		public bool ProvideCustomFields { protected set; get; }
		public T Reference { get { return (T)target; } }
		public SerializedProperty BeginProperty { get { return _beginProperty; } }
		public SerializedProperty EndProperty { get { return _endProperty; } }
		#endregion

		#region Abstract and Virtual Methods
		protected abstract void OnSetBegin (T target);
		protected abstract void OnSetEnd (T target);

		protected virtual void DrawAdditionalFields () { }
		protected virtual void DrawCustomBeginField () { }
		protected virtual void DrawCustomEndField () { }
		#endregion

		#region Editor Hooks
		protected virtual void OnEnable () {
			_isPlayingProperty = serializedObject.FindProperty("_isPlaying");
			_isForwardProperty = serializedObject.FindProperty("_isForward");

			_loopModeProperty = serializedObject.FindProperty("_loopMode");
			_numIterationsProperty = serializedObject.FindProperty("_numIterations");

			_durationProperty = serializedObject.FindProperty("_duration");
			_delayProperty = serializedObject.FindProperty("_delay");

			_distributionCurveProperty = serializedObject.FindProperty("_distributionCurve");
			_beginProperty = serializedObject.FindProperty("_begin");
			_endProperty = serializedObject.FindProperty("_end");

			_onPlayProperty = serializedObject.FindProperty("_onPlay");
			_onFinishProperty = serializedObject.FindProperty("_onFinish");
			_onIterateProperty = serializedObject.FindProperty("_onIterate");
			_onUpdateProperty = serializedObject.FindProperty("_onUpdate");
		}

		public override void OnInspectorGUI () {
			// update the serialized object
			serializedObject.Update();
			EditorGUILayout.BeginVertical();

			// draws additional fields defined by subclass
			DrawAdditionalFields();
			EditorGUILayout.Space();

			// draw properties
			DrawStateProperties();
			DrawLoopProperties();
			DrawDurationProperties();
			DrawAnimationProperties();
			DrawSetterButtons();
			DrawResetButtons();
			DrawEventProperties();

			EditorGUILayout.EndVertical();
			serializedObject.ApplyModifiedProperties();
		}
		#endregion

		#region Draw Methods
		private void DrawStateProperties () {
			// display state
			EditorGUILayout.PropertyField(_isPlayingProperty);
			EditorGUILayout.PropertyField(_isForwardProperty);
			EditorGUILayout.Space();
		}

		private void DrawLoopProperties () {
			// display loop
			EditorGUILayout.PropertyField(_loopModeProperty);
			EditorGUILayout.PropertyField(_numIterationsProperty);
			EditorGUILayout.Space();
		}

		private void DrawDurationProperties () {
			// display duration and delay
			EditorGUILayout.PropertyField(_durationProperty);
			EditorGUILayout.PropertyField(_delayProperty);
			EditorGUILayout.Space();
		}

		private void DrawAnimationProperties () {
			// check if the subclass should draw the begin and end fields
			if (ProvideCustomFields) {
				DrawCustomBeginField();
				DrawCustomEndField();
			} else {
				// draw default fields
				EditorGUILayout.PropertyField(BeginProperty);
				EditorGUILayout.PropertyField(EndProperty);
			}

			// display the distribution curve
			EditorGUILayout.PropertyField(_distributionCurveProperty);
			GUI.enabled = false;
			EditorGUILayout.Slider("Progress", Reference.Factor, 0.0f, 1.0f);
			GUI.enabled = true;
		}

		private void DrawSetterButtons () {
			EditorGUILayout.BeginHorizontal();

			// check if the swap button was pressed
			EditorGUI.BeginChangeCheck();
			GUILayout.Button("Set Begin");
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObjects(targets, "Set Begin");
				foreach (Object target in targets) {
					OnSetBegin((T)target);
				}
			}

			// check if the swap button was pressed
			EditorGUI.BeginChangeCheck();
			GUILayout.Button("Set End");
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObjects(targets, "Set End");
				foreach (Object target in targets) {
					OnSetEnd((T)target);
				}
			}

			// check if the swap button was pressed
			EditorGUI.BeginChangeCheck();
			GUILayout.Button("Swap");
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObjects(targets, "Swap Begin and End");
				foreach (Object target in targets) {
					((T)target).Swap();
				}
			}

			EditorGUILayout.EndHorizontal();
		}

		private void DrawResetButtons () {
			// convenience buttons
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("To Begin")) {
				foreach (Object target in targets) {
					((T)target).ResetToBegin();
				}
			}

			if (GUILayout.Button("To End")) {
				foreach (Object target in targets) {
					((T)target).ResetToEnd();
				}
			}
			
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}

		private void DrawEventProperties () {
			// display UnityEvent properties
			EditorGUILayout.PropertyField(_onPlayProperty);
			EditorGUILayout.PropertyField(_onFinishProperty);
			EditorGUILayout.PropertyField(_onIterateProperty);
			EditorGUILayout.PropertyField(_onUpdateProperty);
		}
		#endregion
	}
}
