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
	public class MinAttribute : PropertyAttribute
	{
		public readonly float min;

		public MinAttribute(float min)
		{
			this.min = min;
		}
	}
}
