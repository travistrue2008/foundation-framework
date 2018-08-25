using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour {
	#region Fields
	[SerializeField]
	private TextMeshProUGUI _hpLabel;
	[SerializeField]
	private TextMeshProUGUI _intTestLabel;
	#endregion

	#region Properties
	public int IntValue { set; get; }
	public float HP { set; get; }
	#endregion

	#region Methods
	private void Awake () {
		HP = 520;
	}

	private void Update () {
		_hpLabel.text = $"HP: {HP:N0}";
		_intTestLabel.text = $"INT: {IntValue}";
	}
	#endregion

	#region Actions
	public void Increment (int amount) {
		HP += amount;
	}
	#endregion
}
