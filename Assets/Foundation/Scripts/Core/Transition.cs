/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;


namespace Foundation.Framework
{
	[Serializable]
	public class TransitionEvent : UnityEvent<float> { }


	public class Transition : MonoBehaviour
	{
		#region Fields
		[SerializeField]
		private UnityEvent _onBegin = new UnityEvent();
		[SerializeField]
		private UnityEvent _onFinish = new UnityEvent();
		[SerializeField]
		private TransitionEvent _onUpdate = new TransitionEvent();
		#endregion


		#region Properties
		public float Progress { set; get; }
		public Tween Tween { private set; get; }
		#endregion


		#region Methods
		protected virtual void OnLoadFrame(float progress) { }

		protected virtual void Awake()
		{
			Tween = GetComponent<Tween>();
			_onBegin.Invoke();
		}

		protected virtual void OnDestroy()
		{
			_onFinish.Invoke();
		}

		public void UpdateLoadFrame(float progress)
		{
			Progress = progress;
			OnLoadFrame(progress);
			_onUpdate.Invoke(progress);
		}
		#endregion
	}
}
