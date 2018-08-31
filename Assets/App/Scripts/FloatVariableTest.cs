using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TRUEStudios.Foundation.Variables;

public class FloatVariableTest : MonoBehaviour {
	#region Fields
	[SerializeField]
	private FloatReference _testReference;
	#endregion

	#region Properties
	public FloatReference TestReference { get { return _testReference; } }
	#endregion
}
