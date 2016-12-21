using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public partial class WaveManagerWindow : EditorWindow {

	GUIStyle labelStyle;
	enum TemplateType { TemplateType_None = 0, TemplateType_Square, TemplateType_Circle};

	TemplateType BeginingTemplateType = TemplateType.TemplateType_None;
	TemplateType EndingTemplateType = TemplateType.TemplateType_None;

	WaveScriptableObject wave = null;
	bool WaitPreviousWave;
	int sizeX = 1; int sizeY = 1;
	int radius = 1;
	float timeBeforeNextWave = 10.0f;
	string name = "";

	List<WaveScriptableObject.UnitType> buttonsType = new List<WaveScriptableObject.UnitType>();

	public void Init()
	{
		labelStyle = new GUIStyle ();
		setLabelStyle ();
		buttonsType.Capacity = 100;
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
			BeginingTemplateType = (TemplateType) EditorGUILayout.EnumPopup ("Begining pattern : ", BeginingTemplateType);

			if(BeginingTemplateType != TemplateType.TemplateType_None)
			{
				if(BeginingTemplateType == TemplateType.TemplateType_Square)
				{
					DrawSquareTemplate ( wave.spawners );
				}
				else
				{
					DrawCircleTemplate ( wave.spawners );
				}
			}

			EndingTemplateType = (TemplateType) EditorGUILayout.EnumPopup ("Ending pattern : ", EndingTemplateType);

			if(EndingTemplateType != TemplateType.TemplateType_None)
			{

				if(EndingTemplateType == TemplateType.TemplateType_Square)
				{
					DrawSquareTemplate ( wave.Destination );
				}
				else
				{
					DrawCircleTemplate ( wave.Destination );
				}
			}

			WaitPreviousWave = GUILayout.Toggle (WaitPreviousWave, "Wait the previous wave"); 
			timeBeforeNextWave = EditorGUILayout.FloatField ("Time before the next wave : ",timeBeforeNextWave);
			name = EditorGUILayout.TextField("Name : ", name);

			if (name != "" && GUILayout.Button(" Save wave "))
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
			
		}
	}

	void CreateWave ()
	{
		wave = ScriptableObject.CreateInstance<WaveScriptableObject>(); // check si ca detruit quand tu quitte la fenetre ?
	}

	void SaveWave()
	{
		//wave.spawners// set Initial pos 
		//wave.Destination// set destination pos

		wave.timeBeforeNextWave = timeBeforeNextWave;
		wave.waitPreviousWave = WaitPreviousWave;

		AssetDatabase.CreateAsset ( wave,"Assets/Databases/Waves/" + name + ".asset");
		AssetDatabase.SaveAssets ();
	}

	void setLabelStyle()
	{
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.fontSize = 20;
	}

	void DrawSquareTemplate(List<WaveScriptableObject.Info> infos)
	{
		GUILayout.BeginHorizontal ();
		sizeX = EditorGUILayout.IntField("Size X : ",  sizeX); sizeY = EditorGUILayout.IntField("Size Y : ",  sizeY);
		GUILayout.EndHorizontal ();

		for( int y = 0; y < sizeY; y++ )
		{
			GUILayout.BeginHorizontal ();
			for( int x = 0; x < sizeX; x++ )
			{
				//change button by state

				//if ( buttonsType[x + (x * y)] == null )
					//buttonsType[x + (x * y)] = new WaveScriptableObject.UnitType();	
				//checkButtonType ( buttonsType[x + (x * y)] );
			} 
			GUILayout.EndHorizontal ();
		}
	}

	void DrawCircleTemplate( List<WaveScriptableObject.Info> infos)
	{
		radius = EditorGUILayout.IntField("Radius : ",  radius);
	}

	void checkButtonType(WaveScriptableObject.UnitType unitType)
	{
		switch(unitType)
		{
			case WaveScriptableObject.UnitType.UnitType_None:
			if (GUILayout.Button ("X"))
				unitType = WaveScriptableObject.UnitType.UnitType_Tank;
			break;

			case WaveScriptableObject.UnitType.UnitType_Tank:
			if ( GUILayout.Button("T"))
				unitType = WaveScriptableObject.UnitType.UnitType_Drone;
			break;

			case WaveScriptableObject.UnitType.UnitType_Drone:
			if ( GUILayout.Button("D"))
				unitType = WaveScriptableObject.UnitType.UnitType_None;
			break;
		} 
	}
}
