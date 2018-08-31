/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2018
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TRUEStudios.Foundation.Core {
	public class MenuItems {
		#region Constants
		public const string PackageName = "Foundation.unitypackage";
		#endregion

		#region Methods
		[MenuItem("Game/Import Package")]
		public static void ImportPackage() {
			string packagePath = $"../Foundation/{PackageName}";
			AssetDatabase.ImportPackage(packagePath, false);
			Debug.Log("Imported package...");
		}

		[MenuItem("Game/Clear Preferences")]
		public static void ClearPreferences() {
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
		}

		[MenuItem("Game/Screenshot")]
		public static void TakeScreenshot() {
			const string SubDirectory = "Screenshots";
			if (!Directory.Exists(SubDirectory)) {
				Directory.CreateDirectory(SubDirectory);
			}

			// generate the timestamp, and take the screenshot
			var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
			ScreenCapture.CaptureScreenshot($"Screenshots/{timestamp}.png");
			Debug.Log("Screenshot saved");
		}
		#endregion
	}
}
