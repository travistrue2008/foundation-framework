/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Foundation.Framework
{
	[Serializable]
	public class CurrencyEvent : UnityEvent<int> { }


	public class CurrencyService : Service
	{
		#region Constants
		public const string CurrencyKey = "CurrencyAmount";
		#endregion


		#region Events
		[SerializeField]
		private CurrencyEvent _onBalanceChanged = new CurrencyEvent();
		#endregion


		#region Fields
		[SerializeField, Min(0)]
		private int _initialCurrencyBalance = 50;

		private int _balance = 0;
		#endregion


		#region Properties
		public CurrencyEvent BalanceChanged { get { return _onBalanceChanged; } }
		public int Balance
		{
			set
			{
				// get the new balance (bound by zero), and cache the old
				int oldBalance = _balance;
				int newBalance = Mathf.Max(0, value);

				// check if a change has occurred
				if (oldBalance != newBalance)
				{
					// update the current balance
					_balance = newBalance;
					PlayerPrefs.SetInt(CurrencyKey, _balance);
					PlayerPrefs.Save();
					_onBalanceChanged.Invoke(_balance); // signal an event if available
				}
			}

			get { return _balance; }
		}
		#endregion


		#region MonoBehaviour Overrides
		protected override void OnInitialize()
		{
			Balance = PlayerPrefs.GetInt(CurrencyKey, _initialCurrencyBalance);
		}
		#endregion


		#region Actions
		public void ResetCurrency()
		{
			Balance = _initialCurrencyBalance;
		}
		#endregion


		#region Menu Items
#if UNITY_EDITOR
		[MenuItem("Tools/Game/Currency/Reset Currency", true)]
		public static bool CanMenuResetCurrency()
		{
			return EditorApplication.isPlaying;
		}

		[MenuItem("Tools/Game/Currency/Reset Currency")]
		public static void MenuResetCurrency()
		{
			Services.Get<CurrencyService>().ResetCurrency();
		}

		[MenuItem("Tools/Game/Currency/Zero Currency", true)]
		public static bool CanMenuZeroCurrency()
		{
			return EditorApplication.isPlaying;
		}

		[MenuItem("Tools/Game/Currency/Zero Currency")]
		public static void MenuZeroCurrency()
		{
			Services.Get<CurrencyService>().Balance = 0;
		}
#endif
		#endregion
	}
}
