using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public partial class WaveManagerWindow : EditorWindow {

	GUIStyle labelStyle;
	public enum TemplateType { TemplateType_None = 0, TemplateType_Square, TemplateType_Circle};
	public List<WaveScriptableObject.Spawn> list = new List<WaveScriptableObject.Spawn>();

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

			var list = wave.Spawns;

			int newCount = Mathf.Max (0, EditorGUILayout.IntField ("size", list.Count));

			while (newCount < list.Count) 
			{
				list.RemoveAt ( list.Count - 1);
			}

			while (newCount > list.Count) 
			{
				list.Add ( ScriptableObject.CreateInstance<WaveScriptableObject.Spawn>() );
			}

			for(int i = 0; i < list.Count; i++)
			{
				list [i] = (WaveScriptableObject.Spawn)EditorGUILayout.ObjectField (list[i], typeof(WaveScriptableObject.Spawn), true);
			}

			if (wave.ObjectName != "" && GUILayout.Button(" Save wave "))
			{
				SaveWave();
			}
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

	void OnDrawGizmos( )
	{
		foreach(WaveScriptableObject.Spawn spawn in list)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere ( spawn.SpawnPosition, 1.0f);
			Gizmos.color = Color.green;
			Gizmos.DrawLine (spawn.SpawnPosition, spawn.AttackPosition);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere ( spawn.AttackPosition, 1.0f);
		}
	}

	void setLabelStyle()
	{
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.fontSize = 20;
	}

	void initInfoState(List<WaveScriptableObject.Spawn> infos)
	{
		if(infos.Count == 0)
		{
			for (int i = 0; i < 25; i++) 
			{
				infos.Add (new WaveScriptableObject.Spawn());
			}
		}
	}
}