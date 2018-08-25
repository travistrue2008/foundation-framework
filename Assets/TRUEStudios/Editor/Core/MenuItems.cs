﻿/******************************************************************************
 * Foundation Framework
 * Created by: Travis J True, 2016
 * This framework is free to use with no limitations.
******************************************************************************/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuItems {
	[MenuItem("Game/Clear Preferences")]
	public static void ClearPreferences () {
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}

	[MenuItem("Game/Screenshot")]
	public static void TakeScreenshot () {
		const string SubDirectory = "Screenshots";
		if (!Directory.Exists(SubDirectory)) {
			Directory.CreateDirectory(SubDirectory);
		}

		// generate the timestamp, and take the screenshot
		var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
		ScreenCapture.CaptureScreenshot($"Screenshots/{timestamp}.png");
		Debug.Log("Screenshot saved");
	}
}
