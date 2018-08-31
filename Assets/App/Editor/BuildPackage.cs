using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TRUEStudios.Foundation.Core;

public static class BuildPackage {
	#region Methods
	[MenuItem("Game/Export Package")]
	public static void ExportPackage() {
		var assetPathNames = new string[] { "Assets/TRUEStudios" };
		AssetDatabase.ExportPackage(assetPathNames, MenuItems.PackageName, ExportPackageOptions.Recurse);
		Debug.Log("Exported package...");
	}
	#endregion
}
