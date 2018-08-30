using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour {
	#region Fields
	[SerializeField]
	private TextMeshProUGUI _hpLabel;
	#endregion

	#region Properties
	public float HP { set; get; }
	#endregion

	#region Methods
	private void Awake () {
		HP = 520;
	}

	private void Update () {
		_hpLabel.text = $"HP: {HP:N0}";
	}
	#endregion

	#region Actions
	public void Increment (int amount) {
		HP += amount;
	}
	#endregion
}
