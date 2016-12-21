using UnityEngine;
using System.Collections;
using UnityEditor;

public partial class WaveManagerWindow : EditorWindow {

	GUIStyle labelStyle;
	enum TemplateType { TemplateType_None = 0, TemplateType_Square, TemplateType_Circle};

	TemplateType BeginingTemplateType = TemplateType.TemplateType_Square;
	TemplateType EndingTemplateType = TemplateType.TemplateType_Square;

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
			BeginingTemplateType = (TemplateType) EditorGUILayout.EnumPopup ("Begining pattern : ", BeginingTemplateType);
			EndingTemplateType = (TemplateType) EditorGUILayout.EnumPopup ("Ending pattern : ", EndingTemplateType);

			if(BeginingTemplateType != TemplateType.TemplateType_None)
			{
				
			}

			if(EndingTemplateType != TemplateType.TemplateType_None)
			{

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
		
	}

	void SaveWave( WaveScriptableObject wave)
	{
		AssetDatabase.CreateAsset ( wave,"Assets/Waves/Wave.asset");
		AssetDatabase.SaveAssets ();
	}

	void setLabelStyle()
	{
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.fontSize = 20;
	}
}
