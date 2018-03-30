/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using UnityEngine;


namespace TRUEStudios.Core
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class MaxAttribute : PropertyAttribute
	{
		public readonly float max;

		public MaxAttribute(float max)
		{
			this.max = max;
		}
	}
}
