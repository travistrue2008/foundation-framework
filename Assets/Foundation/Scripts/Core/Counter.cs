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
	public class CounterIntEvent : UnityEvent<int> { }
	[Serializable]
	public class CounterStringEvent : UnityEvent<string> { }


	public class Counter : MonoBehaviour
	{

		#region Fields
		[SerializeField]
		private int _targetValue = 0;
		[SerializeField]
		private UnityEvent _onTargetReached = new UnityEvent();
		[SerializeField]
		private CounterIntEvent _onIntValueChanged = new CounterIntEvent();
		[SerializeField]
		private CounterStringEvent _onStringValueChanged = new CounterStringEvent();

		private int _value = 0;
		#endregion


		#region Properties
		public int Value
		{
			set
			{
				int oldValue = _value;
				_value = value;

				if (oldValue != _value)
				{
					_onIntValueChanged.Invoke(_value);
					_onStringValueChanged.Invoke(_value.ToString());
				}

				if (_value == _targetValue)
					_onTargetReached.Invoke();
			}
			get { return _value; }
		}
		#endregion


		#region Methods
		public void OffsetValue(int offset)
		{
			Value += offset;
		}

		public void IncrementValue()
		{
			++Value;
		}

		public void DecrementValue()
		{
			--Value;
		}

		public void Reset()
		{
			Value = 0;
		}
		#endregion
	}
}
