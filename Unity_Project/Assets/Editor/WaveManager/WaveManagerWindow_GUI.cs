using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public partial class WaveManagerWindow : EditorWindow {

	GUIStyle labelStyle;
	public enum TemplateType { TemplateType_None = 0, TemplateType_Square, TemplateType_Circle};

	WaveScriptableObject wave = null;

	public void Init()
	{
		labelStyle = new GUIStyle ();
		setLabelStyle ();
	}

	void OnGUI()
	{
		GUILayout.Space (10);

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Wave Manager", labelStyle);
		GUILayout.EndHorizontal ();

		GUILayout.Space (10);

		GUILayout.BeginHorizontal ();
		if(GUILayout.Button("Create a new Wave"))
		{
			CreateWave ();
		}
		if (GUILayout.Button("Edit a Wave"))
		{
			EditWave();
		}
		GUILayout.EndHorizontal ();

		if (wave != null) 
		{
			wave.ObjectName = EditorGUILayout.TextField("Name : ", wave.ObjectName);
			GUILayout.BeginHorizontal ();
			wave.waitPreviousWave = GUILayout.Toggle (wave.waitPreviousWave, "Wait the previous wave"); wave.timeBeforeNextWave = EditorGUILayout.FloatField ("Time before the next wave : ",wave.timeBeforeNextWave);
			GUILayout.EndHorizontal();

			int newCount = Mathf.Max (0, EditorGUILayout.IntField ("size", wave.Spawns.Count));

			while (newCount < wave.Spawns.Count) 
			{
				wave.Spawns.RemoveAt ( wave.Spawns.Count - 1);
			}

			while (newCount > wave.Spawns.Count) 
			{
				wave.Spawns.Add ( ScriptableObject.CreateInstance<WaveScriptableObject.Spawn>() );
			}

			for(int i = 0; i < wave.Spawns.Count; i++)
			{
				if (wave.Spawns [i] == null)
					wave.Spawns [i] = ScriptableObject.CreateInstance<WaveScriptableObject.Spawn> ();
				wave.Spawns [i] = (WaveScriptableObject.Spawn)EditorGUILayout.ObjectField (wave.Spawns[i], typeof(WaveScriptableObject.Spawn), true);
			}
			GUILayout.BeginHorizontal ();

			if (GUILayout.Button(" Preview mode"))
			{
				Preview();
			}
			if (wave.ObjectName != "" && GUILayout.Button(" Save wave "))
			{
				SaveWave();
			}
			GUILayout.EndHorizontal ();
		}
	}

	void EditWave()
	{
		string path = EditorUtility.OpenFilePanel ("Wave", "", "");
		if (path.StartsWith (Application.dataPath)) 
		{
			string relpath = path.Substring(Application.dataPath.Length - "Assets".Length);
			wave = AssetDatabase.LoadAssetAtPath(relpath, typeof(WaveScriptableObject)) as WaveScriptableObject;
		}
	}

	void CreateWave ()
	{
		wave = ScriptableObject.CreateInstance<WaveScriptableObject>(); // check si ca detruit quand tu quitte la fenetre ?
	}

	void SaveWave()
	{
		AssetDatabase.CreateAsset ( wave,"Assets/Databases/Waves/" + wave.ObjectName + ".asset"); //try catche
		AssetDatabase.SaveAssets ();
	}
		
	void setLabelStyle()
	{
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.fontSize = 20;
	}
		
	void Preview( )
	{
		WaveHelper.ClearDraw ();
		WaveHelper.DrawWave (wave.Spawns);
	}

	void OnDestroy()
	{
		WaveHelper.ClearDraw ();
	}
}