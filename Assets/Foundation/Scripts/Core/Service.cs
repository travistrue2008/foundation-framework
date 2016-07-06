/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using System.Collections;


namespace Foundation.Framework
{
	public class Service : MonoBehaviour
	{
		#region Methods
		private bool _initialized = false;

		// Services use OnInitialize() instead of Awake() to prevent multiple instances of
		// Service components from initializing during the development cycle when an instance
		// of the Services prefab may exist in multiple scenes for direct-scene testing.
		protected virtual void OnInitialize() { }

		public void Initialize()
		{
			// only process sub-classed initialization if it hasn't been initialized already
			if (!_initialized)
			{
				OnInitialize();
				_initialized = true;
			}
		}


		// Prevent from overriding... There's probably a better way to do this. One day, one day...
		private void Awake() { }
		private void Start() { }
		private void OnLevelWasLoaded(int level) { }
		#endregion
	}
}
