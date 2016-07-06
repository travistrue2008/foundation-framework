/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using UnityEngine;
using System.Collections;


namespace Foundation.Framework
{
	public class MinAttribute : PropertyAttribute
	{
		public readonly float minimum;


		public MinAttribute(float minimum)
		{
			this.minimum = minimum;
		}
	}


	public class MaxAttribute : PropertyAttribute
	{
		public readonly float maximum;


		public MaxAttribute(float maximum)
		{
			this.maximum = maximum;
		}
	}
}
