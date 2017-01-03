using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public partial class WaveManagerWindow : EditorWindow {

	GUIStyle labelStyle;
	public enum TemplateType { TemplateType_None = 0, TemplateType_Square, TemplateType_Circle};

	WaveObject wave = null;

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
			wave.nextWaveWait = GUILayout.Toggle (wave.nextWaveWait, "Next wave has to wait"); wave.timeBeforeNextWave = EditorGUILayout.FloatField ("Time before the next wave : ",wave.timeBeforeNextWave);
			GUILayout.EndHorizontal();

			int newCount = Mathf.Max (0, EditorGUILayout.IntField ("size", wave.Spawns.Count));

			while (newCount < wave.Spawns.Count) 
			{
				wave.Spawns.RemoveAt ( wave.Spawns.Count - 1);
			}

			while (newCount > wave.Spawns.Count) 
			{
				wave.Spawns.Add ( ScriptableObject.CreateInstance<SpawnObject>() );
			}

			for(int i = 0; i < wave.Spawns.Count; i++)
			{
				if (wave.Spawns [i] == null)
					wave.Spawns [i] = ScriptableObject.CreateInstance<SpawnObject> ();
				wave.Spawns [i] = (SpawnObject)EditorGUILayout.ObjectField (wave.Spawns[i], typeof(SpawnObject), true);
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
		string path = EditorUtility.OpenFilePanel ("Wave", "Assets/Databases", "asset");
		if (path.StartsWith (Application.dataPath)) 
		{
			string relpath = path.Substring(Application.dataPath.Length - "Assets".Length);
			wave = AssetDatabase.LoadAssetAtPath(relpath, typeof(WaveObject)) as WaveObject;
			Debug.Log (wave);
		}
	}

	void CreateWave ()
	{
		wave = ScriptableObject.CreateInstance<WaveObject>();  // check si ca detruit quand tu quitte la fenetre ?
	}

	void SaveWave()
	{
		AssetDatabase.CreateFolder ( "Assets/Databases/Waves", wave.ObjectName);
		AssetDatabase.CreateAsset ( wave,"Assets/Databases/Waves/" + wave.ObjectName + "/" + wave.ObjectName + ".asset"); //try catche
		for( int i = 0; i < wave.Spawns.Count; i++ )
		{
			AssetDatabase.CreateAsset (wave.Spawns[i], "Assets/Databases/Waves/" + wave.ObjectName + "/" + "Spawn_" + i + ".asset");
		}
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