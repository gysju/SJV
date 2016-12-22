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
	int firstsizeX = 1; int firstsizeY = 1;int secondSizeX = 1; int secondSizeY = 1;
	int firstRadius = 1;int secondRadius = 1;
	float timeBeforeNextWave = 10.0f;
	string name = "";

	List<WaveScriptableObject.UnitType> firstButtonsType = new List<WaveScriptableObject.UnitType>();
	List<WaveScriptableObject.UnitType> secondeButtonsType = new List<WaveScriptableObject.UnitType>();

	public void Init()
	{
		labelStyle = new GUIStyle ();
		setLabelStyle ();
		firstButtonsType.Capacity = 100;
		secondeButtonsType.Capacity = 100;
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
					DrawSquareTemplate (ref firstsizeX, ref firstsizeY, firstButtonsType );
				}
				else
				{
					DrawCircleTemplate (firstRadius, firstButtonsType );
				}
			}

			EndingTemplateType = (TemplateType) EditorGUILayout.EnumPopup ("Ending pattern : ", EndingTemplateType);

			if(EndingTemplateType != TemplateType.TemplateType_None)
			{

				if(EndingTemplateType == TemplateType.TemplateType_Square)
				{
					DrawSquareTemplate (ref secondSizeX, ref secondSizeY, secondeButtonsType );
				}
				else
				{
					DrawCircleTemplate (secondRadius, secondeButtonsType );
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

		AssetDatabase.CreateAsset ( wave,"Assets/Databases/Waves/" + name + ".asset"); //try catche
		AssetDatabase.SaveAssets ();
	}

	void setLabelStyle()
	{
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.fontSize = 20;
	}

	void DrawSquareTemplate( ref int sizeX, ref int sizeY, List<WaveScriptableObject.UnitType> unitType)
	{
		GUILayout.BeginHorizontal ();
		sizeX = EditorGUILayout.IntField("Size X : ",  sizeX); sizeY = EditorGUILayout.IntField("Size Y : ",  sizeY);
		GUILayout.EndHorizontal ();

		initButtonState (unitType);

		for( int y = 0; y < sizeY; y++ )
		{
			GUILayout.BeginHorizontal ();
			for( int x = 0; x < sizeX; x++ )
			{
				int index = x + (x * y);
				unitType[index] = checkButtonType ( unitType[index] );
			} 
			GUILayout.EndHorizontal ();
		}
	}

	void DrawCircleTemplate( int radius, List<WaveScriptableObject.UnitType> unitType)
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

	void initButtonState(List<WaveScriptableObject.UnitType> unitType)
	{
		if(unitType.Count == 0)
		{
			for (int i = 0; i < 100; i++) 
			{
				unitType.Add (WaveScriptableObject.UnitType.UnitType_None);
			}
		}
	}
}
