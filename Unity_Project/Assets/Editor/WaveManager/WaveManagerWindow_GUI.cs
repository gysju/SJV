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
			wave.SpawnerType = (TemplateType) EditorGUILayout.EnumPopup ("Begining pattern : ", wave.SpawnerType);

			if(wave.SpawnerType != TemplateType.TemplateType_None)
			{
				if(wave.SpawnerType == TemplateType.TemplateType_Square)
				{
					DrawSquareTemplate (ref wave.SpawnSizeX, ref wave.SpawnSizeY, ref wave.spawners );
				}
				else
				{
					DrawCircleTemplate (ref wave.SpawnSizeX, wave.spawners );
				}
				wave.DistanceBetweenSpawnerPoint = EditorGUILayout.FloatField("Distance between Spawner point ", wave.DistanceBetweenSpawnerPoint); 
			}

			GUILayout.Space (10);

			wave.DestinationType = (TemplateType) EditorGUILayout.EnumPopup ("Ending pattern : ", wave.DestinationType);

			if(wave.DestinationType != TemplateType.TemplateType_None)
			{

				if(wave.DestinationType == TemplateType.TemplateType_Square)
				{
					DrawSquareTemplate (ref wave.DestinationSizeX, ref wave.DestinationSizeY, ref wave.Destination );
				}
				else
				{
					DrawCircleTemplate (ref wave.DestinationSizeX, wave.Destination );
				}
				wave.DistanceBetweenDestinationPoint = EditorGUILayout.FloatField("Distance between destination point ", wave.DistanceBetweenDestinationPoint);
			}

			GUILayout.Space (10);

			wave.waitPreviousWave = GUILayout.Toggle (wave.waitPreviousWave, "Wait the previous wave"); 
			wave.timeBeforeNextWave = EditorGUILayout.FloatField ("Time before the next wave : ",wave.timeBeforeNextWave);
			wave.ObjectName = EditorGUILayout.TextField("Name : ", wave.ObjectName);

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
		wave.spawners.RemoveAll (x => x.type == WaveScriptableObject.UnitType.UnitType_None);// set Initial pos 
		wave.Destination.RemoveAll( x => x.type == WaveScriptableObject.UnitType.UnitType_None);// set destination pos

		wave.spawners.Capacity = wave.spawners.Count;
		wave.Destination.Capacity = wave.Destination.Count;

		AssetDatabase.CreateAsset ( wave,"Assets/Databases/Waves/" + wave.ObjectName + ".asset"); //try catche
		AssetDatabase.SaveAssets ();
	}

	void setLabelStyle()
	{
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.fontSize = 20;
	}

	void DrawSquareTemplate( ref int sizeX, ref int sizeY,ref List<WaveScriptableObject.Info> infos)
	{
		GUILayout.BeginHorizontal ();
		sizeX = EditorGUILayout.IntField("Size X : ",  sizeX); sizeY = EditorGUILayout.IntField("Size Y : ",  sizeY);
		GUILayout.EndHorizontal ();

		initInfoState (infos);

		for( int y = 0; y < sizeY; y++ )
		{
			GUILayout.BeginHorizontal ();
			for( int x = 0; x < sizeX; x++ )
			{
				int index = x + (sizeX * y);
				infos[index].type = checkButtonType ( infos[index].type );
				if (infos [index].type != WaveScriptableObject.UnitType.UnitType_None) 
				{
					infos [index].PosX = x;
					infos [index].PosY = y;
				}	
			} 
			GUILayout.EndHorizontal ();
		}
	}

	void DrawCircleTemplate( ref int radius, List<WaveScriptableObject.Info> infos)
	{
		radius = EditorGUILayout.IntField("Radius : ",  radius);
	}

	WaveScriptableObject.UnitType checkButtonType(WaveScriptableObject.UnitType unitType)
	{
		switch(unitType)
		{
			case WaveScriptableObject.UnitType.UnitType_None:
			if (GUILayout.Button ("X"))
				return WaveScriptableObject.UnitType.UnitType_Tank;
			break;

			case WaveScriptableObject.UnitType.UnitType_Tank:
			if ( GUILayout.Button("T"))
				return WaveScriptableObject.UnitType.UnitType_Drone;
			break;

			case WaveScriptableObject.UnitType.UnitType_Drone:
			if ( GUILayout.Button("D"))
				return WaveScriptableObject.UnitType.UnitType_None;
			break;
		} 
		return unitType;
	}

	void initInfoState(List<WaveScriptableObject.Info> infos)
	{
		if(infos.Count == 0)
		{
			for (int i = 0; i < 25; i++) 
			{
				infos.Add (new WaveScriptableObject.Info());
			}
		}
	}
}
