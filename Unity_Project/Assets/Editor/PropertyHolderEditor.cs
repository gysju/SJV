using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(setByLocalisation)), CanEditMultipleObjects]
public class PropertyHolderEditor : Editor 
{
	public SerializedProperty 
	root_Prop, 
	gameplay_Prop,
	menu_Prop, 
	setupScene_Prop, 
	intro_Prop,
	titleText_Prop,
	descriptionText_Prop,
	textButton_Prop;

	void OnEnable() //setup
	{
		root_Prop = serializedObject.FindProperty ("root");
		gameplay_Prop = serializedObject.FindProperty ("gameplay");
		menu_Prop = serializedObject.FindProperty ("menu");
		setupScene_Prop = serializedObject.FindProperty ("setupScene");
		intro_Prop = serializedObject.FindProperty ("intro");
		titleText_Prop = serializedObject.FindProperty("titleText");
		descriptionText_Prop = serializedObject.FindProperty("descriptionText");
		textButton_Prop = serializedObject.FindProperty("textButton");;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField (root_Prop);

		setByLocalisation.Root root = (setByLocalisation.Root)root_Prop.enumValueIndex;

		switch(root)
		{
			case setByLocalisation.Root.Root_Menu:
				EditorGUILayout.PropertyField (menu_Prop, new GUIContent ("menu"));

				setByLocalisation.Menu menu = (setByLocalisation.Menu)menu_Prop.enumValueIndex;
				switch(menu)
				{
					case setByLocalisation.Menu.Menu_SetupScene:
						EditorGUILayout.PropertyField (setupScene_Prop, new GUIContent ("setupScene"));
						SetProperties (true, true, true);
					break;

					case setByLocalisation.Menu.Menu_Intro:
						EditorGUILayout.PropertyField (intro_Prop, new GUIContent ("intro"));
						SetProperties (false, false, true);
					break;
				}
			break;

			case setByLocalisation.Root.Root_Gameplay:
				EditorGUILayout.PropertyField (gameplay_Prop, new GUIContent("gameplay"));
			break;
		}



		serializedObject.ApplyModifiedProperties ();
	}

	void SetProperties(bool TitleText, bool DescriptionText, bool Buttons)
	{
		if(TitleText)
			EditorGUILayout.PropertyField (titleText_Prop, new GUIContent("titleText"));
		if(DescriptionText)
			EditorGUILayout.PropertyField (descriptionText_Prop, new GUIContent("descriptionText"));
		if(Buttons)
			EditorGUILayout.PropertyField (textButton_Prop, new GUIContent("textButton"),true);
	}
	/*void setState<T>(SerializedProperty State, SerializedProperty prop1 = null, 
					 SerializedProperty prop2 = null, SerializedProperty prop3 = null, SerializedProperty prop4 = null)
	{
		T root = (T)State.enumValueIndex;

		switch(root)
		{
			case 0:
				EditorGUILayout.PropertyField ( prop1, new GUIContent(prop1.name));
				break;
			case 1:
				EditorGUILayout.PropertyField ( prop2, new GUIContent(prop2.name));
				break;
			case 2:
				EditorGUILayout.PropertyField ( prop1, new GUIContent(prop1.name));
				break;
			case 3:
				EditorGUILayout.PropertyField ( prop2, new GUIContent(prop2.name));
				break;
		}
	}*/
}
