using UnityEngine;
using System.Collections;
using UnityEditor;

public partial class WaveManagerWindow : EditorWindow 
{
	[MenuItem("Window/Wave Manager")]
	public static void ShowWindow()
	{
		var window = GetWindow<WaveManagerWindow>();
		window.Init();
		window.Show();
	}
}